using System;
using System.Collections.Generic;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Lock3 : Interactable {
  [SerializeField] private TextMeshProUGUI v1Text, v2Text, v3Text;
  [SerializeField] private CinemachineCamera lockCamera;
  [SerializeField] private int code1, code2, code3;
  [SerializeField] private List<GameObject> inputControls;
  
  private int v1, v2, v3;

  private bool interactingWith = false;

  public UnityEvent OnLockCompleted;
  
  private void Awake() {
    v1 = 0;
    v2 = 0;
    v3 = 0;

    v1Text.text = v1.ToString();
    v2Text.text = v2.ToString();
    v3Text.text = v3.ToString();
    inputControls.ForEach(x => x.SetActive(false));
  }

  public override void Interact() {
    base.Interact();
    interactingWith = true;
    GlobalGameManager.Instance.ChangeGameplayMode(GameplayMode.INTERACT_UI);
    lockCamera.Priority.Value = 100;
    inputControls.ForEach(x => x.SetActive(true));
  }

  private void Update() {
    if (interactingWith == false)
      return;

    if (GlobalGameManager.Instance.InputHandler.WasUICancelPressed()) {
      interactingWith = false;
      lockCamera.Priority.Value = 0;
      GlobalGameManager.Instance.ChangeGameplayMode(GameplayMode.LOOK_AROUND);
    }
  }

  public void ChangeValue1(bool increase) {
    v1 += SanitizeValue(increase ? 1 : -1);
    v1 %= 10;
    v1Text.text = v1.ToString();
    CheckCompleted();
  }

  public void ChangeValue2(bool increase) {
    v2 += SanitizeValue(increase ? 1 : -1);
    v2 %= 10;
    v2Text.text = v2.ToString();
    CheckCompleted();
  }

  public void ChangeValue3(bool increase) {
    v3 += SanitizeValue(increase ? 1 : -1);
    v3Text.text = v3.ToString();
    CheckCompleted();
  }

  private int SanitizeValue(int v) {
    v = Math.Clamp(v % 10, 0, 9);
    return v;
  }

  private void CheckCompleted() {
    if (v1 == code1 && v2 == code2 && v3 == code3) { 
      OnLockCompleted?.Invoke();
      interactingWith = false;
      lockCamera.Priority.Value = 0;
      GlobalGameManager.Instance.ChangeGameplayMode(GameplayMode.LOOK_AROUND);
      canInteract = false;
      inputControls.ForEach(x => x.SetActive(false));
      
      Destroy(gameObject, 1);
    }
  }
}
