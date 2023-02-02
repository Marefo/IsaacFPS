using System;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using UnityEngine;

namespace _CodeBase.Units.GaperCode
{
  public class Gaper : MonoBehaviour, IDamageable
  {
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [Space(10)]
    [SerializeField] private Health _health;

    private void OnEnable() => _health.ValueCameToZero += Die;
    private void OnDisable() => _health.ValueCameToZero -= Die;

    public void ReceiveDamage(int damageValue, Vector3 position) => _health.Decrease(damageValue);

    private void Die()
    {
      Instantiate(_deathVfx, _deathVfxSpawnPoint.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}