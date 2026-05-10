using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Laywelin {
  public class UIDocumentExplorer : MonoBehaviour {
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image documentImage;
    [SerializeField] private AudioClip pageChangeSound;
    
    private List<Sprite> loadedDocumentImages;
    private int currentIndex;
    
    private void Awake() {
      canvasGroup.Toggle(false);
      GameplayEventManager.AddListener<InteractedWithDocumentEvent>(InteractedWithDocumentHandler);
    }

    private void InteractedWithDocumentHandler(InteractedWithDocumentEvent evt) { 
      ShowDocument(evt.document);
    }

    private void Update() {
      if (loadedDocumentImages == null)
        return;

      if (inputHandler.WasDocumentCancelPressed()) {
        HideDocument();
        return;
      }

      if (inputHandler.WasDocumentNextPressed()) {
        NextPage();
        return;
      }

      if (inputHandler.WasDocumentPreviousPressed()) {
        PreviousPage();
        return;
      }
    }

    public void ShowDocument(DocumentScriptableObject document, int index = 0) {
      if (document == null) {
        Debug.LogError("Cannot show document: null");
        return;
      }

      loadedDocumentImages = (GlobalGameManager.Instance.PlayerSanity.currentSanity <= document.sanityThreshold 
        ? document.insaneDocumentImages 
        : document.documentImages).ToList();
      if (loadedDocumentImages.Count == 0) {
        Debug.LogError("Cannot show document: empty");
        return;
      }
      
      currentIndex = Math.Clamp(index, 0, loadedDocumentImages.Count - 1);
      documentImage.sprite = loadedDocumentImages[currentIndex];
      canvasGroup.Toggle(true);
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }

    public void HideDocument() {
      loadedDocumentImages = null;
      canvasGroup.Toggle(false);
      currentIndex = 0;
      documentImage.sprite = null;
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
      GameplayEventManager.Emit(new ClosedDocumentEvent());
    }

    public void NextPage() {
      if (loadedDocumentImages == null || loadedDocumentImages.Count == 0)
        return;

      int previousIndex = currentIndex;

      currentIndex = Math.Clamp(currentIndex + 1, 0, loadedDocumentImages.Count - 1);
      if (previousIndex == currentIndex)
        return;

      documentImage.sprite = loadedDocumentImages[currentIndex];
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }

    public void PreviousPage() {
      if (loadedDocumentImages == null || loadedDocumentImages.Count == 0)
        return;

      int previousIndex = currentIndex;
      currentIndex = Math.Clamp(currentIndex - 1, 0, loadedDocumentImages.Count - 1);
      if (previousIndex == currentIndex)
        return;

      documentImage.sprite = loadedDocumentImages[currentIndex];
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }
  }
}
