using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laywelin {

  public enum GameplayMode { 
    LOOK_AROUND,
    DOCUMENT,
    INTERACT_UI,
  }

  public class GlobalGameManager : MonoBehaviour {
    public static GlobalGameManager Instance { get; private set; }

    [SerializeField] private InputHandler _inputHandler;
    public InputHandler InputHandler => _inputHandler;

    public readonly PlayerInventory PlayerInventory = new();
    [SerializeField] private PlayerSanity playerSanity;
    public PlayerSanity PlayerSanity => playerSanity;
    
    public event Action<GameplayMode, GameplayMode> OnGameplayModeChanged;
    public event Action OnGamePaused, OnGameResumed;
    
    private bool isGamePaused;
    private GameplayMode previousGameplayMode = GameplayMode.LOOK_AROUND;
    private GameplayMode _currentGameplayMode = GameplayMode.LOOK_AROUND;
    public GameplayMode CurrentGameplayMode {
      get => _currentGameplayMode;
      private set {
        if (_currentGameplayMode == value)
          return;
        previousGameplayMode = _currentGameplayMode;
        _currentGameplayMode = value;
        OnGameplayModeChanged?.Invoke(previousGameplayMode, _currentGameplayMode);
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
      GameplayEventManager.AddListener<InteractedWithDocumentEvent>(InteractedWithDocumentHandler);
      GameplayEventManager.AddListener<ClosedDocumentEvent>(ClosedDocumentHandler);
    }

    private void InteractedWithDocumentHandler(InteractedWithDocumentEvent evt) {
      ChangeGameplayMode(GameplayMode.DOCUMENT);
    }

    private void ClosedDocumentHandler(ClosedDocumentEvent evt) { 
      ChangeGameplayMode(previousGameplayMode);
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
      if (InputHandler.WasPausePressed())
        PauseGame();
      
      if (isGamePaused && InputHandler.WasUICancelPressed())
        ResumeGame();
    }
    
    public void PauseGame() {
      isGamePaused = true;
      Time.timeScale = 0;
      InputHandler.SwitchContext(InputContext.UI);
      OnGamePaused?.Invoke();
    }

    public void ResumeGame() {
      isGamePaused = false;
      Time.timeScale = 1;
      SetInputHandlerFromGameplayMode();
      OnGameResumed?.Invoke();
    }

    public void SetInputHandlerFromGameplayMode() {
      switch (CurrentGameplayMode) {
        case GameplayMode.LOOK_AROUND:
          InputHandler.SwitchContext(InputContext.GAMEPLAY);
          break;
        case GameplayMode.DOCUMENT:
          InputHandler.SwitchContext(InputContext.DOCUMENT);
          break;
        case GameplayMode.INTERACT_UI:
          InputHandler.SwitchContext(InputContext.UI);
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