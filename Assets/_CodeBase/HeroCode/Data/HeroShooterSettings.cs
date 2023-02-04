using _CodeBase.ShooterCode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.HeroCode.Data
{
  [CreateAssetMenu(fileName = "HeroShooterSettings", menuName = "Settings/Hero/HeroShooter")]
  public class HeroShooterSettings : ScriptableObject
  {
    public float Delay;
    public Bullet ProjectilePrefab;
  }
}