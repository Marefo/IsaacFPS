using UnityEngine;

namespace _CodeBase.ShooterCode.Data
{
  [CreateAssetMenu(fileName = "GrenadeSettings", menuName = "Settings/Grenade")]
  public class GrenadeSettings : ScriptableObject
  {
    public int Damage;
    public float Gravity;
    public float Height;
    public float MaxThrowDistance;
    public float MovingThrowDistanceMultiplier;
  }
}