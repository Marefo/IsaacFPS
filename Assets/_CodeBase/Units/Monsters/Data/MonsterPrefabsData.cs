using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _CodeBase.Units.Monsters.Data
{
  [CreateAssetMenu(fileName = "MonsterPrefabsData", menuName = "StaticData/MonsterPrefabs")]
  public class MonsterPrefabsData : ScriptableObject
  {
    public List<MonsterPrefab> Data;

    public GameObject GetPrefab(MonsterType type) => 
      Data.First(other => other.Type == type).Prefab;
  }
  
  [Serializable]
  public class MonsterPrefab
  {
    public MonsterType Type;
    public GameObject Prefab;
  }
}