using System;
using DG.Tweening;
using UnityEngine;

namespace Laywelin {
  public class DayNightManager : MonoBehaviour {
    [SerializeField] private Light sunlight = null!;
    [SerializeField] private Gradient sunlightColorInTime = null!;
    [SerializeField] private AnimationCurve sunlightIntensityInTime = null!;
    [SerializeField] private float lightIntensityDayStarts = 0.6f;

    public float dayLengthSecs = 420;
    public float currentTime;

    public Action OnDayStarts = null!;
    public Action OnNightStarts = null!;

    public bool IsDay => sunlight.intensity >= lightIntensityDayStarts;
    public bool IsNight => sunlight.intensity < lightIntensityDayStarts;

    private bool isTimeLocked;

    private void Awake() {
      EvaluateLightIntensity();
      EvaluateLightColor();
    }

    private void Update() {
      var wasDay = IsDay;

      if (!isTimeLocked)
        currentTime += Time.deltaTime;

      currentTime %= dayLengthSecs;

      EvaluateLightIntensity();
      EvaluateLightColor();

      if (!wasDay && IsDay)
        OnDayStarts?.Invoke();
      else if (wasDay && !IsDay)
        OnNightStarts?.Invoke();
    }

    private void EvaluateLightIntensity() {
      var intensity = Mathf.Round(sunlightIntensityInTime.Evaluate(currentTime / dayLengthSecs) * 100f) / 100f;
      if (!Mathf.Approximately(intensity, sunlight.intensity))
        sunlight.intensity = intensity;
    }

    private void EvaluateLightColor() {
      var color = sunlightColorInTime.Evaluate(currentTime / dayLengthSecs);

      if (color != sunlight.color)
        sunlight.color = color;
    }

    public void SetLockTime01(float time, float duration, bool locked) {
      bool wasDay = IsDay, wasNight = IsNight;
      DOVirtual.Float(currentTime, dayLengthSecs * time, duration, x => { currentTime = x; });
      isTimeLocked = locked;

      if (!wasDay && IsDay)
        OnDayStarts?.Invoke();
      if (!wasNight && IsNight)
        OnNightStarts?.Invoke();
    }
  }
}