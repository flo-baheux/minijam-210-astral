using System;
using DG.Tweening;
using UnityEngine;

public class DoubleSlidingDoor : MonoBehaviour {
  [SerializeField] private Transform doorL, doorR;
  [SerializeField] private Vector2 doorLOpenPos, doorLClosePos, doorROpenPos, doorRClosePos;
  
  [SerializeField] private bool isOpen;
  public bool IsOpen => isOpen;

  [SerializeField] private float animationDuration = 0.5f;

  public Action OnOpen, OnClose;
  
  private void Awake() {
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
      .Append(doorL.DOLocalMove(IsOpen ? doorLOpenPos : doorLClosePos, animationDuration))
      .Join(doorR.DOLocalMove(IsOpen ? doorROpenPos : doorRClosePos, animationDuration))
      .OnComplete(() => callback?.Invoke());
  }
}
