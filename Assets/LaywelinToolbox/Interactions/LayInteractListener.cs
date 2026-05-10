using System;
using UnityEngine;
using UnityEngine.Events;

namespace Laywelin {
  public class InteractListener : MonoBehaviour {
    [SerializeField] private Interactable interactable;
    
    [SerializeField] private UnityEvent editorOnInteractionReceived;
    public event Action<Interactable> OnInteractionReceived = null!;

    private void Awake() {
      interactable.OnInteractedWith += OnInteractedWithHandler;
    }

    protected virtual void OnInteractedWithHandler(Interactable from) {
      OnInteractionReceived?.Invoke(from);
      editorOnInteractionReceived?.Invoke();
    }

    private void OnDestroy() {
      interactable.OnInteractedWith -= OnInteractedWithHandler;
    }
  }
}