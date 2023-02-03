using System;
using _CodeBase.Etc;
using _CodeBase.Interfaces;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class Monster : MonoBehaviour, IDamageable
  {
    public event Action Initialized;
    public event Action<Monster> Dead;
    
    public bool IsDead { get; private set; }
    public TriggerListener RoomZone { get; private set; }

    public void Initialize(TriggerListener roomZone)
    {
      RoomZone = roomZone;
      Initialized?.Invoke();
    }

    public virtual void ReceiveDamage(int damageValue, Vector3 position)
    {
      
    }

    protected virtual void Die()
    {
      IsDead = true;
      Dead?.Invoke(this);
    }
  }
}