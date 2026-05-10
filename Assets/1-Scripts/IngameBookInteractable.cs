using System;
using System.Collections.Generic;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

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
