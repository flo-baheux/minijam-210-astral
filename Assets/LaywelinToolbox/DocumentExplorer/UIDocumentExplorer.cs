using System;
using UnityEngine;
using UnityEngine.UI;

namespace Laywelin {
  public class UIDocumentExplorer : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image documentImage;
    [SerializeField] private AudioClip pageChangeSound;
    
    private DocumentScriptableObject loadedDocument;
    private int currentIndex;
    
    private void Awake() {
      canvasGroup.Toggle(false);
    }

    public void ShowDocument(DocumentScriptableObject document, int index = 0) {
      if (loadedDocument == null || loadedDocument.documentImages.Count == 0)
        return;
      loadedDocument = document;
      currentIndex = Math.Clamp(index, 0, loadedDocument.documentImages.Count - 1);
      documentImage.sprite = loadedDocument.documentImages[currentIndex];
      canvasGroup.Toggle(true);
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }
    
    public void HideDocument() {
      loadedDocument = null;
      canvasGroup.Toggle(false);
      currentIndex = 0;
      documentImage.sprite = null;
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }

    public void NextPage() {
      if (loadedDocument == null)
        return;

      int previousIndex = currentIndex;
      currentIndex = Math.Clamp(currentIndex + 1, 0, loadedDocument.documentImages.Count - 1);
      if (previousIndex == currentIndex)
        return;

      documentImage.sprite = loadedDocument.documentImages[currentIndex];
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }

    public void PreviousPage() {
      if (loadedDocument == null)
        return;

      int previousIndex = currentIndex;
      currentIndex = Math.Clamp(currentIndex - 1, 0, loadedDocument.documentImages.Count - 1);
      if (previousIndex == currentIndex)
        return;

      documentImage.sprite = loadedDocument.documentImages[currentIndex];
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }
  }
}
