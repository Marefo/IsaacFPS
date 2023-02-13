using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Audio.Data
{
  [CreateAssetMenu(fileName = "AudioData", menuName = "StaticData/Audio", order = 0)]
  public class AudioData : ScriptableObject
  {
    public float MusicVolume;
    public float DefaultMusicVolume = 0.18f;

    public void SetMusicVolume(float volume) => MusicVolume = volume;

    [Button]
    public void ResetMusicVolume() => MusicVolume = DefaultMusicVolume;
  }
}