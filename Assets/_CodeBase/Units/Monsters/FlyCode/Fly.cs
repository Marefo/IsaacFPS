using _CodeBase.Extensions;
using UnityEngine;

namespace _CodeBase.Units.Monsters.FlyCode
{
  public class Fly : Monster
  {
    [SerializeField] private Transform _deathVfxPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [SerializeField] private GameObject _deathTrail;

    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnSubscribeEvents();

    protected override void Die()
    {
      _audioService.PlaySfx(_audioService.SfxData.FlyDeath.GetRandomValue(), true, 0.5f);
      Instantiate(_deathTrail, _deathVfxPoint.position, Quaternion.identity);
      Instantiate(_deathVfx, _deathVfxPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
  }
}