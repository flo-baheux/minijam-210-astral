using System;
using UnityEngine;

namespace Laywelin {
  public class PlayerSanity: MonoBehaviour {
    [SerializeField] private int maxSanity, startSanity;
    public int currentSanity { get; private set; }

    public Action<int> OnSanityReduced;

    private void Awake() {
      currentSanity = Math.Clamp(startSanity, 0, maxSanity);
    }

    public void ReduceSanity() {
      int sanityBefore = currentSanity;
      currentSanity = Math.Clamp(currentSanity - 1, 0, maxSanity);
      if (currentSanity != sanityBefore)
        OnSanityReduced?.Invoke(currentSanity);
    }
  }
}