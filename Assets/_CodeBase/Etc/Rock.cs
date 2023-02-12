using _CodeBase.Extensions;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Interfaces;
using UnityEngine;
using Zenject;

namespace _CodeBase.Etc
{
  public class Rock : MonoBehaviour, IExplosive
  {
    [SerializeField] private ParticleSystem _explodeVfx;

    private AudioService _audioService;
    
    [Inject]
    public void Construct(AudioService audioService)
    {
      _audioService = audioService;
    }
    
    public void Explode()
    {
      _audioService.PlaySfx(_audioService.SfxData.RockDestroy.GetRandomValue());
      Instantiate(_explodeVfx, transform.position, _explodeVfx.transform.rotation);
      Destroy(gameObject);
    }
  }
}