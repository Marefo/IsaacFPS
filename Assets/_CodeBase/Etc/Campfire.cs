using System;
using _CodeBase.IndicatorCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Interfaces;
using _CodeBase.Logging;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _CodeBase.Etc
{
  public class Campfire : MonoBehaviour, IDamageable
  {
    [SerializeField] private float _punchScaleStrength;
    [SerializeField] private float _punchScaleTime;
    [Space(10)]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _particles;
    [SerializeField] private Health _health;

    private bool _choked;
    private AudioService _audioService;

    [Inject]
    public void Construct(AudioService audioService)
    {
      _audioService = audioService;
    }
    
    private void OnEnable()
    {
      _health.HealthAmountChanged += OnHealthAmountChange;
    }

    private void OnDisable()
    {
      _health.HealthAmountChanged -= OnHealthAmountChange;
    }

    public void ReceiveDamage(int damageValue, Vector3 position) => _health.DecreaseForOne();

    [Button]
    private void PlayPunchScaleEffect()
    {
      _particles.DOKill();
      _particles.DOPunchScale(Vector3.one * _punchScaleStrength, _punchScaleTime).SetLink(gameObject);
    }
    
    private void OnHealthAmountChange(int currentValue)
    {
      if(_choked) return;
      float healthPercent = Mathf.InverseLerp(0, _health.MaxValue, currentValue);
      _particles.DOKill();
      _particles.localScale = Vector3.one * healthPercent;

      if (currentValue == 0)
      {
        _choked = true;
        _audioSource.Stop();
        _audioService.PlaySfx(_audioService.SfxData.FireChoke);
      }
      
      _particles.DOPunchScale(Vector3.one * _punchScaleStrength, _punchScaleTime).SetLink(gameObject);
    }
  }
}