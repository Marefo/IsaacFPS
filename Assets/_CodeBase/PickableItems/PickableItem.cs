using System;
using _CodeBase.HeroCode;
using UnityEngine;

namespace _CodeBase.PickableItems
{
  public abstract class PickableItem : MonoBehaviour
  {
    protected bool _used;
    
    private void OnCollisionEnter(Collision collision)
    {
      if(_used || collision.gameObject.TryGetComponent(out Hero hero) == false) return;
      OnCollisionWithHero(hero);
    }

    protected abstract void OnCollisionWithHero(Hero hero);

    protected void SetAsUsed() => _used = true;
  }
}