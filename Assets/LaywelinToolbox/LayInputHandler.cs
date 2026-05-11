using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Laywelin {
  
  public enum InputContext {
    GAMEPLAY,
    UI,
    DOCUMENT,
    CUTSCENE
  }

  public class InputHandler : MonoBehaviour {
    [Header("Look")]
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private bool invertXAxis, invertYAxis;
    
    private GameInputActions gameInputActions;
    
    
    [NonSerialized] public InputContext inputContext;

    private void Awake() {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      gameInputActions = new();
    }

    private void Start() { 
      InputSystem.onActionChange += OnActionChange;
    }

    public void SwitchContext(InputContext context) {
      inputContext = context;

      gameInputActions.Gameplay.Disable();
      gameInputActions.UI.Disable();
      gameInputActions.Document.Disable();

      switch (inputContext) {
        case InputContext.GAMEPLAY:
          gameInputActions.Gameplay.Enable();
          break;

        case InputContext.UI:
          gameInputActions.UI.Enable();
          break;
          
        case InputContext.DOCUMENT:
          gameInputActions.Document.Enable();
          break;
        
        case InputContext.CUTSCENE:
          break;
      }

      ApplyCursorState();
    }

    private void ApplyCursorState() {
      switch (inputContext) {
        case InputContext.GAMEPLAY:
        case InputContext.CUTSCENE:
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
          break;

        case InputContext.UI:
        case InputContext.DOCUMENT:
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
          break;
      }
    }

    // GAMEPLAY

    public bool WasPausePressed() {
      return inputContext == InputContext.GAMEPLAY && gameInputActions.Gameplay.Pause.WasPressedThisFrame();
    }
    public Vector2 GetMovementInput() {
      if (inputContext != InputContext.GAMEPLAY || !CanProcessGameplayInput())
        return Vector2.zero;

      return gameInputActions.Gameplay.Move.ReadValue<Vector2>();
    }

    public Vector2 GetLookInput() {
      if (inputContext != InputContext.GAMEPLAY || !CanProcessGameplayInput())
        return Vector2.zero;

      Vector2 lookInput = gameInputActions.Gameplay.Look.ReadValue<Vector2>();

      if (invertXAxis)
        lookInput.x *= -1;

      if (invertYAxis)
        lookInput.y *= -1;

      lookInput *= lookSensitivity;

      return lookInput;
    }

    public bool WasGameplayInteractPressed() {
      return CanProcessGameplayInput() && gameInputActions.Gameplay.Interact.WasPressedThisFrame();
    }
    
    // UI

    public Vector2 GetUINavigation() {
      if (!CanProcessUIInput())
        return Vector2.zero;

      return gameInputActions.UI.Navigate.ReadValue<Vector2>();
    }

    public bool WasUISubmitPressed() {
      return CanProcessUIInput() && gameInputActions.UI.Submit.WasPressedThisFrame();
    }

    public bool WasUICancelPressed() {
      return CanProcessUIInput() && gameInputActions.UI.Cancel.WasPressedThisFrame();
    }

    // DOCUMENT

    public bool WasDocumentPreviousPressed() {
      return CanProcessDocumentInput() && gameInputActions.Document.Previous.WasPressedThisFrame();
    }

    public bool WasDocumentNextPressed() {
      return CanProcessDocumentInput() && gameInputActions.Document.Next.WasPressedThisFrame();
    }

    public bool WasDocumentCancelPressed() {
      return CanProcessDocumentInput() && gameInputActions.Document.Cancel.WasPressedThisFrame();
    }

    // "CAN PROCESS" HELPERS

    private bool CanProcessGameplayInput() {
      return Application.isFocused && inputContext == InputContext.GAMEPLAY;
    }

    public bool CanProcessUIInput() {
      return Application.isFocused && inputContext == InputContext.UI;
    }

    public bool CanProcessDocumentInput() {
      return Application.isFocused && inputContext == InputContext.DOCUMENT;
    }

    private void OnDestroy() {
      gameInputActions.Dispose();
    }

    public void OnSensibilityChangedHandler(float value) {
      lookSensitivity = value;
    }


    private InputDevice lastDevice;
    public Action OnSwitchDevice;

    private void OnActionChange(object obj, InputActionChange change) {
      if (change == InputActionChange.ActionPerformed) {
        var action = obj as InputAction;
        if (action == null || action.activeControl == null)
          return;

        var device = action.activeControl.device;

        if (device == lastDevice)
          return;

        bool wasGamepad = lastDevice is Gamepad;
        bool isGamepad = device is Gamepad;

        lastDevice = device;
        if (wasGamepad == isGamepad)
          return;

        OnSwitchDevice?.Invoke();

        lookSensitivity = isGamepad ? 1.5f : 0.15f;
      }
    }

    public bool IsUsingGamepad() {
      return lastDevice is Gamepad;
    }
  }
}

