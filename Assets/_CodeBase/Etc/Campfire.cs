using System;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using _CodeBase.Logging;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class Campfire : MonoBehaviour, IDamageable
  {
    [SerializeField] private float _punchScaleStrength;
    [SerializeField] private float _punchScaleTime;
    [Space(10)]
    [SerializeField] private Transform _particles;
    [SerializeField] private Health _health;

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
      float healthPercent = Mathf.InverseLerp(0, _health.MaxValue, currentValue);
      _particles.DOKill();
      _particles.localScale = Vector3.one * healthPercent;
      _particles.DOPunchScale(Vector3.one * _punchScaleStrength, _punchScaleTime).SetLink(gameObject);
    }
  }
}