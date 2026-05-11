using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Laywelin {
  public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource bgmGlobalAudioSource;
    [SerializeField] private AudioSource sfxGlobalAudioSource;
    
    [SerializeField] private AudioClip mainBGM;

    [SerializeField] private AudioClip buttonClickSound, switchOnSound, switchOffSound, openPanelSound, closePanelSound, 
      defaultErrorSound, pickupSound, eerieSound;
    
    public static AudioManager Instance { get; private set; }

    void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    
    public void PlayButtonClickSound() {
      PlayOnceSFX(buttonClickSound);
    }

    public void ToggleSwitchSound(bool toggle) { 
      PlayOnceSFX(toggle? switchOnSound : switchOffSound);
    }

    public void PlayOpenPanelSound() {
      PlayOnceSFX(openPanelSound);
    }

    public void PlayClosePanelSound() {
      PlayOnceSFX(closePanelSound);
    }

    public void PlayErrorSound() {
      PlayOnceSFX(defaultErrorSound);
    }

    public void PlayPickupSound() {
      PlayOnceSFX(pickupSound);
    }

    public void PlayEerieSound() { 
      PlayOnceSFX(eerieSound);
    }

    public void PlayOnceRandomSFX(List<AudioClip> clips, AudioSource source = null) {
      if (clips == null || clips.Count == 0) {
        Debug.LogError("Cannot play random SFX: No audioClip");
        return;
      }

      PlayOnceSFX(clips[Random.Range(0, clips.Count)], source);
    }

    public void PlayOnceSFX(AudioClip clip, AudioSource source = null) {
      if (clip == null) {
        Debug.LogError("Cannot play SFX: missing AudioClip");
        return;
      }

      var sourceToPlayFrom = source == null ? sfxGlobalAudioSource : source;
      if (sourceToPlayFrom == null) {
        Debug.LogError("No AudioSource available for SFX");
        return;
      }
      sourceToPlayFrom.PlayOneShot(clip);
    }

    public void PlayOnceSFX(SFXCue cue, AudioSource source = null) {
      if (cue.offset > 0)
        StartCoroutine(PlayDelayed(cue, source));
      else
        PlayOnceSFX(cue.audioClip, source);
    }

    public void PlayBGM(AudioClip clip, bool fadeTo = false) {
      DOTween.Kill(this);
      var audioSequence = DOTween.Sequence().SetId(this).SetAutoKill(true);

      float BGMVolume = bgmGlobalAudioSource.volume;
      
      if (!fadeTo) {
        bgmGlobalAudioSource.volume = 0;
        bgmGlobalAudioSource.clip = clip;
        bgmGlobalAudioSource.Play();
        audioSequence.Insert(0, bgmGlobalAudioSource.DOFade(BGMVolume, 10f).SetEase(Ease.InOutSine));
        return;
      }

      var fadeOutBGMTmpSource = gameObject.AddComponent<AudioSource>();
        fadeOutBGMTmpSource.clip = bgmGlobalAudioSource.clip;
        fadeOutBGMTmpSource.time = bgmGlobalAudioSource.time;
        fadeOutBGMTmpSource.volume = bgmGlobalAudioSource.volume;
        fadeOutBGMTmpSource.outputAudioMixerGroup = bgmGlobalAudioSource.outputAudioMixerGroup;
        fadeOutBGMTmpSource.Play();

        bgmGlobalAudioSource.volume = 0;
        bgmGlobalAudioSource.clip = clip;
        bgmGlobalAudioSource.Play();

        audioSequence.InsertCallback(0, () => {
          if (fadeOutBGMTmpSource != null)
            fadeOutBGMTmpSource.DOFade(0, 10f).SetEase(Ease.Linear);
        });
        audioSequence.Insert(0,
          bgmGlobalAudioSource.DOFade(BGMVolume, 10f).SetEase(Ease.Linear));

        // Autokill enabled - OnKill will be called anyway whether it gets killed or it completes
        audioSequence.OnKill(() => {
          if (fadeOutBGMTmpSource)
            fadeOutBGMTmpSource.DOFade(0, 2f).OnComplete(() => {
              Destroy(fadeOutBGMTmpSource);
              fadeOutBGMTmpSource = null;
            });
        });
        
    }

    private IEnumerator PlayDelayed(SFXCue cue, AudioSource source) {
      yield return new WaitForSeconds(cue.offset);
      PlayOnceSFX(cue.audioClip, source);
    }
  }
}