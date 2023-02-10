using System;
using UnityEngine;

namespace _CodeBase.Units.Monsters.HiveCode
{
  public class HiveAnimator : MonoBehaviour
  {
    public event Action AttackFramePlayed;

    private void OnAttackFrame() => AttackFramePlayed?.Invoke();
  }
}