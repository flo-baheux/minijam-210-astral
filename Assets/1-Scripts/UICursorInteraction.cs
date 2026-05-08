using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UICursorInteraction : MonoBehaviour {
  [SerializeField] private PlayerInteraction playerInteraction;
  [SerializeField] private Image cursorIcon;
  [SerializeField] private Color canInteractColor, cannotInteractColor;
  [SerializeField] private float canInteractScale, cannotInteractScale;

  private Tween tween;
  
  private void Awake() {
    playerInteraction.OnCanInteractStateChange += OnInteractStateChangeHandler;

    cursorIcon.color = cannotInteractColor;
    cursorIcon.transform.localScale = Vector3.one * cannotInteractScale;
  }

  private void OnInteractStateChangeHandler(bool canInteract) { 
    tween?.Kill();

    Color targetColor = canInteract ? canInteractColor : cannotInteractColor;
    float targetScale = canInteract ? canInteractScale : cannotInteractScale;

    tween = DOTween.Sequence()
      .Join(cursorIcon.DOColor(targetColor, 1f))
      .Join(cursorIcon.rectTransform.DOScale(targetScale, 1f));
  }
}
