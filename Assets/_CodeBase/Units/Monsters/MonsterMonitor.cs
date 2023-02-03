using System;
using System.Collections.Generic;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class MonsterMonitor : MonoBehaviour
  {
    public event Action MonsterDead;
    
    private readonly List<Monster> _monsters = new List<Monster>();

    private void OnDestroy()
    {
      foreach (Monster monster in _monsters) 
        monster.Dead -= RemoveMonster;
    }

    public void AddMonster(Monster monster)
    {
      _monsters.Add(monster);
      monster.Dead += RemoveMonster;
    }

    private void RemoveMonster(Monster monster)
    {
      _monsters.Remove(monster);
      MonsterDead?.Invoke();
    }
  }
}