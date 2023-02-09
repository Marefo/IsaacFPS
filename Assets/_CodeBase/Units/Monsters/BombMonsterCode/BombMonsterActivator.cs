using System;
using _CodeBase.ItemsDrop;
using _CodeBase.Units.Monsters.BombMonsterCode.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Units.Monsters.BombMonsterCode
{
  public class BombMonsterActivator : MonoBehaviour
  {
    [SerializeField] private BombMonster _bombMonster;
    [SerializeField] private DropItem _dropItem;
    [Space(10)] 
    [SerializeField] private BombMonsterSettings _settings;

    private void OnEnable() => _dropItem.Pushed += OnPush;
    private void OnDisable() => _dropItem.Pushed -= OnPush;

    private void OnPush() => 
      DOVirtual.DelayedCall(_settings.ActivationDelay, () => _bombMonster.enabled = true).SetLink(gameObject);
  }
}