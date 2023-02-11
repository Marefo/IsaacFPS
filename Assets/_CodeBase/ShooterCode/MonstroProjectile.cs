using _CodeBase.Interfaces;
using UnityEngine;

namespace _CodeBase.ShooterCode
{
  public class MonstroProjectile : Projectile
  {
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

    private void Update()
    {
      if(_maxDistance != null && Vector3.Distance(_startPosition, transform.position) >= _maxDistance.Value * 2)
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
      StopMovement();
      damageable.ReceiveDamage(_damage, transform.position);
      Destroy();
    }

    protected override void Destroy()
    {
      if(_destroyed) return;
      _destroyed = true;
      Instantiate(_destroyVfx, transform.position, Quaternion.identity);
      StopMovement();
      Destroy(gameObject);
    }

    private void StopMovement() => _rigidbody.velocity = Vector3.zero;
  }
}