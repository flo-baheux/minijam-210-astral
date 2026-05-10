using System.Collections.Generic;
using UnityEngine;

namespace Laywelin {
  [CreateAssetMenu(fileName = "DocumentSO", menuName = "Laywelin/SO/Document")]
  public class DocumentScriptableObject: ScriptableObject {
      public bool isDoublePageDocument;
      public List<Sprite> documentImages = new();
      public int sanityThreshold = 4;
      public List<Sprite> insaneDocumentImages = new();
  }
}