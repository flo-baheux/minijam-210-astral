using Laywelin;
using UnityEngine;
using UnityEngine.UI;

public class DocumentIngameDisplayer : MonoBehaviour {

  [SerializeField] private Image image;
  [SerializeField] private DocumentScriptableObject documentSO;

  private void Start() {
    GlobalGameManager.Instance.PlayerSanity.OnSanityReduced += OnSanityReducedHandler;

    image.sprite = documentSO.GetDataBySanityLevel().listSprites[0];
  }

  private void OnSanityReducedHandler(int newValue) {
    image.sprite = documentSO.GetDataBySanityLevel().listSprites[0];
  }
}