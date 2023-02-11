using System;
using System.Collections;
using _CodeBase.Data;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.ItemsDrop;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class Chest : MonoBehaviour
  {
    [SerializeField] private Transform _model;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private GameObject _lid;
    [SerializeField] private ItemsDropper _itemsDropper;
    [Space(10)]
    [SerializeField] private ChestSettings _settings;

    private Vector3 _defaultModelScale;
    private bool _used;

    private void Start() => _defaultModelScale = _model.localScale;

    private void OnCollisionEnter(Collision collision)
    {
      if(_used || collision.gameObject.TryGetComponent(out Hero hero) == false) return;
      _used = true;
      StartCoroutine(OpenCoroutine(hero));
    }

    private IEnumerator OpenCoroutine(Hero hero)
    {
      _model.DOShakePosition(_settings.ShakeTime, _settings.ShakeStrength, _settings.ShakeVibrato, 90, 
        false, false).SetLink(gameObject);
      
      yield return new WaitForSeconds(_settings.ShakeDuration);
      
      _model.DOKill();
      _model.DOPunchScale(_defaultModelScale * _settings.PunchScaleStrength, _settings.PunchScaleTime, 
        _settings.PunchScaleVibrato).SetLink(gameObject);
      
      yield return new WaitForSeconds(_settings.PunchScaleTime);
      
      _model.DOKill();
      _particles.gameObject.SetActive(false);
      _lid.transform.SetParent(null);
      Vector3 directionFromHero = Vector3.Normalize(transform.position - hero.transform.position);
      directionFromHero.y = 1;
      Rigidbody _lidRigidbody = _lid.AddComponent<Rigidbody>();
      _lidRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
      _lidRigidbody.AddForce(directionFromHero * _settings.LidPushForce, ForceMode.Impulse);

      _itemsDropper.TryDropItem(directionFromHero);
    }
  }
}