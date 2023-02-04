using System;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class TriggerDamageDealer : MonoBehaviour
  {
    [SerializeField] private TriggerListener _zone;
    [SerializeField] private bool _hasHealth;
    [SerializeField, ShowIf("_hasHealth")] private Health _health;

    private void OnEnable() => _zone.Entered += OnZoneEnter;
    private void OnDisable() => _zone.Entered -= OnZoneEnter;

    private void OnZoneEnter(Collider other)
    {
      if(other.TryGetComponent(out Hero hero) == false || _hasHealth && _health.IsValueZero) return;
      hero.ApplyContactDamage(transform.position);
    }
  }
}