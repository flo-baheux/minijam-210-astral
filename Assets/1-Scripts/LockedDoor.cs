using System;
using DG.Tweening;
using Laywelin;
using UnityEngine;

public class LockedDoor : Interactable {
  [SerializeField] private Transform doorPivot;
  [SerializeField] private Collider collider;
  [SerializeField] private InventoryItemSO requiredKey;
  
  private void Awake() {
    collider.enabled = true;
    doorPivot.localRotation = Quaternion.identity;
  }

  public override void Interact() {
    base.Interact();
    TryOpen();
  }

  public void TryOpen() {
    if (!GlobalGameManager.Instance.PlayerInventory.ContainsItem(requiredKey)) {
      GameplayEventManager.Emit(new LockedNeedItemEvent(){ itemNeededName = requiredKey.itemName });
      doorPivot.DOShakeRotation(1f, new Vector3(0, 1, 0), 10, 12, false, ShakeRandomnessMode.Harmonic);
      return;
    }
    
    GlobalGameManager.Instance.PlayerInventory.RemoveItem(requiredKey);
    doorPivot.DOLocalRotate(new(0, 95f, 0), 0.4f);
    collider.enabled = false;
  }
}
