using UnityEngine;

namespace _CodeBase.IndicatorCode.Data
{
  [CreateAssetMenu(fileName = "IndicatorSettings", menuName = "Settings/Indicator")]
  public class IndicatorSettings : ScriptableObject
  {
    public int MaxValue;
    public int StartValue;
  }
}