using System.Collections.Generic;
using System.Linq;
using Laywelin;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UISettingsView : MonoBehaviour {
  [SerializeField] private AudioMixer mixer;

  [SerializeField] private Slider GlobalVolumeSlider, BGMVolumeSlider, SFXVolumeSlider;
  [SerializeField] private TMP_Dropdown LanguageDropdown, ResolutionDropdown, DisplaysDropdown;
  [SerializeField] private Toggle FullScreenToggle;

  private readonly List<DisplayInfo> displaysInfo = new();

  private void Start() {
    GlobalVolumeSlider.value =
      PlayerPrefs.HasKey("GlobalVolume") ? PlayerPrefs.GetFloat("GlobalVolume") : PlayerPrefsApplier.defaultGlobalVolume;
    BGMVolumeSlider.value = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : PlayerPrefsApplier.defaultBGMVolume;
    SFXVolumeSlider.value = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : PlayerPrefsApplier.defaultSFXVolume;

    if (PlayerPrefs.HasKey("Language"))
      LanguageDropdown.value = PlayerPrefs.GetInt("Language");

    Screen.GetDisplayLayout(displaysInfo);
    DisplaysDropdown.AddOptions(displaysInfo.Select((displayInfo, index) => {
      var optionString = $"{index + 1}";

      if (!string.IsNullOrEmpty(displayInfo.name))
        optionString += $": {displayInfo.name}";

      return new TMP_Dropdown.OptionData(optionString);
    }).ToList());

    DisplaysDropdown.value =
      PlayerPrefs.HasKey("DisplayIndex") ? PlayerPrefs.GetInt("DisplayIndex") : PlayerPrefsApplier.defaultDisplayIndex;

    ResolutionDropdown.value = PlayerPrefs.HasKey("ResolutionIndex")
      ? PlayerPrefs.GetInt("ResolutionIndex")
      : PlayerPrefsApplier.defaultResolutionIndex;

    FullScreenToggle.isOn = PlayerPrefs.HasKey("Fullscreen")
      ? PlayerPrefs.GetInt("Fullscreen") == 1
      : PlayerPrefsApplier.defaultFullscreenEnabled == 1;
  }

  public void OnGlobalVolumeChanged() {
    PlayerPrefs.SetFloat("GlobalVolume", GlobalVolumeSlider.value);
    mixer.SetFloat("GlobalVolume", Mathf.Log10(GlobalVolumeSlider.value) * 20);
  }

  public void OnBGMVolumeChanged() {
    PlayerPrefs.SetFloat("BGMVolume", BGMVolumeSlider.value);
    mixer.SetFloat("BGMVolume", Mathf.Log10(BGMVolumeSlider.value) * 20);
  }

  public void OnSFXVolumeChanged() {
    PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
    mixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
  }

  public void OnLanguageDropdownValueChanged() {
    PlayerPrefs.SetInt("Language", LanguageDropdown.value);
    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LanguageDropdown.value];
  }

  public void OnDisplaysDropdownValueChanged() {
    var newScreenIndex = DisplaysDropdown.value;
    if (newScreenIndex >= displaysInfo.Count)
      return;

    PlayerPrefs.SetInt("DisplayIndex", newScreenIndex);
    var (resX, resY) = GetResolutionFromResolutionDropDownValue();
    Screen.MoveMainWindowTo(displaysInfo[newScreenIndex], new(resX, resY));
  }

  public void OnScreenResolutionDropdownValueChanged() {
    var (resX, resY) = GetResolutionFromResolutionDropDownValue();

    PlayerPrefs.SetInt("ResolutionIndex", ResolutionDropdown.value);
    Screen.SetResolution(resX, resY, FullScreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
  }

  public void OnFullScreenToggleChange() {
    PlayerPrefs.SetInt("Fullscreen", FullScreenToggle.isOn ? 1 : 0);

    var (resX, resY) = GetResolutionFromResolutionDropDownValue();

    Screen.SetResolution(resX, resY, FullScreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
  }

  private (int, int) GetResolutionFromResolutionDropDownValue() {
    int resX = 1920, resY = 1080;

    switch (ResolutionDropdown.value) {
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

    return (resX, resY);
  }
}