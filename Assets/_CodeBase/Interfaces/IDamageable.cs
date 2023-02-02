using UnityEngine;

namespace _CodeBase.Interfaces
{
  public interface IDamageable
  {
    void ReceiveDamage(int damageValue, Vector3 position);
  }
}