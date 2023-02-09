using System.Collections.Generic;
using _CodeBase.Data;
using _CodeBase.Interfaces;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class Poop : MonoBehaviour, IDamageable
  {
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _destroyVfxPoint;
    [Space(10)]
    [SerializeField] private List<GameObject> _models;
    [Space(10)]
    [SerializeField] private PoopSettings _settings;

    private int _hitNumber;

    public void ReceiveDamage(int damageValue, Vector3 position)
    {
      _hitNumber += 1;
      PlayPunchScaleEffect();

      if (_hitNumber == 3)
      {
        Vector3 spawnPosition = position;
        spawnPosition.z = _destroyVfxPoint.position.z;
        Instantiate(_settings.DestroyVfx, spawnPosition, _settings.DestroyVfx.transform.rotation);
        Destroy(gameObject, _settings.PunchScaleTime);
      }
      else
        ChangeModelTo(_hitNumber);
    }

    [Button]
    private void PlayPunchScaleEffect()
    {
      _model.DOKill();
      _model.localScale = Vector3.one;
      _model.DOPunchScale(Vector3.one * _settings.PunchScaleStrength, _settings.PunchScaleTime);
    }
    
    private void ChangeModelTo(int index)
    {
      _models.ForEach(model => model.SetActive(false));
      _models[index].SetActive(true);
    }
  }
}