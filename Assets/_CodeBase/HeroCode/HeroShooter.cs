using System;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.HeroCode.Data;
using _CodeBase.IndicatorCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ShooterCode;
using _CodeBase.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace _CodeBase.HeroCode
{
  public class HeroShooter : MonoBehaviour
  {
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private List<ParticleSystem> _shootVfxList;
    [Space(10)] 
    [SerializeField] private float _rippleTime;
    [SerializeField] private Material _rippleMaterial;
    [Space(10)] 
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private UniversalRendererData _rendererData;
    [Space(10)] 
    [SerializeField] private HeroShooterSettings _settings;

    private float? _lastShootTime;
    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    private void Awake() => FinishChargedVfx();

    private void OnEnable() => _inputService.AttackButtonClicked += TryShoot;
    private void OnDisable() => _inputService.AttackButtonClicked -= TryShoot;

    private void TryShoot()
    {
      if (_lastShootTime == null || Time.time > _lastShootTime.Value + _settings.Delay) 
        Shoot();
    }

    private void Shoot()
    {
      _animator.PlayAttack();
      PlayChargedVfx();
      _shootVfxList.ForEach(vfx => vfx.Play());
      
      HeroBullet projectile = Instantiate(_settings.ProjectilePrefab, _shootPoint.position, Quaternion.identity);
      projectile.OnShoot(_camera.forward, _settings);
      
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
  }
}