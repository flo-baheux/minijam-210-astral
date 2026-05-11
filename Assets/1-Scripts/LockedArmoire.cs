using System;
using DG.Tweening;
using Laywelin;
using UnityEngine;

public class LockedArmoire : Interactable {
  [SerializeField] private Transform pivotL, pivotR;
  [SerializeField] private Collider collider;
  [SerializeField] private bool IsLocked = true;
  [SerializeField] private InventoryItemSO requiredKey;
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip lockedDoorAudio, openDoorAudio;

  private void Awake() {
    collider.enabled = true;
    pivotL.localRotation = Quaternion.identity;
    pivotR.localRotation = Quaternion.identity;
  }

  public override void Interact() {
    base.Interact();
    TryOpen();
  }

  public void TryOpen() {
    if (!GlobalGameManager.Instance.PlayerInventory.ContainsItem(requiredKey)) {
      GameplayEventManager.Emit(new LockedNeedItemEvent() { itemNeededName = requiredKey.itemName });
      pivotL.DOShakeRotation(1f, new Vector3(0, 1, 0), 10, 12, false, ShakeRandomnessMode.Harmonic);
      pivotR.DOShakeRotation(1f, new Vector3(0, 1, 0), 10, 12, false, ShakeRandomnessMode.Harmonic);
      audioSource.PlayOneShot(lockedDoorAudio);
      
      return;
    }

    GlobalGameManager.Instance.PlayerInventory.RemoveItem(requiredKey);
    pivotL.DOLocalRotate(new(0, 70f, 0), 0.4f);
    pivotR.DOLocalRotate(new(0, -70f, 0), 0.4f);
      
    GlobalGameManager.Instance.PlayerSanity.ReduceSanity();
    GameplayEventManager.Emit(new NotificationEvent() { notificationText = "He was... obsessing over something?" });
    
    audioSource.PlayOneShot(openDoorAudio);
    
    collider.enabled = false;
  }
}
