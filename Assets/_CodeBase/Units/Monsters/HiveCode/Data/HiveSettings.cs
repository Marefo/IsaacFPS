using _CodeBase.Data;
using _CodeBase.Units.Monsters.FlyCode;
using _CodeBase.Units.Monsters.PooterCode;
using UnityEngine;

namespace _CodeBase.Units.Monsters.HiveCode.Data
{
  [CreateAssetMenu(fileName = "HiveSettings", menuName = "Settings/Monsters/Hive", order = 0)]
  public class HiveSettings : ScriptableObject
  {
    public Range SoundsDelay;
    [Space(10)]
    public float SpawnDelay;
    public float EscapeDistance;
    [Space(10)]
    public Pooter PooterPrefab;
    public Fly FlyPrefab;
  }
}