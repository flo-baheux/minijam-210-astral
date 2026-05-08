using TMPro;
using UnityEngine;

namespace Laywelin {
  public class UIHoverManager : MonoBehaviour {
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI tooltipText;

    private float hoverTimer;
    private const float hoverTimeToDisplay = 0.2f;
    private bool isTimerOn;

    private bool isTooltipDisplayed;

    private void Start() {
      tooltip.SetActive(false);
    }

    private void Update() {
      hoverTimer -= Time.unscaledDeltaTime;

      if (isTimerOn && hoverTimer <= 0) {
        isTimerOn = false;
        ShowTooltip();
      }
    }

    private void ResetTimer() {
      isTimerOn = true;
      hoverTimer = hoverTimeToDisplay;
    }

    public void SetTooltipComponent(TooltipComponent hoveredTooltip) {
      tooltipText.text = hoveredTooltip.tooltipText;

      ResetTimer();
    }

    public void RemoveTooltipComponent() {
      tooltipText.text = "";
      isTimerOn = false;
      HideTooltip();
    }

    private void ShowTooltip() {
      tooltip.SetActive(true);
    }

    private void HideTooltip() {
      tooltip.SetActive(false);
    }
  }
}