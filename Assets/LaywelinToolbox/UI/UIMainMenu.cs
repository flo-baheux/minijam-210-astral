using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Laywelin {
  public class UIMainMenu : MonoBehaviour {
    
    [SerializeField] private RectTransform mainMenuContainer, settingsContainer, creditsContainer;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup, settingsCanvasGroup, creditsCanvasGroup;
    
    [SerializeField] private Vector2 mainMenuOpenPos, mainMenuClosePos;
    [SerializeField] private Vector2 settingsOpenPos, settingsClosePos;
    [SerializeField] private Vector2 creditsOpenPos, creditsClosePos;
    
    [SerializeField] private string mainGameScene;
    
    private void Awake() {
      ShowOnlyMainMenu();
    }

    public void ShowOnlyMainMenu() {
      mainMenuCanvasGroup.Toggle(true);
      settingsCanvasGroup.Toggle(false);
      creditsCanvasGroup.Toggle(false);
    }

    public void ShowOnlySettings() {
      mainMenuCanvasGroup.Toggle(false);
      settingsCanvasGroup.Toggle(true);
      creditsCanvasGroup.Toggle(false);
    }

    public void ShowOnlyCredits() {
      mainMenuCanvasGroup.Toggle(false);
      settingsCanvasGroup.Toggle(false);
      creditsCanvasGroup.Toggle(true);
    }

    public void PlayGame() {
      SceneTransitionManager.Instance.TransitionToScene(mainGameScene);
    }

        public void QuitGame() {
#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
  }
}