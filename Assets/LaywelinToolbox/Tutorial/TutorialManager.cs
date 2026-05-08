#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laywelin {
  public class TutorialManager : MonoBehaviour {
    private List<BaseTutorialStep> steps = new();
    [SerializeField] private GameObject tutorialUI = null!;
    [SerializeField] private bool skipTutorial;

    private int stepIndex;

    public bool tutorialCompleted;

    public Action OnStartTutorial = null!, OnTutorialComplete = null!, OnSkipTutorial = null!;

    public void StartTutorial() {
      if (skipTutorial) {
        tutorialCompleted = true;
        OnSkipTutorial?.Invoke();
        return;
      }

      OnStartTutorial?.Invoke();

      stepIndex = 0;
      steps[stepIndex].OnStepCompleted += OnStepCompletedHandler;
      steps[stepIndex].Init(this);
      steps[stepIndex].StepStart();
      tutorialUI.SetActive(true);
    }

    private void NextStep() {
      steps[stepIndex].OnStepCompleted -= OnStepCompletedHandler;
      steps[stepIndex].StepEnd();

      if (stepIndex + 1 < steps.Count) {
        stepIndex++;
        steps[stepIndex].OnStepCompleted += OnStepCompletedHandler;
        steps[stepIndex].Init(this);
        steps[stepIndex].StepStart();
      } else {
        CompleteTutorial();
      }
    }

    private void CompleteTutorial() {
      OnTutorialComplete?.Invoke();

      tutorialUI.SetActive(false);
      tutorialCompleted = true;
    }

    private void OnStepCompletedHandler() {
      NextStep();
    }
  }
}