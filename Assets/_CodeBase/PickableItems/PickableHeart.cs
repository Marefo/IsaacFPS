using System;
using System.Collections;
using System.Collections.Generic;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.PickableItems
{
  public class PickableHeart : PickableItem
  {
    [SerializeField] private float _strength;
    [SerializeField] private float _time;
    [SerializeField] private float _delay;
    
    private void Start() => StartCoroutine(PunchScaleEffectCoroutine());

    protected override void OnCollisionWithHero(Hero hero)
    {
      Health health = hero.GetComponent<Health>();
      
      if(health.CurrentValue == health.MaxValue) return; 
      
      health.Increase(1);
      SetAsUsed();
      Destroy(gameObject);
    }

    private IEnumerator PunchScaleEffectCoroutine()
    {
      while (true)
      {
        PlayPunchScaleEffect();
        yield return new WaitForSeconds(_delay);
      }
    }

    private void PlayPunchScaleEffect()
    {
      transform.DOKill();
      transform.localScale = Vector3.one;
      transform.DOPunchScale(Vector3.one * _strength, _time).SetLink(gameObject);
    }
  }
}