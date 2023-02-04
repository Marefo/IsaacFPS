using UnityEngine;

namespace _CodeBase.ShooterCode.Data
{
  [CreateAssetMenu(fileName = "BulletSettings", menuName = "Settings/Bullet")]
  public class BulletSettings : ScriptableObject
  {
    public int Damage;
    public float ProjectileSpeed;
    public float MaxShootDistance;
  }
}