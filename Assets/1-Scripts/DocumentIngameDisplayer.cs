using Laywelin;
using UnityEngine;
using UnityEngine.UI;

public class DocumentIngameDisplayer : MonoBehaviour {

  [SerializeField] private Image image;
  [SerializeField] private DocumentScriptableObject documentSO;

  private void Start() {
    GlobalGameManager.Instance.PlayerSanity.OnSanityReduced += OnSanityReducedHandler;
    image.sprite = GlobalGameManager.Instance.PlayerSanity.currentSanity <= documentSO.sanityThreshold
      ? documentSO.insaneDocumentImages[0]
      : documentSO.documentImages[0];
  }

  private void OnSanityReducedHandler(int newValue) {
    image.sprite = GlobalGameManager.Instance.PlayerSanity.currentSanity <= documentSO.sanityThreshold
      ? documentSO.insaneDocumentImages[0]
      : documentSO.documentImages[0];
  }
}