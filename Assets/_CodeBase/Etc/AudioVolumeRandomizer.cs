using System;
using _CodeBase.Extensions;
using UnityEngine;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.Etc
{
  public class AudioVolumeRandomizer : MonoBehaviour
  {
    [SerializeField] private Range _range;
    [SerializeField] private AudioSource _audioSource;

    private void Start() => _audioSource.volume = _range.GetRandomValue();
  }
}