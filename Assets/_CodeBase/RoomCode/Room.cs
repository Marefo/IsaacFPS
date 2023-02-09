using System;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.Logging;
using _CodeBase.RoomCode.Data;
using _CodeBase.Units.Monsters;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.RoomCode
{
  public class Room : MonoBehaviour
  {
    [field: SerializeField] public TriggerListener Zone;
    [field: Space(10)] 
    [SerializeField] private bool _hasMonsters; 
    [ShowIf("_hasMonsters"), SerializeField] private MonsterMonitor _monsterMonitor;
    [ShowIf("_hasMonsters"), SerializeField] private MonsterSpawner _monsterSpawner;
    [Space(10)] 
    [SerializeField] private GameObject _chandelier;
    [Space(10)] 
    [SerializeField] private List<Room> _linkedRooms;
    [Space(10)] 
    [SerializeField] private List<GameObject> _doors;
    [Space(10)] 
    [SerializeField] private RoomSettings _settings;

    private bool _cleaned;
    
    private void OnEnable()
    {
      Zone.Entered += OnZoneEnter;
      Zone.Canceled += OnZoneCancel;
      
      if(_hasMonsters)
        _monsterMonitor.AllMonstersDied += OnAllMonstersDie;
    }

    private void OnDisable()
    {
      Zone.Entered -= OnZoneEnter;
      Zone.Canceled -= OnZoneCancel;
      
      if(_hasMonsters)
        _monsterMonitor.AllMonstersDied -= OnAllMonstersDie;
    }

    private void OnZoneEnter(Collider obj)
    {
      if (obj.TryGetComponent(out Hero hero) == false) return;
      _chandelier.SetActive(true);
      if(_hasMonsters == false || _cleaned) return;
      ChangeDoorsState(true);
      ChangeLinkedRoomsDoorsState(true);
      DOVirtual.DelayedCall(_settings.SpawnDelay, () => _monsterSpawner.SpawnMonsters(_settings.SpawnAfterSmokeDelay));
    }

    private void OnZoneCancel(Collider obj)
    {
      if (obj.TryGetComponent(out Hero hero) == false) return;
      _chandelier.SetActive(false);
    }

    private void OnAllMonstersDie()
    {
      _cleaned = true;
      ChangeDoorsState(false);
      ChangeLinkedRoomsDoorsState(false);
    }

    public void ChangeLinkedRoomsDoorsState(bool enable) => _linkedRooms.ForEach(room => room.ChangeDoorsState(enable));
    public void ChangeDoorsState(bool enable) => _doors.ForEach(door => door.SetActive(enable));
  }
}