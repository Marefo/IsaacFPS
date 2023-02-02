using UnityEngine;

namespace _CodeBase.CameraCode.Data
{
  [CreateAssetMenu(fileName = "ShakeSettings", menuName = "Settings/Shake", order = 0)]
  public class ShakeSettings : ScriptableObject
  {
    public float Duration;
    public float Amount;
    public float DecreaseFactor;
  }
}