﻿using System;
using _CodeBase.Extensions;
using UnityEngine;
using UnityEngine.AI;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.Units.Monsters.FlyCode
{
  public class Fly : Monster
  {
    [SerializeField] private Transform _deathVfxPoint;
    [SerializeField] private ParticleSystem _deathVfx;

    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnSubscribeEvents();

    protected override void Die()
    {
      Instantiate(_deathVfx, _deathVfxPoint.position, Quaternion.identity);
      base.Die();
      Destroy(gameObject);
    }
  }
}