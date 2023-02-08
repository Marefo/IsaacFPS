using _CodeBase.HeroCode;
using _CodeBase.IndicatorCode;
using UnityEngine;

namespace _CodeBase.PickableItems
{
  public class PickableHeart : PickableItem
  {
    protected override void OnCollisionWithHero(Hero hero)
    {
      Health health = hero.GetComponent<Health>();
      
      if(health.CurrentValue == health.MaxValue) return; 
      
      health.Increase(1);
      SetAsUsed();
      Destroy(gameObject);
    }
  }
}