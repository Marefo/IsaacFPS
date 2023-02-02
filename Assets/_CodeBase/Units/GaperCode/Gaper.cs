using System;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using _CodeBase.Interfaces;
using _CodeBase.Units.GaperCode.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Units.GaperCode
{
  public class Gaper : MonoBehaviour, IDamageable
  {
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [Space(10)]
    [SerializeField] private Health _health;
    [SerializeField] private TriggerListener _attackZone;
    [Space(10)] 
    [SerializeField] private GaperSettings _settings;

    private bool _isAttacking;
    private bool _attacked;
    
    private void OnEnable()
    {
      _health.ValueCameToZero += Die;
      _attackZone.Entered += OnAttackZoneEnter;
    }

    private void OnDisable()
    {
      _health.ValueCameToZero -= Die;
      _attackZone.Entered -= OnAttackZoneEnter;
    }

    public void ReceiveDamage(int damageValue, Vector3 position) => _health.Decrease(damageValue);

    private void OnAttackZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false || _isAttacking == false || _attacked) return;
      ApplyDamageToHero(hero);
    }

    public void Attack()
    {
      _isAttacking = true;
      DOVirtual.DelayedCall(_settings.AttackDuration, ResetAttack);

      Collider heroCollider = _attackZone.CollidersInZone.FirstOrDefault(other => other.TryGetComponent(out Hero hero));
      
      if(heroCollider != null)
        ApplyDamageToHero(heroCollider.GetComponent<Hero>());
    }

    private void ResetAttack()
    {
      _isAttacking = false;
      _attacked = false;
    }

    private void ApplyDamageToHero(Hero hero)
    {
      hero.ApplyContactDamage(transform.position);
      _attacked = true;
    }
    
    private void Die()
    {
      Instantiate(_deathVfx, _deathVfxSpawnPoint.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}