using UnityEngine;

namespace _CodeBase.Data
{
  [CreateAssetMenu(fileName = "PoopSettings", menuName = "Settings/Poop", order = 0)]
  public class PoopSettings : ScriptableObject
  {
    public float PunchScaleStrength;
    public float PunchScaleTime;
    public ParticleSystem DestroyVfx;
  }
}