using _CodeBase.ShooterCode;
using _CodeBase.ShooterCode.Data;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.Interfaces;
using _CodeBase.Units.Monsters.PooterCode.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Units.Monsters.PooterCode.States
{
  public class AttackState : State
  {
    private readonly PooterStateMachine _stateMachine;
    private readonly UnitAnimator _animator;
    private readonly ITargetProvider _targetProvider;
    private readonly Transform _shootPoint;
    private readonly BulletSettings _bulletSettings;
    private readonly PooterSettings _settings;

    public AttackState(PooterStateMachine stateMachine, UnitAnimator animator, ITargetProvider targetProvider, 
      Transform shootPoint, BulletSettings bulletSettings, PooterSettings settings)
    {
      _stateMachine = stateMachine;
      _animator = animator;
      _targetProvider = targetProvider;
      _shootPoint = shootPoint;
      _bulletSettings = bulletSettings;
      _settings = settings;
    }

    public override void Enter()
    {
      base.Enter();
      _stateMachine.ChangeAttackPossibilityState(false);
      _stateMachine.ChangeAttackState(false);
      Attack();
    }

    private void Attack()
    {
      _animator.PlayAttack();
      
      Bullet projectile = Object.Instantiate(_settings.ProjectilePrefab, _shootPoint.position, Quaternion.identity);
      Vector3 targetPosition = _targetProvider.GetTarget().transform.position;
      Vector3 direction = Vector3.Normalize(targetPosition - _stateMachine.transform.position);
      projectile.OnShoot(direction, _bulletSettings);
      
      _stateMachine.ChangeAttackState(true);
    }
  }
}