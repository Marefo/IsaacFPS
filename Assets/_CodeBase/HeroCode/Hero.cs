using _CodeBase.Data;
using _CodeBase.IndicatorCode;
using _CodeBase.Units;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour
  {
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UnitAnimator _animator;
    [Space(10)]
    [SerializeField] private ContactDamageSettings _contactDamageSettings;

    public void ApplyContactDamage(Vector3 contactPoint)
    {
      _animator.PlayAttack();
      _health.Decrease(_contactDamageSettings.Damage);
      Vector3 knockBackDirection = Vector3.Normalize(transform.position - contactPoint);
      _rigidbody.velocity = Vector3.zero;
      _rigidbody.AddForce(knockBackDirection * _contactDamageSettings.KnockBackForce, ForceMode.Impulse);
    }
  }
}