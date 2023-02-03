using _CodeBase.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.GaperCode.Data
{
  [CreateAssetMenu(fileName = "GapperSettings", menuName = "Settings/Gapper", order = 0)]
  public class GaperSettings : ScriptableObject
  {
    public float AttackDuration;
    public float AttackCooldown;
    public Range Acceleration;
    public Range MoveSpeed;
  }
}