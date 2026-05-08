using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Laywelin {
  [CreateAssetMenu(fileName = "DocumentSO", menuName = "Laywelin/SO/Document")]
  public class DocumentScriptableObject: ScriptableObject {
      public bool isDoublePageDocument;
      public List<Sprite> documentImages = new();
  }
}