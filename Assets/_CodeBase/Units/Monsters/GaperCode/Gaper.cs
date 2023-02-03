using System;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using _CodeBase.Units.Monsters.GaperCode.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.GaperCode
{
  public class Gaper : Monster
  {
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [Space(10)]
    [SerializeField] private Health _health;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private GaperAnimator _gaperAnimator;
    [SerializeField] private TriggerListener _attackZone;
    [Space(10)] 
    [SerializeField] private GaperSettings _settings;

    private bool _isAttacking;
    private bool _attacked;
    
    private void OnEnable()
    {
      _health.ValueCameToZero += Die;
      _attackZone.Entered += OnAttackZoneEnter;
      _gaperAnimator.AttackFramePlayed += OnAttackFrame;
    }

    private void OnDisable()
    {
      _health.ValueCameToZero -= Die;
      _attackZone.Entered -= OnAttackZoneEnter;
      _gaperAnimator.AttackFramePlayed -= OnAttackFrame;
    }

    private void Start() => SetUpSpeed();

    private void Update() => UpdateRunAnimationState();

    public override void ReceiveDamage(int damageValue, Vector3 position) => _health.Decrease(damageValue);

    public void Attack()
    {
      _isAttacking = true;
      DOVirtual.DelayedCall(_settings.AttackDuration, ResetAttack);
      PlayAttack();
    }

    private void OnAttackZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false || _isAttacking == false || _attacked) return;
      PlayAttack();
    }

    private void OnAttackFrame()
    {
      Collider heroCollider = _attackZone.CollidersInZone.FirstOrDefault(other => 
        other != null && other.TryGetComponent(out Hero hero));
      
      if (heroCollider != null)
        ApplyDamageToHero(heroCollider.GetComponent<Hero>());
    }

    private void SetUpSpeed()
    {
      _agent.acceleration = _settings.Acceleration.GetRandomValue();
      _agent.speed = _settings.MoveSpeed.GetRandomValue();
    }
    
    private void PlayAttack() => _animator.PlayAttack();

    private void ResetAttack()
    {
      _isAttacking = false;
      _attacked = false;
    }

    private void ApplyDamageToHero(Hero hero)
    {
      hero.ApplyContactDamage(transform.position);
      _attacked = true;
    }
    
    protected override void Die()
    {
      Instantiate(_deathVfx, _deathVfxSpawnPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
    
    private void UpdateRunAnimationState() => 
      _animator.ChangeRunState(_agent.velocity.x != 0 || _agent.velocity.z != 0);
  }
}