using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Laywelin {
  [RequireComponent(typeof(Button))]
  public class UIHoverableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    [SerializeField] private Image backgroundHover;
    private Button button;

    private void Awake() {
      button = GetComponent<Button>();
      button.onClick.AddListener(() => { AudioManager.Instance.PlayButtonClickSound(); });

      if (backgroundHover != null) {
        var c = backgroundHover.color;
        c.a = 0;
        backgroundHover.color = c;
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      OnSelect();
    }

    public void OnSelect(BaseEventData eventData) {
      OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData) {
      OnDeselect();
    }

    public void OnDeselect(BaseEventData eventData) {
      OnDeselect();
    }

    private void OnSelect() {
      var c = backgroundHover.color;
      c.a = 1;
      backgroundHover.color = c;
    }

    private void OnDeselect() {
      var c = backgroundHover.color;
      c.a = 0;
      backgroundHover.color = c;
    }
  }
}