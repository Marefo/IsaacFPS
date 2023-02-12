using System;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _CodeBase.PickableItems
{
  public abstract class PickableItem : MonoBehaviour
  {
    protected bool _used;
    protected AudioService _audioService { get; private set; }

    [Inject]
    public void Construct(AudioService audioService)
    {
      Initialize(audioService);
    }
    
    public void Initialize(AudioService audioService) => _audioService = audioService;

    private void OnCollisionEnter(Collision collision)
    {
      if(_used || collision.gameObject.TryGetComponent(out Hero hero) == false) return;
      OnCollisionWithHero(hero);
    }

    protected abstract void OnCollisionWithHero(Hero hero);

    protected void SetAsUsed() => _used = true;
  }
}