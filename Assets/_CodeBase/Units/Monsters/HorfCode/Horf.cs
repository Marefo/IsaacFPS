using System.Collections;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Units.Monsters.HorfCode.Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Units.Monsters.HorfCode
{
  public class Horf : Monster
  {
    [SerializeField] private Transform _model;
    [Space(10)] 
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _deathVfxPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [Space(10)] 
    [SerializeField] private TriggerListener _agroZone;
    [SerializeField] private MonsterShooter _shooter;
    [Space(10)] 
    [SerializeField] private HorfSettings _settings;

    private Hero _hero;
    private bool _isHeroInZone;
    private float _lastAttackTime;

    private void OnEnable()
    {
      SubscribeEvents();
      _agroZone.Entered += OnAgroZoneEnter;
      _agroZone.Canceled += OnAgroZoneCancel;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      _agroZone.Entered -= OnAgroZoneEnter;
      _agroZone.Canceled -= OnAgroZoneCancel;
    }

    private void Start() => StartCoroutine(AttackCoroutine());

    private void Update() => RotateToHero();

    private void OnDrawGizmos()
    {
      if(_isHeroInZone == false) return;
      Gizmos.color = Color.red;
      Vector3 direction = Vector3.Normalize(_hero.ShootTarget.position - _shootPoint.position);
      Gizmos.DrawLine(_shootPoint.position, _shootPoint.position + direction * _settings.BulletSettings.MaxShootDistance);
    }

    private void OnAgroZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _hero = hero;
      _isHeroInZone = true;
    }

    private void OnAgroZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInZone = false;
    }

    private IEnumerator AttackCoroutine()
    {
      PlayShake();
      
      while (true)
      {
        if (_isHeroInZone)
        {
          Vector3 direction = Vector3.Normalize(_hero.ShootTarget.position - _shootPoint.position);
          Physics.Raycast(_shootPoint.position, direction, out RaycastHit hit, _settings.BulletSettings.MaxShootDistance);

          if (Time.time < _lastAttackTime + _settings.AttackCooldown ||
              hit.collider.gameObject.CompareLayers(_settings.ObstaclesLayerMask))
          {
            yield return null;
          }
          else
          {
            Attack();
          }
        }
        else
        {
          yield return null;
        }
      }
    }

    [Button]
    private void PlayShake() => 
      _model.DOShakePosition(_settings.ShakeDuration, _settings.ShakeStrength).SetLoops(-1).SetLink(gameObject);

    [Button]
    private void StopShake() => _model.DOKill();
    
    private void Attack()
    {
      _lastAttackTime = Time.time;
      _shooter.Shoot(_settings.BulletPrefab, _shootPoint, _hero.ShootTarget, _settings.BulletSettings);
      DOVirtual.DelayedCall(1, PlayShake).SetLink(gameObject);
    }

    private void RotateToHero()
    {
      if(_isHeroInZone == false) return;
      Vector3 rotationTargetPosition = _hero.transform.position;
      rotationTargetPosition.y = transform.position.y;
      transform.LookAt(rotationTargetPosition);
    }
    
    protected override void Die()
    {
      Instantiate(_deathVfx, _deathVfxPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
  }
}