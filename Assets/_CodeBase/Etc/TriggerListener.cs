using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class TriggerListener : MonoBehaviour
  {
    public event Action<Collider> Entered;
    public event Action<Collider> Canceled;

    public IReadOnlyList<Collider> CollidersInZone => _collidersInZone;

    private readonly List<Collider> _collidersInZone = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
      _collidersInZone.AddIfNotExists(other);
      Entered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
      _collidersInZone.Remove(other);
      Canceled?.Invoke(other);
    }

    public Collider GetHeroFromZone() => 
      CollidersInZone.FirstOrDefault(other => other != null && other.GetComponent<Hero>() != null);
  }
}