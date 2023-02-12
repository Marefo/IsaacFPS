using _CodeBase.HeroCode;

namespace _CodeBase.PickableItems
{
  public class PickableBomb : PickableItem
  {
    protected override void OnCollisionWithHero(Hero hero)
    {
      _audioService.PlaySfx(_audioService.SfxData.PickUp, true);
      HeroShooter shooter = hero.GetComponent<HeroShooter>();
      shooter.IncreaseBombAmount();
      SetAsUsed();
      Destroy(gameObject);
    }
  }
}