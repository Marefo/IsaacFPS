using _CodeBase.ShooterCode;
using _CodeBase.ShooterCode.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.HorfCode.Data
{
  [CreateAssetMenu(fileName = "HorfSettings", menuName = "Settings/Monsters/Horf", order = 0)]
  public class HorfSettings : ScriptableObject
  {
    public float AttackCooldown;
    [Space(10)] 
    public float ShakeDuration;
    public float ShakeStrength;
    [Space(10)]
    public LayerMask ObstaclesLayerMask;
    public Bullet BulletPrefab;
    public BulletSettings BulletSettings;
  }
}