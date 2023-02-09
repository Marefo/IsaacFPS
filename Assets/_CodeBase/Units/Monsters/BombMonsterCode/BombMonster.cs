using System;
using System.Collections;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using _CodeBase.Units.Monsters.BombMonsterCode.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.BombMonsterCode
{
  public class BombMonster : MonoBehaviour, IDamageable
  {
    [SerializeField] private Transform _destroyVfxPoint;
    [SerializeField] private TriggerListener _chaseZone;
    [SerializeField] private TriggerListener _explodeZone;
    [SerializeField] private TriggerListener _attackZone;
    [Space(10)] 
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Health _health;
    [Space(10)] 
    [SerializeField] private BombMonsterSettings _settings;

    private bool _exploded;
    private Hero _hero;
    private bool _canChase;

    private void OnEnable()
    {
      _health.ValueCameToZero += Explode;
      _attackZone.Entered += OnAttackZoneEnter;
      _chaseZone.Entered += OnChaseZoneEnter;
      _chaseZone.Canceled += OnChaseZoneCancel;

      CheckForHero();
    }

    private void OnDisable()
    {
      _health.ValueCameToZero -= Explode;
      _attackZone.Entered -= OnAttackZoneEnter;
      _chaseZone.Entered -= OnChaseZoneEnter;
      _chaseZone.Canceled -= OnChaseZoneCancel;
    }

    private void Update()
    {
      if (_canChase)
        RotateToHero();
    }

    private void FixedUpdate()
    {
      if (_canChase) 
        Chase();
    }

    public virtual void ReceiveDamage(int damageValue, Vector3 position) => _health.Decrease(damageValue);

    private void OnChaseZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _hero = hero;
      _canChase = true;
    }

    private void OnChaseZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _canChase = false;
      _hero = null;
    }

    private void OnAttackZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      Explode();
    }

    private void CheckForHero()
    {
      Collider heroInAttackZone = _attackZone.GetHeroFromZone();

      if (heroInAttackZone != null) 
        Explode();
      else
      {
        Collider heroInChaseZone = _chaseZone.GetHeroFromZone();
        _hero = heroInChaseZone.GetComponent<Hero>();
        _canChase = true;
      }
    }

    private void Chase() => _rigidbody.AddForce(transform.forward * _settings.MoveSpeed, ForceMode.Force);

    private void RotateToHero()
    {
      Vector3 rotationTargetPosition = _hero.transform.position;
      rotationTargetPosition.y = transform.position.y;
      transform.LookAt(rotationTargetPosition);
    }

    private void Explode()
    {
      if(_exploded) return; 
      _exploded = true;

      Instantiate(_settings.DestroyVfx, _destroyVfxPoint.position, _settings.DestroyVfx.transform.rotation);
      
      foreach (Collider obj in _explodeZone.CollidersInZone)
      {
        if(obj == null) continue;
        
        if(obj.TryGetComponent(out IExplosive explosive))
          explosive.Explode();
        else if(obj.TryGetComponent(out IDamageable damageable))
          damageable.ReceiveDamage(1, transform.position);
      }
      
      Destroy(gameObject);
    }
  }
}