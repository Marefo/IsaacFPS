using System.Collections;
using _CodeBase.Points;
using _CodeBase.RoomCode;
using _CodeBase.Units.Monsters.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class MonsterSpawner : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _spawnVfx;
    [Space(10)] 
    [SerializeField] private Room _room;
    [SerializeField] private MonsterMonitor _monitor;
    [SerializeField] private MonstersSpawnPointsStorage _spawnPointsStorage;
    [Space(10)] 
    [SerializeField] private MonsterPrefabsData _prefabsData;

    private int _spawnedChasers;

    public void SpawnMonsters(float spawnAfterSmokeDelay) => 
      _spawnPointsStorage.Points.ForEach(point => StartCoroutine(SpawnMonster(point, spawnAfterSmokeDelay)));

    private IEnumerator SpawnMonster(MonsterSpawnPoint spawnPoint, float spawnAfterSmokeDelay)
    {
      spawnPoint.Take();
      GameObject prefab = _prefabsData.GetPrefab(spawnPoint.Type);
      Monster prefabMonster = prefab.GetComponent<Monster>();
      Instantiate(_spawnVfx, spawnPoint.transform);

      yield return new WaitForSeconds(spawnAfterSmokeDelay);
      
      Monster monster = Instantiate(prefab, spawnPoint.transform).GetComponent<Monster>();
      monster.transform.localPosition = Vector3.up * prefabMonster.SpawnHeight;

      if (spawnPoint.HasTargetRotation)
        monster.transform.localRotation = Quaternion.Euler(spawnPoint.TargetRotation);
          
      monster.Initialize(_room.Zone, _monitor);
      _monitor.AddMonster(monster);
    }
  }
}