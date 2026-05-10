using System;
using DG.Tweening;
using Laywelin;
using UnityEngine;

public class DoubleDoor : MonoBehaviour {
  [SerializeField] private Transform doorL, doorR;
  [SerializeField] private Vector2 doorLOpenPos, doorLClosePos, doorROpenPos, doorRClosePos;
  
  [SerializeField] private bool isDefaultOpen;
  public bool isOpen;

  [SerializeField] private InventoryItemSO keyToOpen;
  
  [SerializeField] private float animationDuration = 0.5f;

  public Action OnOpen, OnClose;
  
  private void Awake() {
    isOpen = isDefaultOpen;
    doorL.transform.localPosition = isOpen ? doorLOpenPos : doorLClosePos;
    doorR.transform.localPosition = isOpen ? doorROpenPos : doorRClosePos;
  }

  public void Toggle() {
    if (isOpen)
      Close();
    else
      Open();
  }

  public void Open() {
    if (isOpen)
      return;
    
    isOpen = true;
    
    Animate();
    OnOpen?.Invoke();
  }

  public void Close() {
    if (!isOpen)
      return;
  
    isOpen = false;
    Animate();
    OnClose?.Invoke();
  }

  private void Animate(Action callback = null) {
    doorL.DOKill();
    doorR.DOKill();
    DOTween.Sequence()
      .Append(doorL.DOLocalMove(isOpen ? doorLOpenPos : doorLClosePos, animationDuration))
      .Join(doorR.DOLocalMove(isOpen ? doorROpenPos : doorRClosePos, animationDuration))
      .OnComplete(() => callback?.Invoke());
  }
}
