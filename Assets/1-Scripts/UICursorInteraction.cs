using DG.Tweening;
using Laywelin;
using UnityEngine;
using UnityEngine.UI;

public class UICursorInteraction : MonoBehaviour {
  [SerializeField] private PlayerInteraction playerInteraction;
  [SerializeField] private Image cursorIcon;
  [SerializeField] private Color canInteractColor, cannotInteractColor;
  [SerializeField] private float canInteractScale, cannotInteractScale;
  [SerializeField] private CanvasGroup canvasGroup;

  private Tween tween;

  private void Awake() {
    playerInteraction.OnCanInteractStateChange += OnInteractStateChangeHandler;

    cursorIcon.color = cannotInteractColor;
    cursorIcon.transform.localScale = Vector3.one * cannotInteractScale;
  }

  private void Start() { 
    GlobalGameManager.Instance.OnGameplayModeChanged += OnGameplayModeChangedHandler;
  }

  private void OnGameplayModeChangedHandler(GameplayMode prev, GameplayMode current) {
    canvasGroup.Toggle(current == GameplayMode.LOOK_AROUND);
  }

  private void OnInteractStateChangeHandler(bool canInteract) { 
    tween?.Kill();

    Color targetColor = canInteract ? canInteractColor : cannotInteractColor;
    float targetScale = canInteract ? canInteractScale : cannotInteractScale;

    tween = DOTween.Sequence()
      .Join(cursorIcon.DOColor(targetColor, 0.5f))
      .Join(cursorIcon.rectTransform.DOScale(targetScale, 0.5f));
  }
}
