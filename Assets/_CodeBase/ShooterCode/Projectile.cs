using _CodeBase.Etc;
using _CodeBase.Infrastructure;
using _CodeBase.Interfaces;
using UnityEngine;

namespace _CodeBase.ShooterCode
{
  public abstract class Projectile : MonoBehaviour
  {
    [SerializeField] private LayerMask _destroyerLayer;
    [Space(10)]
    [SerializeField] private TriggerListener _zone;

    private void OnEnable() => _zone.Entered += OnZoneEnter;
    private void OnDisable() => _zone.Entered -= OnZoneEnter;

    private void OnZoneEnter(Collider obj)
    {
      if (obj.gameObject.TryGetComponent(out IDamageable damageable))
        OnDamageableZoneEnter(damageable);

      if (Helpers.CompareLayers(obj.gameObject.layer, _destroyerLayer))
        OnDestroyerZoneEnter();
    }

    protected abstract void OnDamageableZoneEnter(IDamageable damageable);
    protected virtual void OnDestroyerZoneEnter() => Destroy();
    protected abstract void Destroy();
  }
}