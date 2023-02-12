using System;
using System.Collections;
using _CodeBase.Data;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ItemsDrop;
using _CodeBase.Logging;
using _CodeBase.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _CodeBase.Etc
{
  public class Chest : MonoBehaviour
  {
    [SerializeField] private bool _withWinnerLetter;
    [Space(10)]
    [SerializeField] private Transform _model;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private GameObject _lid;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private ItemsDropper _itemsDropper;
    [Space(10)]
    [SerializeField] private ChestSettings _settings;

    private AudioService _audioService;
    private Vector3 _defaultModelScale;
    private WinnerLetter _winnerLetter;
    private bool _used;

    [Inject]
    public void Construct(WinnerLetter winnerLetter, AudioService audioService)
    {
      Initialize(winnerLetter, audioService);
    }
    
    private void OnEnable() => _groundChecker.Landed += OnLand;
    private void OnDisable() => _groundChecker.Landed -= OnLand;

    private void Start() => _defaultModelScale = _model.localScale;

    private void OnCollisionEnter(Collision collision)
    {
      if(_used || collision.gameObject.TryGetComponent(out Hero hero) == false) return;
      _used = true;
      StartCoroutine(OpenCoroutine(hero));
    }

    private void OnLand(float velocityY)
    {
      float volume = Mathf.InverseLerp(0, 10, Mathf.Abs(velocityY));
      _audioService.PlaySfx(_audioService.SfxData.ChestLand, true, volume);
    }

    public void Initialize(WinnerLetter winnerLetter, AudioService audioService)
    {
      _winnerLetter = winnerLetter;
      _audioService = audioService;
      _itemsDropper.Initialize(_audioService);
    }

    private IEnumerator OpenCoroutine(Hero hero)
    {
      _model.DOShakePosition(_settings.ShakeTime, _settings.ShakeStrength, _settings.ShakeVibrato, 90, 
        false, false).SetLink(gameObject);
      
      yield return new WaitForSeconds(_settings.ShakeDuration);
      
      _model.DOKill();
      _model.DOPunchScale(_defaultModelScale * _settings.PunchScaleStrength, _settings.PunchScaleTime, 
        _settings.PunchScaleVibrato).SetLink(gameObject);
      _audioService.PlaySfx(_audioService.SfxData.ChestOpen);
      
      yield return new WaitForSeconds(_settings.PunchScaleTime);
      
      _model.DOKill();
      _particles.gameObject.SetActive(false);
      _lid.transform.SetParent(null);
      Vector3 directionFromHero = Vector3.Normalize(transform.position - hero.transform.position);
      directionFromHero.y = 1;
      Rigidbody _lidRigidbody = _lid.AddComponent<Rigidbody>();
      _lidRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
      _lidRigidbody.AddForce(directionFromHero * _settings.LidPushForce, ForceMode.Impulse);

      if (_withWinnerLetter == false)
        _itemsDropper.TryDropItem(directionFromHero);
      else
        _winnerLetter.Open();
    }
  }
}