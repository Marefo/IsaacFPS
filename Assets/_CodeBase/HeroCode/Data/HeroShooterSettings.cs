using _CodeBase.ShooterCode;
using _CodeBase.ShooterCode.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.HeroCode.Data
{
  [CreateAssetMenu(fileName = "HeroShooterSettings", menuName = "Settings/Hero/HeroShooter")]
  public class HeroShooterSettings : ScriptableObject
  {
    public float Delay;
    [Space(10)]
    public Bullet ProjectilePrefab;
    public Grenade GrenadePrefab;
    public BulletSettings BulletSettings;
    public GrenadeSettings GrenadeSettings;
  }
}