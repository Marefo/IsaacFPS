using System;
using _CodeBase.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.PooterCode
{
  public class Pooter : Monster
  {
    [SerializeField] private Transform _model;
    [SerializeField] private PooterStateMachine _stateMachine;
    [SerializeField] private Transform _deathVfxPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [SerializeField] private GameObject _deathTrail;
    
    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnSubscribeEvents();

    private void Update()
    {
      if(_stateMachine.IsHeroInRoomZone == false) return;
      Vector3 rotationTargetPosition = _stateMachine.Hero.transform.position;
      rotationTargetPosition.y = transform.position.y;
      _model.LookAt(rotationTargetPosition);
    }

    protected override void Die()
    {
      Instantiate(_deathTrail, _deathVfxPoint.position, Quaternion.identity);
      Instantiate(_deathVfx, _deathVfxPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
  }
}