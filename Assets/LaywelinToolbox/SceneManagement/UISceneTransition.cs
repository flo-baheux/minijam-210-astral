using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Laywelin {
  public class UISceneTransition : MonoBehaviour {
    [SerializeField] private RectTransform sceneTransitionContainer;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private TextMeshProUGUI progressPercentageText;

    public void ShowTransition(Action callback = null) {
      sceneTransitionContainer.anchoredPosition = new(1280, sceneTransitionContainer.anchoredPosition.y);
      progressBarFill.fillAmount = 0;
      progressPercentageText.text = "";
      sceneTransitionContainer.DOAnchorPosX(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => callback?.Invoke());
    }

    public void HideTransition(Action callback = null) {
      sceneTransitionContainer.DOAnchorPosX(-1280, 2f).SetEase(Ease.InExpo).OnComplete(() => callback?.Invoke());
    }
  }
}