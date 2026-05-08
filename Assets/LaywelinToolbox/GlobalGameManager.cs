using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Laywelin {

  public enum GameplayMode { 
    LOOK_AROUND,
    DOCUMENT,
    INTERACT_UI,
  }

  public class GlobalGameManager : MonoBehaviour {
    public static GlobalGameManager Instance { get; private set; }

    [SerializeField] private InputHandler inputHandler;
    
    public event Action<GameplayMode, GameplayMode> OnGameplayModeChanged;

    private bool isGamePaused;
    private GameplayMode _currentGameplayMode = GameplayMode.LOOK_AROUND;
    public GameplayMode CurrentGameplayMode {
      get => _currentGameplayMode;
      private set {
        if (_currentGameplayMode == value)
          return;
        var previousAction = _currentGameplayMode;
        _currentGameplayMode = value;
        OnGameplayModeChanged?.Invoke(previousAction, _currentGameplayMode);
      }
    }
    
    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);

      FramerateVSyncSetup();
      DOTweenSetup();
      OnGameplayModeChanged += OnGameplayModeChangedHandler;
    }

    private void Start() { 
      ChangeGameplayMode(GameplayMode.LOOK_AROUND);
    }

    private void DOTweenSetup() {
      DOTween.SetTweensCapacity(1000, 500);
      DOTween.defaultAutoKill = true;
      DOTween.defaultTimeScaleIndependent = true;
    }

    private void FramerateVSyncSetup() {
      Application.targetFrameRate = 60;
      QualitySettings.vSyncCount = 1;
    }

    public void ChangeGameplayMode(GameplayMode newAction) {
      CurrentGameplayMode = newAction;
      SetInputHandlerFromGameplayMode();
    }

    private void Update() {
      if (inputHandler.WasPausePressed())
        PauseResumeGame();
    }

    private void OnGameplayModeChangedHandler(GameplayMode previousAction, GameplayMode currentAction) {

    }
    
    public void PauseResumeGame() {
      isGamePaused = !isGamePaused;
      Time.timeScale = isGamePaused ? 0: 1;
      if (isGamePaused)
          inputHandler.SwitchContext(InputContext.UI);
      else
        SetInputHandlerFromGameplayMode();
    }

    public void SetInputHandlerFromGameplayMode() {
      switch (CurrentGameplayMode) {
        case GameplayMode.LOOK_AROUND:
          inputHandler.SwitchContext(InputContext.GAMEPLAY);
          break;
        case GameplayMode.DOCUMENT:
          inputHandler.SwitchContext(InputContext.DOCUMENT);
          break;
        case GameplayMode.INTERACT_UI:
          inputHandler.SwitchContext(InputContext.UI);
          break;
      }
    }

    public void QuitGame() {
#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
  }
}