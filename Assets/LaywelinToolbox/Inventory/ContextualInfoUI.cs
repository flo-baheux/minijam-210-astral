using DG.Tweening;
using Laywelin;
using TMPro;
using UnityEngine;

public class ContextualInfoUI: MonoBehaviour {
    [SerializeField] private RectTransform transform;
    [SerializeField] private TextMeshProUGUI sanityText;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private void Start() {
      canvasGroup.Toggle(false);
      sanityText.text = "";
      GameplayEventManager.AddListener<LockedNeedItemEvent>(LockedNeedItemEventHandler);
      GameplayEventManager.AddListener<NotificationEvent>(NotificationEventHandler);
    }

    private void LockedNeedItemEventHandler(LockedNeedItemEvent evt) {
      sanityText.text = $"It's locked.";
      if (evt.itemNeededName != null && evt.itemNeededName.Length > 0)
        sanityText.text += $" I need the {evt.itemNeededName}.";

      DOTween.Kill(this);
      DOTween.Sequence().SetId(this)
        .Append(canvasGroup.DOFade(1, 0.4f).From(0))
        .AppendInterval(sanityText.text.Split(" ").Length * 0.2f + 0.5f)
        .Append(canvasGroup.DOFade(0, 0.4f));
    }

    private void NotificationEventHandler(NotificationEvent evt) {
      sanityText.text = evt.notificationText;

      DOTween.Kill(this);
      DOTween.Sequence().SetId(this)
        .Append(canvasGroup.DOFade(1, 0.4f).From(0))
        .AppendInterval(sanityText.text.Split(" ").Length * 0.2f + 2f)
        .Append(canvasGroup.DOFade(0, 0.4f));
    }


}