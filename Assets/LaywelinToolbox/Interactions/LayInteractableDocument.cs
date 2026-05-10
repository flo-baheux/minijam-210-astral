using System;
using UnityEngine;

namespace Laywelin {
  public class InteractableDocument : Interactable {
    [SerializeField] private DocumentScriptableObject documentSO;

    public override void Interact() {
      InteractedWithDocumentEvent evt = new() {
        document = documentSO
      };
      GameplayEventManager.Emit(evt);
      
      base.Interact();
    }
  }
}