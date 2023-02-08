using System;
using _CodeBase.Infrastructure;
using _CodeBase.Interfaces;
using _CodeBase.Logging;
using _CodeBase.ShooterCode.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.ShooterCode
{
  public class Grenade : Projectile
  {
    [SerializeField] private bool _disableDamage;
    [SerializeField] private bool _disableDestroy;
    [Space(10)]
    [SerializeField] private GameObject _model;
    [SerializeField] private ParticleSystem _destroyVfx;
    [SerializeField] private Rigidbody _rigidbody;
    
    private Vector3 _startPosition;
    private Coroutine _moveCoroutine;
    private int _ricochetTimes;
    private float? _maxDistance;
    private float? _gravity;
    private int _damage;
    private bool _destroyed;

    public void OnShoot(Vector3 initialVelocity, float maxDistance, float gravity, int damage)
    {
      _startPosition = transform.position;
      _rigidbody.velocity = initialVelocity;
      _maxDistance = maxDistance;
      _gravity = gravity;
      _damage = damage;
    }

    public void EnableDamage() => _disableDamage = false;
    public void EnableDestroy() => _disableDestroy = false;

    private void Update()
    {
      if(_maxDistance != null && Vector3.Distance(_startPosition, transform.position) >= _maxDistance.Value)
        Destroy();
    }

    private void FixedUpdate()
    {
      if(_gravity == null) return;
      ApplyGravity();
    }

    private void ApplyGravity() => _rigidbody.AddForce(Vector3.down * _gravity.Value);

    protected override void OnDamageableZoneEnter(IDamageable damageable)
    {
      if(_disableDamage) return;
      StopMovement();
      damageable.ReceiveDamage(_damage, transform.position);
      Destroy();
    }

    protected override void OnDestroyerZoneEnter()
    {
      if(_disableDestroy) return;
      base.OnDestroyerZoneEnter();
    }

    protected override void Destroy()
    {
      if(_destroyed) return;
      _destroyed = true;
      _model.SetActive(false);
      Instantiate(_destroyVfx, transform.position + Vector3.one * 0.15f, _destroyVfx.transform.rotation);
      _zone.transform.localScale *= 5;
      StopMovement();
      Destroy(gameObject, 1);
    }

    private void StopMovement() => _rigidbody.velocity = Vector3.zero;
  }
}