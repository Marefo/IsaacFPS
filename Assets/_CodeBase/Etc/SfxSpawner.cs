using System.Collections.Generic;
using _CodeBase.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.Etc
{
  public class SfxSpawner : MonoBehaviour
  {
    [SerializeField] private List<GameObject> _sfxPrefabs;
    [SerializeField] private bool _destroyWhenDone = true;
    [SerializeField, Range(0.01f, 10f)] private float _pitchRandomMultiplier = 1f;
    
    private void Start()
    {
      GameObject sfx = Instantiate(_sfxPrefabs.GetRandomValue(), transform.position, Quaternion.identity);
      AudioSource audioSource = sfx.GetComponent<AudioSource>();
      
      if (_pitchRandomMultiplier != 1)
      {
        if (Random.value < .5)
          audioSource.pitch *= Random.Range(1 / _pitchRandomMultiplier, 1);
        else
          audioSource.pitch *= Random.Range(1, _pitchRandomMultiplier);
      }

      if (_destroyWhenDone)
      {
        float life = audioSource.clip.length / audioSource.pitch;
        Destroy(sfx, life);
      }
    }
  }
}