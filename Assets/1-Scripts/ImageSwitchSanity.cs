using System;
using DG.Tweening;
using Laywelin;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitchSanity : MonoBehaviour {
  [SerializeField] private int sanityThresholdFirstImage, sanityThresholdSecondImage;
  [SerializeField] private Image img;
  [SerializeField] private Sprite firstImage, secondImage;
  
  private void Awake() {
    img.sprite = null;
    img.color = Color.clear;
  }

  private void Start() { 
    GlobalGameManager.Instance.PlayerSanity.OnSanityReduced += OnSanityReducedHandler;
  }

  private void OnSanityReducedHandler(int sanity) {
    if (sanity <= sanityThresholdFirstImage) {
      img.sprite = firstImage; 
      img.color = Color.white;
    }

    if (sanity <= sanityThresholdSecondImage) {
      img.sprite = secondImage;
      img.color = Color.white;
    }
  }
}
