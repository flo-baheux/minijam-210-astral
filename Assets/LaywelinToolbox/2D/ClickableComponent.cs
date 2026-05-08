#nullable enable
using System;
using UnityEngine;

namespace Laywelin {
  /* usage:
  var hits = Lay2D.RaycastCameraToMousePos();

  if (!hits.Any())
    return;

  foreach (var hit in hits)
    if (hit.collider.TryGetComponent(out ClickableComponent clickableComponent))
      clickableComponent.TriggerClick();
      ...
*/

  public abstract class ClickableComponent : MonoBehaviour {
    [SerializeField] private AudioClip? clickSound;
    [SerializeField] private AudioSource? audioSource;
    public Action OnClick = null!;

    private void Awake() {
      if (!GetComponent<Collider2D>())
        Debug.LogError("CLICKABLE COMPONENT - NO COLLIDER FOUND");
    }

    public virtual void TriggerClick() {
      OnClick?.Invoke();
      AudioManager.Instance.PlayOnceSFX(clickSound, audioSource);
    }
  }
}