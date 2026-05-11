using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Laywelin {
  public class UIDocumentExplorer : MonoBehaviour {
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image documentImage;
    [SerializeField] private AudioClip pageChangeSound;
    [SerializeField] private GameObject nbPagesObject;
    [SerializeField] private TextMeshProUGUI nbPageIndicatorText;
    
    private List<Sprite> loadedDocumentImages;
    private int currentIndex;

    private HashSet<SanityDocumentData> alreadyReadDocData = new();
    
    private void Awake() {
      canvasGroup.Toggle(false);
      nbPagesObject.SetActive(false);
      
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
      
      nbPagesObject.SetActive(false);

      var documentData = document.GetDataBySanityLevel();
      loadedDocumentImages = documentData.listSprites;
      if (loadedDocumentImages.Count == 0) {
        Debug.LogError("Cannot show document: empty");
        return;
      }

      currentIndex = Math.Clamp(index, 0, loadedDocumentImages.Count - 1);

      RefreshPrevNextIndicator();
      
      documentImage.sprite = loadedDocumentImages[currentIndex];
      canvasGroup.Toggle(true);
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);

      bool firstTimeReading = alreadyReadDocData.Add(documentData);
      if (documentData.reduceSanityOnRead && firstTimeReading)
        GlobalGameManager.Instance.PlayerSanity.ReduceSanity();

      if (documentData.notificationOnRead.Length > 0)
        if (!documentData.notifOnlyFirstTime || firstTimeReading)
          GameplayEventManager.Emit(new NotificationEvent() { notificationText = documentData.notificationOnRead });

    }

    public void HideDocument() {
      loadedDocumentImages = null;
      canvasGroup.Toggle(false);
      currentIndex = 0;
      documentImage.sprite = null;
      nbPagesObject.SetActive(false);
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
      GameplayEventManager.Emit(new ClosedDocumentEvent());
    }

    public void NextPage() {
      if (loadedDocumentImages == null || loadedDocumentImages.Count == 0)
        return;

      int previousIndex = currentIndex;

      currentIndex = Math.Clamp(currentIndex + 1, 0, loadedDocumentImages.Count - 1);

      RefreshPrevNextIndicator();
      
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

      RefreshPrevNextIndicator();

      if (previousIndex == currentIndex) {
        HideDocument();
        return;
      }

      documentImage.sprite = loadedDocumentImages[currentIndex];
      AudioManager.Instance.PlayOnceSFX(pageChangeSound);
    }

    private void RefreshPrevNextIndicator() {
      nbPageIndicatorText.text = $"{currentIndex + 1} / {loadedDocumentImages.Count}";
      nbPagesObject.SetActive(loadedDocumentImages.Count > 1);
    }
  }
}
