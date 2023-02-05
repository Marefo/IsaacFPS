using _CodeBase.ShooterCode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.Units.Monsters.PooterCode.Data
{
  [CreateAssetMenu(fileName = "PooterSettings", menuName = "Settings/Monsters/Pooter")]
  public class PooterSettings : ScriptableObject
  {
    [FormerlySerializedAs("ShootDelay")] public float AttackDelay;
    public float EscapeDistance;
    public Bullet ProjectilePrefab;
  }
}