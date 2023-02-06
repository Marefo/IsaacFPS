using System;
using System.Collections;
using _CodeBase.CameraCode;
using _CodeBase.CameraCode.Data;
using _CodeBase.Data;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using _CodeBase.Logging;
using _CodeBase.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour, IDamageable
  {
    [field: SerializeField] public Transform ShootTarget { get; private set; }
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private Volume _volume;
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UnitAnimator _animator;
    [Space(10)]
    [SerializeField] private ShakeSettings _shakeSettings;
    [SerializeField] private ContactDamageSettings _contactDamageSettings;

    private Vignette _vignette;
    private Tween _resetVignetteColorTween;
    
    private void Start()
    {
      _volume.profile.TryGet(out Vignette vignette);
      _vignette = vignette;
    }

    public void ReceiveDamage(int damageValue, Vector3 position) => ApplyContactDamage(position);

    public void ApplyContactDamage(Vector3 contactPoint)
    {
      PlayDamagedScreenEffect();
      _cameraShaker.Shake(_shakeSettings);
      _animator.PlayAttack();
      _health.Decrease(_contactDamageSettings.Damage);
      Vector3 knockBackDirection = Vector3.Normalize(transform.position - contactPoint);
      _rigidbody.velocity = Vector3.zero;
      _rigidbody.AddForce(knockBackDirection * _contactDamageSettings.KnockBackForce, ForceMode.Impulse);
    }

    [Button]
    private void PlayDamagedScreenEffect()
    {
      _resetVignetteColorTween?.Kill();
      StopAllCoroutines();
      _vignette.color.value = Color.black;
      
      StartCoroutine(ChangeVignetteColorCoroutine(Color.red, _shakeSettings.Duration / 2.1f));
      _resetVignetteColorTween = DOVirtual.DelayedCall(_shakeSettings.Duration / 2, ResetVignetteColor);
    }

    private void ResetVignetteColor()
    {
      _resetVignetteColorTween?.Kill();
      StopAllCoroutines();
      StartCoroutine(ChangeVignetteColorCoroutine(Color.black, _shakeSettings.Duration / 2.1f));
    }

    private IEnumerator ChangeVignetteColorCoroutine(Color targetColor, float duration)
    {
      Color startColor = _vignette.color.value;
      float time = 0f;

      while(true)
      {
        if(time >= duration)
          yield break;
        else
          yield return null;
        
        float percent = Mathf.InverseLerp(0, duration, time);
        _vignette.color.value = Color.Lerp(startColor, targetColor, percent);
        time += Time.deltaTime;
      }
    }
  }
}