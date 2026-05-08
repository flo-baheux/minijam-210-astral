using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Laywelin {
  public abstract class SceneBootstrapper : MonoBehaviour {
    private void Awake() {
      SceneTransitionManager.Instance.RegisterSceneBootstrapper(this);
    }

    public abstract IEnumerator Initialize();
  }
}