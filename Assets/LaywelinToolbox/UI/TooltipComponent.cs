using UnityEngine;
using UnityEngine.EventSystems;

namespace Laywelin {
  public class TooltipComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [TextArea] public string tooltipText;
    [SerializeField] private UIHoverManager hoverTooltip;
    [SerializeField] private CanvasGroup canvasGroup;

    public virtual void OnPointerEnter(PointerEventData eventData) {
      if (transform.localScale == Vector3.zero)
        return;

      if (canvasGroup && canvasGroup.alpha == 0)
        return;

      hoverTooltip.SetTooltipComponent(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
      if (transform.localScale == Vector3.zero)
        return;

      if (canvasGroup && canvasGroup.alpha == 0)
        return;

      hoverTooltip.RemoveTooltipComponent();
    }
  }
}