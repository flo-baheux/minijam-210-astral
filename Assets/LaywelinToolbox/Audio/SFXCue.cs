using System;
using UnityEngine;

namespace Laywelin {
  [Serializable]
  public struct SFXCue {
    public AudioClip audioClip;
    public float offset;
  }
}