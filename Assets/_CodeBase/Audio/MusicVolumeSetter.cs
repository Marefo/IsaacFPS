using System;
using _CodeBase.Audio.Data;
using UnityEngine;

namespace _CodeBase.Audio
{
  public class MusicVolumeSetter : MonoBehaviour
  {
    [SerializeField] private AudioSource _musicSource;
    [Space(10)] 
    [SerializeField] private AudioData _audioData;

    private void Update() => _musicSource.volume = _audioData.MusicVolume;
  }
}