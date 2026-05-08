using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

namespace Laywelin {
  public class PlayerPrefsApplier : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;

    public static int defaultResolutionIndex = 1;
    public static int defaultFullscreenEnabled = 1;
    public static int defaultDisplayIndex = 0;

    public static float defaultGlobalVolume = 1;
    public static float defaultBGMVolume = 1;
    public static float defaultSFXVolume = 1;

    private void Start() {
      if (PlayerPrefs.HasKey("GlobalVolume"))
        mixer.SetFloat("GlobalVolume", Mathf.Log10(PlayerPrefs.GetFloat("GlobalVolume")) * 20);
      else
        mixer.SetFloat("GlobalVolume", Mathf.Log10(defaultGlobalVolume) * 20);

      if (PlayerPrefs.HasKey("BGMVolume"))
        mixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("BGMVolume")) * 20);
      else
        mixer.SetFloat("BGMVolume", Mathf.Log10(defaultBGMVolume) * 20);

      if (PlayerPrefs.HasKey("SFXVolume"))
        mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
      else
        mixer.SetFloat("SFXVolume", Mathf.Log10(defaultSFXVolume) * 20);

      if (PlayerPrefs.HasKey("Language"))
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("Language")];

      var resIndex = defaultResolutionIndex;
      if (PlayerPrefs.HasKey("ResolutionIndex"))
        resIndex = PlayerPrefs.GetInt("ResolutionIndex");

      List<DisplayInfo> displaysInfo = new();
      Screen.GetDisplayLayout(displaysInfo);

      var displayIndex = defaultDisplayIndex;
      if (PlayerPrefs.HasKey("DisplayIndex"))
        displayIndex = PlayerPrefs.GetInt("DisplayIndex");

      // display saved in player prefs cannot be found
      if (displayIndex >= displaysInfo.Count) {
        displayIndex = defaultDisplayIndex;
        PlayerPrefs.SetInt("DisplayIndex", displayIndex);
      }

      var fullscreenEnabled = defaultFullscreenEnabled == 1;
      if (PlayerPrefs.HasKey("Fullscreen"))
        fullscreenEnabled = PlayerPrefs.GetInt("Fullscreen") == 1;

      int resX, resY;
      switch (resIndex) {
        case 0:
          resX = 2560;
          resY = 1440;
          break;
        case 1:
          resX = 1920;
          resY = 1080;
          break;
        case 2:
          resX = 1366;
          resY = 768;
          break;
        case 3:
          resX = 1280;
          resY = 720;
          break;
        case 4:
          resX = 1280;
          resY = 800;
          break;
        case 5:
          resX = 1440;
          resY = 900;
          break;
        case 6:
          resX = 1600;
          resY = 900;
          break;
        default:
          resX = 1920;
          resY = 1080;
          break;
      }

      Screen.SetResolution(resX, resY, fullscreenEnabled ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
      Screen.MoveMainWindowTo(displaysInfo[displayIndex], new(resX, resY));
    }
  }
}