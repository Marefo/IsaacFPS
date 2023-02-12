using System.Collections;
using _CodeBase.Extensions;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logging;
using _CodeBase.Points;
using _CodeBase.RoomCode;
using _CodeBase.UI;
using _CodeBase.Units.Monsters.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace _CodeBase.Units.Monsters
{
  public class MonsterSpawner : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _spawnVfx;
    [FormerlySerializedAs("_isBoss")]
    [Space(10)]
    [SerializeField] private bool _hasBoss;
    [ShowIf("_hasBoss"), SerializeField] private BossHealthVisualizer _bossHealthVisualizer;  
    [SerializeField] private Room _room;
    [SerializeField] private MonsterMonitor _monitor;
    [SerializeField] private MonstersSpawnPointsStorage _spawnPointsStorage;
    [Space(10)] 
    [SerializeField] private MonsterPrefabsData _prefabsData;

    private NavMeshService _navMeshService;
    private AudioService _audioService;
    private int _spawnedChasers;
    private float _spawnAfterSmokeDelay;

    [Inject]
    public void Construct(NavMeshService navMeshService, AudioService audioService)
    {
      _navMeshService = navMeshService;
      _audioService = audioService;
    }
    
    public void SpawnMonsters(float spawnAfterSmokeDelay)
    {
      if (_navMeshService.IsInitialized)
        _spawnPointsStorage.Points.ForEach(point => StartCoroutine(SpawnMonster(point, spawnAfterSmokeDelay)));
      else
      {
        _spawnAfterSmokeDelay = spawnAfterSmokeDelay;
        _navMeshService.Initialized += OnInitialize;
      }
    }

    private void OnInitialize()
    {
      _navMeshService.Initialized -= OnInitialize;
      SpawnMonsters(_spawnAfterSmokeDelay);
    }

    private IEnumerator SpawnMonster(MonsterSpawnPoint spawnPoint, float spawnAfterSmokeDelay)
    {
      spawnPoint.Take();
      GameObject prefab = _prefabsData.GetPrefab(spawnPoint.Type);
      Monster monsterPrefab = prefab.GetComponent<Monster>();

      Transform vfx = Instantiate(_spawnVfx, spawnPoint.transform).transform;
      vfx.localPosition = new Vector3(0, vfx.localPosition.y, 0);

      yield return new WaitForSeconds(spawnAfterSmokeDelay);
      
      Monster monster = Instantiate(prefab, spawnPoint.transform).GetComponent<Monster>();
      Vector3 spawnPosition = Vector3.zero;

      if (monsterPrefab.HasSpawnOffsetY)
        spawnPosition.y = monsterPrefab.SpawnOffsetY;
      
      monster.transform.localPosition = spawnPosition;

      if (monster.IsBoss)
      {
        _bossHealthVisualizer.RegisterBoss(monster.Health);
        monster.Dead += OnMonsterDeath;
      }
      
      if(monster.TryGetComponent(out NavMeshAgent agent))
        agent.Warp(spawnPoint.Position.GetNavMeshSampledPosition());

      if (spawnPoint.HasTargetRotation)
        monster.transform.localRotation = Quaternion.Euler(spawnPoint.TargetRotation);
          
      monster.Initialize(_room.Zone, _monitor, _audioService);
      _monitor.AddMonster(monster);
    }

    private void OnMonsterDeath(Monster monster)
    {
      monster.Dead -= OnMonsterDeath;
      _bossHealthVisualizer.UnRegisterBoss(monster.Health);
    }
  }
}