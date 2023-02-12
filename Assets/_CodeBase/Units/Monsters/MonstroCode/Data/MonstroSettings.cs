using _CodeBase.Data;
using _CodeBase.ShooterCode;
using UnityEngine;

namespace _CodeBase.Units.Monsters.MonstroCode.Data
{
  [CreateAssetMenu(fileName = "MonstroSettings", menuName = "Settings/Monsters/Monstro", order = 0)]
  public class MonstroSettings : ScriptableObject
  {
    public Range RoarDelay;
    [Space(10)]
    public Range AttackDelay;
    [Space(10)]
    public Range ShootOffsetZ;
    public float ProjectilePickHeight;
    public float ProjectileGravity;
    [Space(10)]
    public ParticleSystem JumpImpactVfxPrefab;
    public MonstroProjectile ProjectilePrefab;
    public Grenade Grenade;
  }
}