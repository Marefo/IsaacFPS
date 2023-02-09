using System;
using System.Collections;
using System.Collections.Generic;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure;
using _CodeBase.Logging;
using _CodeBase.Units.Monsters.FlyCode;
using _CodeBase.Units.Monsters.HiveCode.Data;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _CodeBase.Units.Monsters.HiveCode
{
  public class Hive : Monster
  {
    [SerializeField] private Transform _pooterSpawnPoint;
    [SerializeField] private List<Transform> _flySpawnPoints;
    [Space(10)] 
    [SerializeField] private ParticleSystem _deathVfx;
    [SerializeField] private GameObject _deathTrail;
    [SerializeField] private Transform _deathVfxPoint;
    [Space(10)] 
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [Space(10)]
    [SerializeField] private HiveSettings _settings;

    private Hero _hero;
    private bool _isHeroInRoomZone;
    private Vector3 _escapePosition;
    private Coroutine _spawnFliesCoroutine;
    private Collider _roomZoneCollider;

    private void OnEnable()
    {
      SubscribeEvents();
      Initialized += OnInitialize;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      Initialized -= OnInitialize;
    }

    private void Start() => CheckForHero();

    private void Update()
    {
      if(_isHeroInRoomZone)
        Escape();
      
      UpdateRunAnimationState();
    }

    private void OnDestroy()
    {
      RoomZone.Entered -= OnRoomZoneEnter;
      RoomZone.Canceled -= OnRoomZoneCancel;
    }

    private void OnInitialize()
    {
      _roomZoneCollider = RoomZone.GetComponent<Collider>();
      RoomZone.Entered += OnRoomZoneEnter;
      RoomZone.Canceled += OnRoomZoneCancel;
    }

    private void OnRoomZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      OnHeroEnterZone(obj);
    }

    private void OnRoomZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _hero = null;
      _isHeroInRoomZone = false;

      if (_spawnFliesCoroutine != null)
        StopCoroutine(_spawnFliesCoroutine);
    }

    private void CheckForHero()
    {
      Collider heroCollider = RoomZone.GetHeroFromZone();

      if (heroCollider == null) return;
      OnHeroEnterZone(heroCollider);
    }

    private void OnHeroEnterZone(Collider heroCollider)
    {
      _hero = heroCollider.GetComponent<Hero>();
      _isHeroInRoomZone = true;
      _spawnFliesCoroutine = StartCoroutine(SpawnFliesCoroutine());
    }

    private void Escape()
    {
      Vector3 rotationTargetPosition = _hero.transform.position;
      rotationTargetPosition.y = transform.position.y;
      transform.LookAt(rotationTargetPosition);
      
      if (IsTargetPositionFarEnough() == false) 
        _escapePosition = Helpers.GetRandomPositionInCollider(_roomZoneCollider, transform.position.y).GetNavMeshSampledPosition();

      _agent.SetDestination(_escapePosition);
    }

    private bool IsTargetPositionFarEnough() => 
      Vector3.Distance(_hero.transform.position, _escapePosition) >= _settings.EscapeDistance;

    public IEnumerator SpawnFliesCoroutine()
    {
      while (true)
      {
        yield return new WaitForSeconds(_settings.SpawnDelay);
        _animator.PlayAttack();
        SpawnFly();
      }
    }

    private void SpawnFly(int amount = 1)
    {
      for (int i = 0; i < amount; i++) 
        SpawnMonster(_settings.FlyPrefab, _flySpawnPoints);
    }

    private void SpawnPooter() => 
      SpawnMonster(_settings.PooterPrefab, new List<Transform>() {_pooterSpawnPoint});

    private void SpawnMonster(Monster prefab, List<Transform> spawnPoints)
    {
      Monster monster = Instantiate(prefab, spawnPoints.GetRandomValue().position, Quaternion.identity);
      monster.Initialize(RoomZone, _monsterMonitor);
      _monsterMonitor.AddMonster(monster);
    }

    protected override void Die()
    {
      Instantiate(_deathTrail, _deathVfxPoint.position, Quaternion.identity);
      Instantiate(_deathVfx, _deathVfxPoint.position, Quaternion.identity);
      base.Die();
      SpawnFly(2);
      SpawnPooter();
      Destroy(gameObject);
    }
    
    private void UpdateRunAnimationState() => 
      _animator.ChangeRunState(_agent.velocity.x != 0 || _agent.velocity.z != 0);
  }
}