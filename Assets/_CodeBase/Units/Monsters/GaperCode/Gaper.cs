using System.Collections;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Units.Monsters.GaperCode.Data;
using _CodeBase.Units.Monsters.PacerCode;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.GaperCode
{
  public class Gaper : Monster
  {
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [SerializeField] private GameObject _deathTrail;
    [Space(10)]
    [SerializeField] private Pacer _pacerPrefab;
    [Space(10)]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private GaperAnimator _gaperAnimator;
    [SerializeField] private TriggerListener _attackZone;
    [Space(10)] 
    [SerializeField] private GaperSettings _settings;

    private bool _isAttacking;
    private bool _attacked;
    private bool _isFirstSound = true;
    
    private void OnEnable()
    {
      SubscribeEvents();
      _attackZone.Entered += OnAttackZoneEnter;
      _gaperAnimator.AttackFramePlayed += OnAttackFrame;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      _attackZone.Entered -= OnAttackZoneEnter;
      _gaperAnimator.AttackFramePlayed -= OnAttackFrame;
    }

    private void Start()
    {
      SetUpSpeed();
      StartCoroutine(SfxCoroutine());
    }

    private void Update() => UpdateRunAnimationState();

    private IEnumerator SfxCoroutine()
    {
      while (true)
      {
        if (_isFirstSound || Health.IsValueZero == false)
        {
          _isFirstSound = false;
          _audioService.PlaySfx(_audioService.SfxData.GaperSounds.GetRandomValue(), true, 0.6f);
        }

        yield return new WaitForSeconds(_settings.SfxDelay.GetRandomValue());
      }
    }
    
    public void Attack()
    {
      _isAttacking = true;
      DOVirtual.DelayedCall(_settings.AttackDuration, ResetAttack).SetLink(gameObject);
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
      _audioService.PlaySfx(_audioService.SfxData.GaperDeath.GetRandomValue(), true);
      Instantiate(_deathTrail, _deathVfxSpawnPoint.position, Quaternion.identity);
      Instantiate(_deathVfx, _deathVfxSpawnPoint.position, Quaternion.identity);
      Vector3 spawnPosition = transform.position;
      spawnPosition.y += _pacerPrefab.SpawnOffsetY;
      Pacer pacer = Instantiate(_pacerPrefab, spawnPosition, Quaternion.identity);
      pacer.Initialize(RoomZone, _monsterMonitor, _audioService);
      _monsterMonitor.AddMonster(pacer);
      base.Die();
      Destroy(gameObject);
    }
    
    private void UpdateRunAnimationState() => 
      _animator.ChangeRunState(_agent.velocity.x != 0 || _agent.velocity.z != 0);
  }
}