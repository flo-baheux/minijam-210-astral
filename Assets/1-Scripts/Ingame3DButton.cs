using System;
using System.Collections.Generic;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class Ingame3DButton : Interactable {
  [SerializeField] private TextMeshProUGUI vText;
  [SerializeField] private Transform buttonMovingPart;
  [SerializeField] private int value = 0;

  public int Value => value;
  private Sequence pressButtonSequence;
  
  private void Awake() {
    vText.text = value.ToString();
    pressButtonSequence = DOTween.Sequence().SetAutoKill(false);
    pressButtonSequence
      .Append(buttonMovingPart.DOLocalMoveY(-0.08f, 0.1f))
      .Append(buttonMovingPart.DOLocalMoveY(0.08f, 0.4f))
      .Pause();
  }

  public override void Interact() {
    base.Interact();
    value = Math.Clamp((value + 1) % 10, 0, 9);
    vText.text = value.ToString();

    pressButtonSequence.Restart();
  }
}
