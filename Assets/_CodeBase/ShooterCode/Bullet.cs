using System.Collections;
using _CodeBase.HeroCode.Data;
using _CodeBase.Interfaces;
using _CodeBase.Logging;
using _CodeBase.ShooterCode.Data;
using UnityEngine;

namespace _CodeBase.ShooterCode
{
  public class Bullet : Projectile
  {
    [SerializeField] private float _destroyVfxScale;
    [SerializeField] private ParticleSystem _destroyVfx;
    
    private Vector3 _startPosition;
    private Vector3 _direction;
    private BulletSettings _settings;
    private Coroutine _moveCoroutine;
    private int _ricochetTimes;

    public void OnShoot(Vector3 direction, BulletSettings settings)
    {
      _startPosition = transform.position;
      _direction = direction;
      _settings = settings;
      StartMovement();
    }

    protected override void OnDamageableZoneEnter(IDamageable damageable)
    {
      StopMovement();
      damageable.ReceiveDamage(_settings.Damage, transform.position);
      Destroy();
    }

    private void StartMovement() => 
      _moveCoroutine = StartCoroutine(MoveCoroutine());

    private IEnumerator MoveCoroutine()
    {
      while (true)
      {
        transform.position += _direction * _settings.ProjectileSpeed * Time.deltaTime;
        float reachedDistance = Vector3.Distance(_startPosition, transform.position);

        if (reachedDistance >= _settings.MaxShootDistance)
        {
          Destroy();
          yield break;
        }
        else
          yield return null;
      } 
    }

    private void StopMovement()
    {
      if(_moveCoroutine == null) return;
      StopCoroutine(_moveCoroutine);
    }

    protected override void Destroy()
    {
      ParticleSystem vfx = Instantiate(_destroyVfx, transform.position, Quaternion.identity);
      vfx.transform.localScale = Vector3.one * _destroyVfxScale;
      StopMovement();
      Destroy(gameObject);
    }
  }
}