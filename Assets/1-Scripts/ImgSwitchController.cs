using System;
using System.Collections.Generic;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImgSwitchController : Interactable {
  [SerializeField] private Image img;
  [SerializeField] private Sprite  keyboardSprite, controllerSprite;

  private void Start() {
    OnSwitchDeviceHandler();
    GlobalGameManager.Instance.InputHandler.OnSwitchDevice += OnSwitchDeviceHandler;
  }

  private void OnSwitchDeviceHandler() { 
    img.sprite = GlobalGameManager.Instance.InputHandler.IsUsingGamepad() ? controllerSprite : keyboardSprite;
  }
}
