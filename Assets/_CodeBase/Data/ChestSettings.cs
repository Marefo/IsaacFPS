using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.Data
{
  [CreateAssetMenu(fileName = "ChestSettings", menuName = "Settings/Chest")]
  public class ChestSettings : ScriptableObject
  {
    public float LidPushForce;
    [Space(10)]
    public float PunchScaleStrength;
    public float PunchScaleTime;
    public int PunchScaleVibrato;
    [Space(10)]
    public float ShakeStrength;
    public float ShakeTime;
    public int ShakeVibrato;
    public float ShakeDuration;
  }
}