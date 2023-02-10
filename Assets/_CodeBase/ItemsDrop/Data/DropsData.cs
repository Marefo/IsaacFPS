using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.ItemsDrop.Data
{
  [CreateAssetMenu(fileName = "DropsData", menuName = "StaticData/Drops")]
  public class DropsData : ScriptableObject
  {
    public bool CanBeEmpty;
    [Space(10)]
    public List<Drop> Drops;

    public DropItem GetRandomDrop()
    {
      Drop result = null;
      Sort();

      if (CanBeEmpty)
      {
        float randomValue = Random.value;
        result = GetDropFromRandomValue(randomValue, result);
      }
      else
      {
        while (result == null)
        {
          float randomValue = Random.value;
          result = GetDropFromRandomValue(randomValue, result);
        }
      }
      
      return result?.Prefab;
    }

    private Drop GetDropFromRandomValue(float randomValue, Drop result)
    {
      foreach (Drop drop in Drops)
      {
        if (randomValue > drop.Chance / 100) continue;
        result = drop;
        break;
      }

      return result;
    }

    [Button]
    public void Sort() => Drops.Sort((x,y) => x.Chance.CompareTo(y.Chance));
  }
}