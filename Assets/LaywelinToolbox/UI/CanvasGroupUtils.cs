using UnityEngine;
using UnityEngine.EventSystems;

namespace Laywelin {
  public static class CanvasGroupUtils {
    public static void Toggle(this CanvasGroup cg, bool toggle) {
      cg.alpha = toggle ? 1 : 0;
      cg.interactable = toggle;
      cg.blocksRaycasts = toggle;
    }
  }
}