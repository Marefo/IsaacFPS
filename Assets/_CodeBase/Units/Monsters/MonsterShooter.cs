using _CodeBase.ShooterCode;
using _CodeBase.ShooterCode.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class MonsterShooter : MonoBehaviour
  {
    public void Shoot(Bullet projectilePrefab, Transform shootPoint, Transform target, BulletSettings settings)
    {
      Bullet projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
      Vector3 targetPosition = target.transform.position;
      Vector3 direction = Vector3.Normalize(targetPosition - transform.position);
      projectile.OnShoot(direction, settings);
    }
  }
}