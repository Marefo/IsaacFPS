using System;
using UnityEngine;

namespace _CodeBase.Units.Monsters.GaperCode
{
  public class GaperAnimator : MonoBehaviour
  {
    public event Action AttackFramePlayed;
    
    private void OnAttackFrame() => AttackFramePlayed?.Invoke();
  }
}