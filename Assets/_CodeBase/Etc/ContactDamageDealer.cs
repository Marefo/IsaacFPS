using System;
using _CodeBase.Data;
using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using _CodeBase.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Etc
{
  [RequireComponent(typeof(Collider))]
  public class ContactDamageDealer : MonoBehaviour
  {
    [SerializeField] private bool _hasHealth;
    [SerializeField, ShowIf("_hasHealth")] private Health _health;

    private void OnCollisionEnter(Collision collision)
    {
      MyDebug.Log($"collision = {collision.gameObject}", MyDebug.DebugColor.green);
      if(collision.gameObject.TryGetComponent(out Hero hero) == false || _hasHealth && _health.IsValueZero) return;
      hero.ApplyContactDamage(collision.GetContact(0).point);
    }
  }
}