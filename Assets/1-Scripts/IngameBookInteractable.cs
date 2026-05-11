using System;
using DG.Tweening;
using Laywelin;

public class IngameBookInteractable : Interactable {
  public bool active = false;
  public Action<bool> OnStateChange;
  
  public override void Interact() {
    base.Interact();
    active = !active;
    OnStateChange?.Invoke(active);
    transform.DOKill();
    transform.DOLocalMoveY(active ? -1.25f : -1, 0.3f);
  }
}
