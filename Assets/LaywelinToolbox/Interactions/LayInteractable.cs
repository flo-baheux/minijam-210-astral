using System;
using UnityEngine;

namespace Laywelin {
  public class Interactable : MonoBehaviour {
    public event Action<Interactable> OnInteractedWith = null!;

    public virtual void Interact() { 
      OnInteractedWith?.Invoke(this);      
    }
  }
}