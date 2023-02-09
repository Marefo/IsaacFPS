using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.ItemsDrop.Data
{
  [CreateAssetMenu(fileName = "DropsData", menuName = "StaticData/Drops")]
  public class DropsData : ScriptableObject
  {
    public List<Drop> Drops;

    public DropItem GetRandomDrop()
    {
      Drop result = null;
      float randomValue = Random.value;
      Sort();

      foreach (Drop drop in Drops)
      {
        if (randomValue > drop.Chance / 100) continue;
        result = drop;
        break;
      }

      return result?.Prefab;
    }

    [Button]
    public void Sort() => Drops.Sort((x,y) => x.Chance.CompareTo(y.Chance));
  }
}