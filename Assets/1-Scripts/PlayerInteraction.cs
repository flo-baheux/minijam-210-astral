using System;
using Laywelin;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
  [SerializeField] private float maxInteractionDistance = 1.5f;
  [SerializeField] private InputHandler inputHandler;
  [SerializeField] private Transform cameraTransform;
  
  private RaycastHit[] raycastHits = new RaycastHit[1];
  
  public Action<bool> OnCanInteractStateChange;

  private Interactable _interactable;
  public bool CanInteract => _interactable != null;
  private Interactable CurrentInteractable {
    get => _interactable;
    set {
      bool couldInteractBefore = CanInteract;
      _interactable = value;
      
      
      if (CanInteract != couldInteractBefore)
        OnCanInteractStateChange?.Invoke(CanInteract);
    }
  }

  private void Update() {
    DetectInteractable();
    if (CanInteract && inputHandler.WasGameplayInteractPressed())
      CurrentInteractable?.Interact();
  }

  private void DetectInteractable() {
    int hits = Physics.RaycastNonAlloc(cameraTransform.position, cameraTransform.forward, raycastHits, maxInteractionDistance);
    Debug.DrawRay(cameraTransform.position, cameraTransform.forward * maxInteractionDistance, Color.red, 1);
    if (hits > 0 && raycastHits[0].collider.TryGetComponent(out Interactable interactable))
      CurrentInteractable = interactable;
    else
      CurrentInteractable = null;
  }
}