using System;
using _CodeBase.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _CodeBase.Infrastructure.Services
{
  public class AudioService : MonoBehaviour
  {
    [field: SerializeField] public SfxData SfxData { get; private set; }
    [field: SerializeField] public MusicData MusicData { get; private set; }
    [field: Space(10)] 
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    private const float MusicFadeTime = 0.2f;
    private float _musicDefaultVolume;

    private void Awake() => _musicDefaultVolume = _musicAudioSource.volume;

    public void PlaySfx(AudioClip clip, bool withRandomPitch = false, float volume = 1)
    {
      _sfxAudioSource.volume = volume;
      
      if (withRandomPitch)
        _sfxAudioSource.pitch = Mathf.Lerp(0.9f, 1.1f, Random.value);
      else
        _sfxAudioSource.pitch = 1;

      _sfxAudioSource.PlayOneShot(clip);
    }
    
    public void PlaySfx(AudioSource source, AudioClip clip, bool withRandomPitch = false, float volume = 1)
    {
      source.volume = volume;
      
      if (withRandomPitch)
        source.pitch = Mathf.Lerp(0.9f, 1.1f, Random.value);
      else
        source.pitch = 1;

      source.PlayOneShot(clip);
    }

    public void ChangeMusicTo(AudioClip clip)
    {
      _musicAudioSource.DOKill();
      _musicAudioSource.DOFade(0, MusicFadeTime).OnComplete(() => OnFadeOutComplete(clip)).SetLink(gameObject);
    }

    private void OnFadeOutComplete(AudioClip clip)
    {
      _musicAudioSource.DOKill();
      _musicAudioSource.clip = clip;
      _musicAudioSource.Play();
      _musicAudioSource.DOFade(_musicDefaultVolume, MusicFadeTime).SetLink(gameObject);
    }
  }
}