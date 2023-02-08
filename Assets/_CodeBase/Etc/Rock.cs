using _CodeBase.Interfaces;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class Rock : MonoBehaviour, IExplosive
  {
    [SerializeField] private ParticleSystem _explodeVfx;
    
    public void Explode()
    {
      Instantiate(_explodeVfx, transform.position, _explodeVfx.transform.rotation);
      Destroy(gameObject);
    }
  }
}