using System;
using Laywelin;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
  [SerializeField] private float maxInteractionDistance = 1.5f;
  [SerializeField] private InputHandler inputHandler;
  [SerializeField] private Transform cameraTransform;
  
  public Action<bool> OnCanInteractStateChange;

  private Interactable interactable;

  private bool _canInteract;
  public bool CanInteract {
    get => _canInteract;
    set {
      bool prevCanInteract = _canInteract;
      _canInteract = value;
      if (_canInteract != prevCanInteract)
        OnCanInteractStateChange?.Invoke(_canInteract);
    }
  }

  private void Update() {
    DetectInteractable();
    if (CanInteract && inputHandler.WasGameplayInteractPressed())
      interactable?.Interact();
  }

  private void DetectInteractable() {
    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, maxInteractionDistance)
        && hit.collider.TryGetComponent(out interactable)
        && interactable.CanInteract)
      CanInteract = true;
    else {
      CanInteract = false;
      interactable = null;
    }

  }
}