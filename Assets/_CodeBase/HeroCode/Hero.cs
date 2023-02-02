using System;
using System.Collections;
using _CodeBase.CameraCode;
using _CodeBase.CameraCode.Data;
using _CodeBase.Data;
using _CodeBase.IndicatorCode;
using _CodeBase.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour
  {
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private Volume _volume;
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UnitAnimator _animator;
    [Space(10)]
    [SerializeField] private ShakeSettings _shakeSettings;
    [SerializeField] private ContactDamageSettings _contactDamageSettings;

    private Vignette _vignette;
    private Coroutine _changeVignetteColorCoroutine;
    private Tween _resetVignetteColorTween;
    
    private void Start()
    {
      _volume.profile.TryGet(out Vignette vignette);
      _vignette = vignette;
    }

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

    private void PlayDamagedScreenEffect()
    {
      if (_changeVignetteColorCoroutine != null)
      {
        StopCoroutine(_changeVignetteColorCoroutine);
        _resetVignetteColorTween?.Kill();
        _vignette.color.value = Color.black;
      }
      
      _changeVignetteColorCoroutine = 
        StartCoroutine(ChangeVignetteColorCoroutine(Color.red, _shakeSettings.Duration / 2));

      _resetVignetteColorTween = DOVirtual.DelayedCall(_shakeSettings.Duration / 2, 
        () => StartCoroutine(ChangeVignetteColorCoroutine(Color.black, _shakeSettings.Duration / 2)));
    }

    private IEnumerator ChangeVignetteColorCoroutine(Color targetColor, float duration)
    {
      Color startColor = _vignette.color.value;
      float time = 0f;
 
      while(time <= duration)
      {
        float percent = Mathf.InverseLerp(0, duration, time);
        _vignette.color.value = Color.Lerp(startColor, targetColor, percent);
        time += Time.deltaTime;
        yield return null;
      }
    }
  }
}