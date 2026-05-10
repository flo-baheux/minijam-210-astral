using System;
using Laywelin;
using UnityEngine;

public class UIPause : MonoBehaviour {
  [SerializeField] private CanvasGroup canvasGroup;

  private void Awake() {
    canvasGroup.Toggle(false);
  }

  private void Start() {
    GlobalGameManager.Instance.OnGamePaused += GamePausedHandler;
    GlobalGameManager.Instance.OnGameResumed += GameResumedHandler;
  }

  private void GamePausedHandler() { 
    canvasGroup.Toggle(true);
  }
  
  private void GameResumedHandler() { 
    canvasGroup.Toggle(false);
  }
}
