using System.Collections;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.ShooterCode;
using _CodeBase.Units.Monsters.MonstroCode.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.MonstroCode
{
  public class Monstro : Monster
  {
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private GameObject _deathTrail;
    [Space(10)] 
    [SerializeField] private Transform _jumpImpactPoint;
    [SerializeField] private TriggerListener _jumpImpactZone;
    [SerializeField] private MonsterStateMachine _monsterStateMachine;
    [SerializeField] private MonstroAnimator _monstroAnimator;
    [SerializeField] private UnitAnimator _animator;
    [Space(10)] 
    [SerializeField] private List<Transform> _attackPoints;
    [Space(10)] 
    [SerializeField] private MonstroSettings _settings;

    private bool _isHeroInJumpImpactZone;
    private bool _jumpImpact;
    private float _lastJumpImpactTime;
    
    private void OnEnable()
    {
      SubscribeEvents();
      _monstroAnimator.AttackFramePlayed += OnShootFrame;
      _monstroAnimator.JumpImpactStarted += OnJumpImpactStart;
      _monstroAnimator.JumpImpactFinished += OnJumpImpactFinish;
      _jumpImpactZone.Entered += OnJumpImpactZoneEnter;
      _jumpImpactZone.Canceled += OnJumpImpactZoneCancel;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      _monstroAnimator.AttackFramePlayed -= OnShootFrame;
      _monstroAnimator.JumpImpactStarted -= OnJumpImpactStart;
      _monstroAnimator.JumpImpactFinished -= OnJumpImpactFinish;
      _jumpImpactZone.Entered -= OnJumpImpactZoneEnter;
      _jumpImpactZone.Canceled -= OnJumpImpactZoneCancel;
    }

    private void Update()
    {
      if(_monsterStateMachine.Hero == null) return;
      RotateToHero();
      TryApplyJumpImpact();
    }

    public void Attack() => _animator.PlayAttack();

    private void OnJumpImpactZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInJumpImpactZone = true;
    }

    private void OnJumpImpactZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInJumpImpactZone = false;
    }

    private void OnJumpImpactStart()
    {
      Instantiate(_settings.JumpImpactVfxPrefab, _jumpImpactPoint.position, Quaternion.identity);
      _jumpImpact = true;
    }

    private void OnJumpImpactFinish() => _jumpImpact = false;


    private void TryApplyJumpImpact()
    {
      if (Time.time >= _lastJumpImpactTime + 0.25f && _jumpImpact && _isHeroInJumpImpactZone)
      {
        _monsterStateMachine.Hero.ApplyContactDamage(transform.position);
        _lastJumpImpactTime = Time.time;
      }
    }

    private void OnShootFrame()
    {
      foreach (Transform point in _attackPoints)
      {
        float distance = Vector3.Distance(_monsterStateMachine.Hero.transform.position, point.position) + _settings.ShootOffsetZ.GetRandomValue();
        Vector3 targetPosition = point.position + point.forward * distance;
        targetPosition.y = _monsterStateMachine.Hero.transform.position.y;
        Grenade projectile = Instantiate(_settings.Grenade, point.position, Quaternion.identity);
        LaunchData launchData = CalculateGrenadeLaunchData(point, targetPosition, _settings.ProjectilePickHeight, 
          _settings.ProjectileGravity);
        projectile.OnShoot(launchData.InitialVelocity, distance, _settings.ProjectileGravity, 1);
      }
    }

    private void RotateToHero()
    {
      Vector3 rotationTargetPosition = _monsterStateMachine.Hero.transform.position;
      rotationTargetPosition.y = transform.position.y;
      transform.LookAt(rotationTargetPosition);
    }

    private LaunchData CalculateGrenadeLaunchData(Transform startPoint, Vector3 targetPosition, float height, float gravity)
    {
      float _gravity = -gravity;
      float displacementY = targetPosition.y - startPoint.position.y;
      Vector3 displacementXZ = new Vector3 (targetPosition.x - startPoint.position.x, 0, 
        targetPosition.z - startPoint.position.z);
      float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);
      Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * _gravity * height);
      Vector3 velocityXZ = displacementXZ / time;

      return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity));
    }

    private struct LaunchData
    {
      public readonly Vector3 InitialVelocity;

      public LaunchData(Vector3 initialVelocity)
      {
        InitialVelocity = initialVelocity;
      }
    }
    
    protected override void Die()
    {
      Instantiate(_deathTrail, _deathVfxSpawnPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
  }
}