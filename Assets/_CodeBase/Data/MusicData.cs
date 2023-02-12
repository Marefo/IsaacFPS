using UnityEngine;

namespace _CodeBase.Data
{
  [CreateAssetMenu(fileName = "MusicData", menuName = "StaticData/Music", order = 0)]
  public class MusicData : ScriptableObject
  {
    public AudioClip Main;
    public AudioClip BossShow;
    public AudioClip BossFight;
  }
}