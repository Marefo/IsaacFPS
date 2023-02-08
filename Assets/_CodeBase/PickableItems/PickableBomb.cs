using _CodeBase.HeroCode;

namespace _CodeBase.PickableItems
{
  public class PickableBomb : PickableItem
  {
    protected override void OnCollisionWithHero(Hero hero)
    {
      HeroShooter shooter = hero.GetComponent<HeroShooter>();
      shooter.IncreaseBombAmount();
      SetAsUsed();
      Destroy(gameObject);
    }
  }
}