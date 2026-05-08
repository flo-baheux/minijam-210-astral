using Laywelin;
using UnityEngine;

public class PlayerMovementControls : MonoBehaviour {

  [SerializeField] private CharacterController characterController;
  [SerializeField] private InputHandler inputHandler;
  [SerializeField] private Transform cameraTransform;
  [SerializeField] private float moveSpeed;

  private float cameraTilt;

  private void Update() {
    HandleCameraLookAround();
    HandleMovements();
  }

  private Vector2 smoothLook;

  private void HandleCameraLookAround() {
    Vector2 lookInput = inputHandler.GetLookInput();
    smoothLook = Vector2.Lerp(smoothLook, lookInput, 0.3f);

    transform.Rotate(new(0f, smoothLook.x, 0f), Space.Self);
  
    cameraTilt = Mathf.Clamp(cameraTilt - smoothLook.y, -70f, 70f);
    cameraTransform.localEulerAngles = new(cameraTilt, 0, 0);
  }
  
private void HandleMovements() {
    Vector2 moveInput = inputHandler.GetMovementInput() * moveSpeed;
    Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
    characterController.SimpleMove(move);
  }
}