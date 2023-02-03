using _CodeBase.Units.Monsters.Data;
using UnityEngine;

namespace _CodeBase.Points
{
  public class MonsterSpawnPoint : Point
  {
    [field: SerializeField] public MonsterType Type { get; private set; }
  }
}