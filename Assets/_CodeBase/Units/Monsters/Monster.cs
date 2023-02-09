using System;
using _CodeBase.Etc;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters
{
  public class Monster : MonoBehaviour, IDamageable
  {
    public event Action Initialized;
    public event Action<Monster> Dead;
    
    [field: SerializeField] public bool HasSpawnOffsetY { get; private set; }
    [field: ShowIf("HasSpawnOffsetY"), SerializeField] public float SpawnOffsetY { get; private set; }
    [Space(10)]
    [SerializeField] private Health _health;
    
    public bool IsDead { get; private set; }
    public TriggerListener RoomZone { get; private set; }
    
    protected MonsterMonitor _monsterMonitor { get; private set; }

    public void Initialize(TriggerListener roomZone, MonsterMonitor monsterMonitor)
    {
      RoomZone = roomZone;
      _monsterMonitor = monsterMonitor;
      Initialized?.Invoke();
    }

    public virtual void ReceiveDamage(int damageValue, Vector3 position) => _health.Decrease(damageValue);

    protected void SubscribeEvents() => _health.ValueCameToZero += Die;
    protected void UnSubscribeEvents() => _health.ValueCameToZero -= Die;

    protected virtual void Die()
    {
      IsDead = true;
      Dead?.Invoke(this);
    }
  }
}