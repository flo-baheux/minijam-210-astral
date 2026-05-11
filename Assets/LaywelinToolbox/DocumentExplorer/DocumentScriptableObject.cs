using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laywelin {
  [Serializable]
  public struct SanityDocumentData {
    public int sanityThreshold;
    public List<Sprite> listSprites;
    public string notificationOnRead;
    public bool notifOnlyFirstTime, reduceSanityOnRead;
  }

  public struct SanitySprite {
    public int sanityThreshold;
    public Sprite sprite;
  }

  [CreateAssetMenu(fileName = "DocumentSO", menuName = "Laywelin/SO/Document")]
  public class DocumentScriptableObject: ScriptableObject {
    public List<SanityDocumentData> listDocumentDataBySanityLevel;
    
    public SanityDocumentData GetDataBySanityLevel() {
      int currentSanity = GlobalGameManager.Instance.PlayerSanity.currentSanity;
      return listDocumentDataBySanityLevel.OrderBy(x => x.sanityThreshold).First(x => currentSanity <= x.sanityThreshold);
    }
  }
}