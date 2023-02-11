using System;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logging;
using _CodeBase.RoomCode.Data;
using _CodeBase.UI;
using _CodeBase.Units.Monsters;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _CodeBase.RoomCode
{
  public class Room : MonoBehaviour
  {
    [field: SerializeField] public TriggerListener Zone;
    [field: Space(10)] 
    [SerializeField] private bool _hasMonsters; 
    [SerializeField] private bool _isBossRoom; 
    [ShowIf("_isBossRoom"), SerializeField] private BossScreen _bossScreen; 
    [ShowIf("_hasMonsters"), SerializeField] private MonsterMonitor _monsterMonitor;
    [ShowIf("_hasMonsters"), SerializeField] private MonsterSpawner _monsterSpawner;
    [ShowIf("_hasMonsters"), SerializeField] private Transform _chestSpawnPoint;
    [Space(10)] 
    [SerializeField] private GameObject _chandelier;
    [Space(10)] 
    [SerializeField] private List<Room> _linkedRooms;
    [Space(10)] 
    [SerializeField] private List<GameObject> _doors;
    [Space(10)] 
    [SerializeField] private RoomSettings _settings;

    private NavMeshService _navMeshService;
    private LoadingCurtain _loadingCurtain;
    private InputService _inputService;
    private WinnerLetter _winnerLetter;
    private bool _cleaned;
    private bool _isHeroInRoomZone;

    [Inject]
    public void Construct(NavMeshService navMeshService, LoadingCurtain loadingCurtain, InputService inputService,
      WinnerLetter winnerLetter)
    {
      _navMeshService = navMeshService;
      _loadingCurtain = loadingCurtain;
      _inputService = inputService;
      _winnerLetter = winnerLetter;
    }
    
    private void OnEnable()
    {
      Zone.Entered += OnZoneEnter;
      Zone.Canceled += OnZoneCancel;

      if(_hasMonsters)
        _monsterMonitor.AllMonstersDied += OnAllMonstersDie;

      if (_isBossRoom)
        _bossScreen.Showed += OnBossScreenShow;
    }

    private void OnDisable()
    {
      Zone.Entered -= OnZoneEnter;
      Zone.Canceled -= OnZoneCancel;
      
      if(_hasMonsters)
        _monsterMonitor.AllMonstersDied -= OnAllMonstersDie;
      
      if (_isBossRoom)
        _bossScreen.Showed -= OnBossScreenShow;
    }

    private void OnZoneEnter(Collider obj)
    {
      if (_isHeroInRoomZone || obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInRoomZone = true;
      _chandelier.SetActive(true);
      if(_hasMonsters == false || _cleaned) return;
      ChangeDoorsState(true);
      ChangeLinkedRoomsDoorsState(true);
      _navMeshService.ReBake();

      if (_isBossRoom)
      {
        _inputService.Disable();
        _loadingCurtain.FadeInAndOut(0.5f, _bossScreen.Show);
      }
      else
        SpawnMonsters();
    }

    private void OnZoneCancel(Collider obj)
    {
      if (_isHeroInRoomZone == false || obj.TryGetComponent(out Hero hero) == false) return;
      _chandelier.SetActive(false);
      _isHeroInRoomZone = false;
    }

    private void OnBossScreenShow() => _loadingCurtain.FadeInAndOut(0.5f, OnBossFightStart);

    private void OnBossFightStart()
    {
      _bossScreen.Hide();
      _inputService.Enable();
      SpawnMonsters();
    }

    private void SpawnMonsters()
    {
      DOVirtual.DelayedCall(_settings.SpawnDelay,
        () => _monsterSpawner.SpawnMonsters(_settings.SpawnAfterSmokeDelay));
    }

    private void OnAllMonstersDie()
    {
      _cleaned = true;
      ChangeDoorsState(false);
      ChangeLinkedRoomsDoorsState(false);
      Chest chest = Instantiate(_settings.ChestPrefab, _chestSpawnPoint.position, _settings.ChestPrefab.transform.rotation);
      chest.Initialize(_winnerLetter);
    }

    public void ChangeLinkedRoomsDoorsState(bool enable) => _linkedRooms.ForEach(room => room.ChangeDoorsState(enable));
    public void ChangeDoorsState(bool enable) => _doors.ForEach(door => door.SetActive(enable));
  }
}