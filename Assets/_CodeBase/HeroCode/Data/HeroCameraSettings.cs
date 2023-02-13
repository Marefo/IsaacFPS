using _CodeBase.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.HeroCode.Data
{
  [CreateAssetMenu(fileName = "HeroCameraSettings", menuName = "Settings/Hero/Camera", order = 0)]
  public class HeroCameraSettings : ScriptableObject
  {
    public Range MinMaxSensitivity;
    public float DefaultSensitivity;
    [Space(10)]
    public float Sensitivity;

    [Button]
    public void ResetSensitivity() => Sensitivity = DefaultSensitivity;
  }
}