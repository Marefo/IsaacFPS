using System;
using System.Collections;
using _CodeBase.CameraCode;
using _CodeBase.CameraCode.Data;
using _CodeBase.Data;
using _CodeBase.Extensions;
using _CodeBase.IndicatorCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Interfaces;
using _CodeBase.UI;
using _CodeBase.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Zenject;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour, IDamageable
  {
    [SerializeField] private float _dieDelay;
    [SerializeField] private float _dieDuration;
    [SerializeField] private Transform _bloodVfxPoint;
    [SerializeField] private GameObject _bloodVfxPrefab;
    [field: Space(10)]
    [field: SerializeField] public Transform ShootTarget { get; private set; }
    [SerializeField] private HeroShooter _heroShooter;
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
    private SceneService _sceneService;
    private AudioService _audioService;
    private LoadingCurtain _loadingCurtain;

    [Inject]
    public void Construct(SceneService sceneService, AudioService audioService, LoadingCurtain loadingCurtain)
    {
      _sceneService = sceneService;
      _audioService = audioService;
      _loadingCurtain = loadingCurtain;
    }

    private void OnEnable() => _health.ValueCameToZero += OnValueBecomeZero;
    private void OnDisable() => _health.ValueCameToZero -= OnValueBecomeZero;

    private void Start()
    {
      _volume.profile.TryGet(out Vignette vignette);
      _vignette = vignette;
    }

    public void ReceiveDamage(int damageValue, Vector3 position) => ApplyContactDamage(position);

    public void ApplyContactDamage(Vector3 contactPoint)
    {
      Instantiate(_bloodVfxPrefab, _bloodVfxPoint.position, transform.rotation);
      PlayDamagedScreenEffect();
      _cameraShaker.Shake(_shakeSettings);
      
      if(_heroShooter.IsThrowingGrenade == false)
        _animator.PlayAttack();
      
      _health.Decrease(_contactDamageSettings.Damage);
      Vector3 knockBackDirection = Vector3.Normalize(transform.position - contactPoint);
      knockBackDirection.y = 0.25f;
      _rigidbody.velocity = Vector3.zero;
      _rigidbody.AddForce(knockBackDirection * _contactDamageSettings.KnockBackForce, ForceMode.Impulse);
      
      if(_health.IsValueZero == false)
        _audioService.PlaySfx(_audioService.SfxData.HeroDamage.GetRandomValue(), true);
    }

    private void OnValueBecomeZero() => DOVirtual.DelayedCall(_dieDelay, Die).SetLink(gameObject);

    private void Die()
    {
      _audioService.PlaySfx(_audioService.SfxData.HeroDeath.GetRandomValue());
      _resetVignetteColorTween?.Kill();
      StopAllCoroutines();
      StartCoroutine(CollapseVignetteCoroutine());
      _loadingCurtain.FadeIn(_dieDuration, OnDie);
    }

    private void OnDie()
    {
      _loadingCurtain.FadeOut(_dieDuration);
      _sceneService.ReloadCurrentScene();
    }

    [Button]
    private void PlayDamagedScreenEffect()
    {
      _resetVignetteColorTween?.Kill();
      StopAllCoroutines();
      _vignette.color.value = Color.black;
      
      StartCoroutine(ChangeVignetteColorCoroutine(Color.red, _shakeSettings.Duration / 2.1f));
      _resetVignetteColorTween = DOVirtual.DelayedCall(_shakeSettings.Duration / 2, ResetVignetteColor).SetLink(gameObject);
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
    
    private IEnumerator CollapseVignetteCoroutine(Action onComplete = null)
    {
      float startIntensity = _vignette.intensity.value;
      float startSmoothness = _vignette.smoothness.value;
      float time = 0f;

      while(true)
      {
        if (time >= _dieDuration)
        {
          onComplete?.Invoke();
          yield break;
        }
        else
          yield return null;
        
        float percent = Mathf.InverseLerp(0, _dieDuration, time);
        _vignette.intensity.value = Mathf.Lerp(startIntensity, _vignette.intensity.max / 2, percent);
        _vignette.smoothness.value = Mathf.Lerp(startSmoothness, _vignette.smoothness.max / 2, percent);
        time += Time.deltaTime;
      }
    }
  }
}