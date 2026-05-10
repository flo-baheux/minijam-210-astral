using System;
using UnityEngine;
using UnityEngine.UI;

namespace Laywelin {
  public class InteractedWithDocumentEvent: GameplayEvent {
    public DocumentScriptableObject document;
  }

  public class ClosedDocumentEvent : GameplayEvent {}
}
