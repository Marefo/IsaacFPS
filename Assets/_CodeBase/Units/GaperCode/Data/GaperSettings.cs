using UnityEngine;

namespace _CodeBase.Units.GaperCode.Data
{
  [CreateAssetMenu(fileName = "GapperSettings", menuName = "Settings/Gapper", order = 0)]
  public class GaperSettings : ScriptableObject
  {
    public float AttackDuration;
    public float AttackCooldown;
  }
}