using System;
using System.Collections.Generic;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

public class TelescopeController : Interactable {
  [SerializeField] private Ingame3DButton b1,b2,b3,b4;

  [SerializeField] private CinemachineCamera camera;
  [SerializeField] private Image img, fadeToBlackImg;
  [SerializeField] private CanvasGroup canvasGroup, thanksForPlayingCanvas;
  [SerializeField] private string coordConstellations, coordInsaneEnding;
  [SerializeField] private InventoryItemSO requiredItemEnding;
  [SerializeField] private Sprite constellationsSprite, endingSprite;
  
  private bool interactingWith = false;

  private bool sanityReducedConstellations = false, lockAllEnding = false;
  private void Awake() {
    canvasGroup.Toggle(false);
  }

  public override void Interact() {
    base.Interact();
    interactingWith = true;
    camera.Priority.Value = 100;
    GlobalGameManager.Instance.ChangeGameplayMode(GameplayMode.INTERACT_UI);
    canvasGroup.Toggle(true);
    canvasGroup.DOFade(1, 0.2f).From(0).SetDelay(0.5f);
    
    string coords = $"{b1.Value}{b2.Value}{b3.Value}{b4.Value}";

    fadeToBlackImg.DOFade(0, 0.2f).From(1).SetDelay(0.5f);
    if (coords == coordConstellations) {
      img.sprite = constellationsSprite;
      img.color = Color.white;

      DOVirtual.DelayedCall(1f,
        () => { GameplayEventManager.Emit(new NotificationEvent() { notificationText = $"Oh my... He's right, stars are missing!?" }); });
      if (!sanityReducedConstellations) {
        GlobalGameManager.Instance.PlayerSanity.ReduceSanity();
        sanityReducedConstellations = true;

      }
    } else if (coords == coordInsaneEnding) {
      if (GlobalGameManager.Instance.PlayerInventory.ContainsItem(requiredItemEnding)) {
        lockAllEnding = true;
        img.sprite = endingSprite;
        img.color = Color.white;
        DOVirtual.DelayedCall(1f,
          () => { GameplayEventManager.Emit(new NotificationEvent() { notificationText = $"He was right... Eric will eat us all! We are doomed!" }); });

        DOVirtual.DelayedCall(3.5f, 
          () => fadeToBlackImg.DOFade(1, 2f)
            .OnComplete(() => thanksForPlayingCanvas.DOFade(1, 1f)));

      } else {
        img.sprite = null;
        img.color = Color.clear;
        DOVirtual.DelayedCall(1f,
          () => { GameplayEventManager.Emit(new NotificationEvent() { notificationText = $"I need something to see what he found." }); });
      }

    } else {
      img.sprite = null;
      img.color = Color.clear;
      DOVirtual.DelayedCall(1f, () => { 
        GameplayEventManager.Emit(new NotificationEvent() { notificationText = $"Hm... Nothing at those coordinates." });
      });
    }
  }

  private void Update() {
    if (interactingWith == false)
      return;

    if (lockAllEnding)
      return;

    if (GlobalGameManager.Instance.InputHandler.WasUICancelPressed()) {
      interactingWith = false;
      camera.Priority.Value = 0;
      canvasGroup.Toggle(false);
      
      GlobalGameManager.Instance.ChangeGameplayMode(GameplayMode.LOOK_AROUND);
    }
  }
}