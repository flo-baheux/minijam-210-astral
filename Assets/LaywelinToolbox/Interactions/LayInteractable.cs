using System;
using UnityEngine;

namespace Laywelin {
  public class Interactable : MonoBehaviour {
    [SerializeField] protected bool canInteract = true;
    public bool CanInteract => canInteract;
    public event Action<Interactable> OnInteractedWith = null!;
    
    public virtual void Interact() {
      if (!canInteract)
        return;
      OnInteractedWith?.Invoke(this);
    }
  }
}