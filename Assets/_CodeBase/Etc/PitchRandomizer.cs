using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _CodeBase.Etc
{
  public class PitchRandomizer : MonoBehaviour
  {
    [SerializeField] private AudioSource _audioSource;

    private void Start() => _audioSource.pitch = Mathf.Lerp(0.9f, 1.1f, Random.value);
  }
}