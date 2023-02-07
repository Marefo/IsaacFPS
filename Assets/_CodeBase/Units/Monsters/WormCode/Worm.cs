using System;
using System.Collections;
using System.Linq;
using _CodeBase.HeroCode;
using _CodeBase.Logging;
using _CodeBase.Units.Monsters.WormCode.Data;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _CodeBase.Units.Monsters.WormCode
{
  public class Worm : Monster
  {
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _deathVfxPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [Space(10)] 
    [SerializeField] private MonsterShooter _monsterShooter;
    [SerializeField] private WormGround _wormGround;
    [SerializeField] private WormAnimator _animator;
    [SerializeField] private Collider _collider;
    [Space(10)] 
    [SerializeField] private WormSettings _settings;

    private Hero _hero;
    private bool _isHeroInRoomZone;
    private float _defaultPositionY;

    private void OnEnable()
    {
      SubscribeEvents();
      Initialized += OnInitialize;
      _animator.AttackFramePlayed += OnAttackFrame;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      Initialized -= OnInitialize;
      _animator.AttackFramePlayed -= OnAttackFrame;
    }

    private void Start()
    {
      _defaultPositionY = transform.position.y;
      CheckForHero();
      StartCoroutine(FightCoroutine());
    }

    private void Update() => RotateToHero();

    private void OnDestroy()
    {
      RoomZone.Entered -= OnRoomZoneEnter;
      RoomZone.Entered -= OnRoomZoneCancel; 
    }

    private void OnInitialize()
    {
      RoomZone.Entered += OnRoomZoneEnter;
      RoomZone.Entered += OnRoomZoneCancel;
    }

    private void OnRoomZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _hero = hero;
      _isHeroInRoomZone = true;
    }

    private void OnRoomZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _hero = null;
      _isHeroInRoomZone = false;
    }

    private void OnAttackFrame() => 
      _monsterShooter.Shoot(_settings.BulletPrefab, _shootPoint, _hero.ShootTarget, _settings.BulletSettings);

    private IEnumerator FightCoroutine()
    {
      while (true)
      {
        yield return new WaitForSeconds(_settings.DisappearDelay);
        Disappear();

        yield return new WaitForSeconds(_settings.AppearDelay / 2);
        MoveToRandomPosition();
        
        yield return new WaitForSeconds(_settings.AppearDelay / 2);
        Appear();
        
        if (_isHeroInRoomZone)
        {
          yield return new WaitForSeconds(_settings.AttackDelay);
          Attack();
        }
      }
    }

    private void MoveToRandomPosition()
    {
      Vector3 targetPosition = GetRandomPosition();
      transform.position = targetPosition;
    }

    private Vector3 GetRandomPosition()
    {
      Vector3 targetPosition;
      Vector3 randomDirection = Random.insideUnitSphere * _settings.MoveDistance;
      randomDirection += transform.position;
      NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _settings.MoveDistance, 1);
      targetPosition = hit.position;
      targetPosition.y = _defaultPositionY;
      return targetPosition;
    }

    private void Attack() => _animator.PlayAttack();

    private void Appear()
    {
      _wormGround.Show();
      _animator.PlayAppear();
      _collider.enabled = true;
    }

    private void Disappear()
    {
      _collider.enabled = false;
      _wormGround.Hide();
      _animator.PlayDisappear();
    }

    private void CheckForHero()
    {
      Collider heroCollider = RoomZone.GetHeroFromZone();

      if (heroCollider == null) return;
      
      _hero = heroCollider.GetComponent<Hero>();
      _isHeroInRoomZone = true;
    }
    
    private void RotateToHero()
    {
      if(_isHeroInRoomZone == false) return;
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