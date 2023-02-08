using System;
using System.Collections.Generic;
using _CodeBase.Extensions;
using _CodeBase.HeroCode.Data;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ShooterCode;
using _CodeBase.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace _CodeBase.HeroCode
{
  public class HeroShooter : MonoBehaviour
  {
    public event Action<int> BombsAmountChanged;
    
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private List<ParticleSystem> _shootVfxList;
    [Space(10)] 
    [SerializeField] private float _rippleTime;
    [SerializeField] private Material _rippleMaterial;
    [Space(10)] 
    [SerializeField] private HeroMovement _heroMovement;
    [SerializeField] private HandsAnimator _handsAnimator;
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private UniversalRendererData _rendererData;
    [Space(10)] 
    [SerializeField] private HeroShooterSettings _settings;

    private float? _lastShootTime;
    private float? _lastThrowTime;
    private InputService _inputService;
    private Grenade _currentGrenade;
    private bool _canThrowGrenade = true;
    private int _bombsAmount;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    private void Awake() => FinishChargedVfx();

    private void OnEnable()
    {
      _inputService.AttackButtonClicked += TryShoot;
      _inputService.ThrowGrenadeButtonClicked += TryThrowGrenade;
      _handsAnimator.GrenadePickedUp += OnGrenadePickedUp;
      _handsAnimator.ThrowFramePlayed += OnThrowFrame;
    }

    private void OnDisable()
    {
      _inputService.AttackButtonClicked -= TryShoot;
      _inputService.ThrowGrenadeButtonClicked -= TryThrowGrenade;
      _handsAnimator.GrenadePickedUp -= OnGrenadePickedUp;
      _handsAnimator.ThrowFramePlayed -= OnThrowFrame;
    }

    private void Start() => IncreaseBombAmount();

    public void IncreaseBombAmount()
    {
      _bombsAmount += 1;
      BombsAmountChanged?.Invoke(_bombsAmount);
    }

    private void DecreaseBombAmount()
    {
      _bombsAmount -= 1;
      BombsAmountChanged?.Invoke(_bombsAmount);
    }

    private void TryShoot()
    {
      if (_lastShootTime == null || Time.time > _lastShootTime.Value + _settings.Delay) 
        Shoot();
    }

    private void OnGrenadePickedUp()
    {
      _currentGrenade = Instantiate(_settings.GrenadePrefab, _throwPoint.position, Quaternion.identity);
      _currentGrenade.transform.SetParent(_throwPoint);
      _currentGrenade.transform.SetLossyScale(1, 1, 1);
    }

    private void OnThrowFrame()
    {
      DecreaseBombAmount();
      _currentGrenade.transform.SetParent(null);
      _currentGrenade.transform.SetLossyScale(1, 1, 1);
      _currentGrenade.transform.DORotate(Vector3.zero, 0.1f).SetLink(_currentGrenade.gameObject);
      LaunchData launchData = CalculateGrenadeLaunchData();
      _currentGrenade.OnShoot(launchData.InitialVelocity, launchData.ThrowDistance, _settings.GrenadeSettings.Gravity, _settings.GrenadeSettings.Damage);
      _currentGrenade.EnableDestroy();
      DOVirtual.DelayedCall(0.1f, EnableGrenade).SetLink(gameObject);
    }

    private void EnableGrenade()
    {
      if(_currentGrenade != null)
        _currentGrenade.EnableDamage();
      
      _currentGrenade = null;
      _canThrowGrenade = true;
    }

    private void TryThrowGrenade()
    {
      if (_bombsAmount > 0 && _canThrowGrenade) 
        Throw();
    }

    private void Throw()
    {
      _canThrowGrenade = false;
      _animator.PlayThrow();
      _lastShootTime = Time.time;
    }

    private void Shoot()
    {
      _animator.PlayAttack();
      PlayChargedVfx();
      _shootVfxList.ForEach(vfx => vfx.Play());
      
      Bullet projectile = Instantiate(_settings.ProjectilePrefab, _shootPoint.position, Quaternion.identity);
      projectile.OnShoot(_camera.forward, _settings.BulletSettings);
      
      _lastShootTime = Time.time;
    }

    private void PlayChargedVfx()
    {
      _rippleMaterial.SetFloat("_Input", 0);
      _rendererData.rendererFeatures[2].SetActive(true);

      DOTween.To(() => _rippleMaterial.GetFloat("_Input"),
        x => _rippleMaterial.SetFloat("_Input", x), 0.99f, _rippleTime);
      
      DOVirtual.DelayedCall(_rippleTime, FinishChargedVfx);
    }

    private void FinishChargedVfx() => 
      _rendererData.rendererFeatures[2].SetActive(false);

    private LaunchData CalculateGrenadeLaunchData()
    {
      Transform _aimStartPoint = _throwPoint;
      float throwDistance = _heroMovement.IsMoving
        ? _settings.GrenadeSettings.MaxThrowDistance * _settings.GrenadeSettings.MovingThrowDistanceMultiplier
        : _settings.GrenadeSettings.MaxThrowDistance;
      Vector3 _aimTargetPoint = transform.position + _camera.forward * throwDistance;
      _aimTargetPoint.y = _aimStartPoint.position.y;
      float _gravity = -_settings.GrenadeSettings.Gravity;
      float height = _settings.GrenadeSettings.Height;
      float displacementY = _aimTargetPoint.y - _aimStartPoint.position.y;
      Vector3 displacementXZ = new Vector3 (_aimTargetPoint.x - _aimStartPoint.position.x, 0, _aimTargetPoint.z - _aimStartPoint.position.z);
      float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);
      Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * _gravity * height);
      Vector3 velocityXZ = displacementXZ / time;

      return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), throwDistance);
    }
    
    private struct LaunchData
    {
      public readonly Vector3 InitialVelocity;
      public readonly float ThrowDistance;

      public LaunchData(Vector3 initialVelocity, float throwDistance)
      {
        InitialVelocity = initialVelocity;
        ThrowDistance = throwDistance;
      }
    }
  }
}