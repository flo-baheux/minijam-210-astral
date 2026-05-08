#nullable enable
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Laywelin {
  public abstract class BaseTutorialStep : MonoBehaviour {
    [SerializeField] private Button skipButton = null!;
    [SerializeField] private bool skippable;

    protected TutorialManager tutorialManager = null!;
    protected bool isStepActive;

    public Action OnStepCompleted = null!;

    private void Awake() {
      if (skipButton != null) {
        skipButton.onClick.AddListener(Complete);
        skipButton.gameObject.SetActive(skippable);
#if UNITY_EDITOR
        skipButton.gameObject.SetActive(true);
#endif
      }

      transform.localScale = Vector3.zero;
    }

    public void Init(TutorialManager tutorialManagerInstance) {
      tutorialManager = tutorialManagerInstance;
    }

    public virtual void StepStart() {
      isStepActive = true;
      Toggle(true);
    }

    public virtual void StepEnd() {
      isStepActive = false;
      Toggle(false);
    }

    public void Toggle(bool show) {
      transform.DOKill();
      transform.DOScale(show ? Vector3.one : Vector3.zero, 0.5f).SetId(transform).OnComplete(() => {
        if (!show)
          gameObject.SetActive(false);
      });
    }

    public void Complete() {
      OnStepCompleted?.Invoke();
    }
  }
}