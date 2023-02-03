using _CodeBase.Points;
using _CodeBase.RoomCode;
using _CodeBase.Units.Monsters.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class MonsterSpawner : MonoBehaviour
  {
    [SerializeField] private Room _room;
    [SerializeField] private MonsterMonitor _monitor;
    [SerializeField] private MonstersSpawnPointsStorage _spawnPointsStorage;
    [Space(10)] 
    [SerializeField] private MonsterPrefabsData _prefabsData;

    private int _spawnedChasers;
    
    private void Start() => SpawnMonsters();

    private void SpawnMonsters() => _spawnPointsStorage.Points.ForEach(SpawnMonster);

    private void SpawnMonster(MonsterSpawnPoint spawnPoint)
    {
      spawnPoint.Take();
      GameObject prefab = _prefabsData.GetPrefab(spawnPoint.Type);
      Monster monster = Instantiate(prefab, spawnPoint.transform).GetComponent<Monster>();
      monster.transform.localPosition = Vector3.zero;

      if (spawnPoint.HasTargetRotation)
        monster.transform.localRotation = Quaternion.Euler(spawnPoint.TargetRotation);
          
      monster.Initialize(_room.Zone);
      _monitor.AddMonster(monster);
    }
  }
}