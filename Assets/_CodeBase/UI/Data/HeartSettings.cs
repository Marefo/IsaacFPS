using UnityEngine;

namespace _CodeBase.UI.Data
{
  [CreateAssetMenu(fileName = "HeartSettings", menuName = "Settings/Heart")]
  public class HeartSettings : ScriptableObject
  {
    public float HeartValue;
    public float Punch;
    public float Duration;
  }
}