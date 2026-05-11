using DG.Tweening;
using Laywelin;
using TMPro;
using UnityEngine;

public class SanityUI: MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    
    private void Start() {
      canvasGroup.Toggle(false);

      GlobalGameManager.Instance.PlayerSanity.OnSanityReduced += OnSanityReducedHandler;
    }

    private void OnSanityReducedHandler(int newValue) {
      DOTween.Sequence()
        .Append(canvasGroup.DOFade(1, 0.5f).From(0))
        .AppendInterval(2f)
        .Append(canvasGroup.DOFade(0, 2f).From(1));
    }
}