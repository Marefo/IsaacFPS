using System;
using UnityEngine;

namespace _CodeBase.Units.Monsters.MonstroCode
{
  public class MonstroAnimator : MonoBehaviour
  {
    public event Action AttackFramePlayed;
    public event Action AttackFinished;
    public event Action JumpImpactStarted;
    public event Action JumpImpactFinished;

    private void OnAttackFrame() => AttackFramePlayed?.Invoke();
    private void OnAttackFinish() => AttackFinished?.Invoke();
    private void OnJumpImpactStart() => JumpImpactStarted?.Invoke();
    private void OnJumpImpactFinish() => JumpImpactFinished?.Invoke();
  }
}