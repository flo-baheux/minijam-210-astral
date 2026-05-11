using System;
using DG.Tweening;
using Laywelin;
using UnityEngine;

public class Chest : Interactable {
  [SerializeField] private Transform topChestPivot, LockFramePivot;
  [SerializeField] private Collider collider;
  [SerializeField] private bool IsLocked = true;
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip lockedDoorAudio, openDoorAudio;

  private void Awake() {
    GetComponent<Collider>().enabled = true;
  }

  public override void Interact() {
    base.Interact();
    TryOpen();
  }

  public void TryOpen() {
    if (IsLocked) {
      topChestPivot.DOShakeRotation(1f, new Vector3(0, 0, 1), 10, 12, false, ShakeRandomnessMode.Harmonic);
      GameplayEventManager.Emit(new LockedNeedItemEvent());
      audioSource.PlayOneShot(lockedDoorAudio);
      
    } else
      OpenChest();
  }

  public void Unlock() {
    if (!IsLocked)
      return;
    IsLocked = true;
    OpenChest();
  }

  private void OpenChest() {
    audioSource.PlayOneShot(openDoorAudio);
        collider.enabled = false;
    
    DOTween.Sequence()
      .Append(LockFramePivot.DOLocalRotate(new(0, 0, -70f), 1f).SetRelative(true))
      .Insert(0.7f, topChestPivot.DOLocalRotate(new (0, 0, -65f), 1f).SetRelative(true))
      .OnComplete(() => {
        canInteract = false;
        collider.enabled = false;
      });
  }
}
