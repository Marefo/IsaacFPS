using System;
using UnityEngine;

namespace _CodeBase.Units.Monsters.WormCode
{
  public class WormAnimator : MonoBehaviour
  {
    public event Action AttackFramePlayed;
    
    [SerializeField] private Animator _animator;
    
    private readonly int _appearHash = Animator.StringToHash("Appear");
    private readonly int _disappearHash = Animator.StringToHash("Disappear");
    private readonly int _attackHash = Animator.StringToHash("Attack");

    public void PlayAppear() => _animator.SetTrigger(_appearHash);
    public void PlayDisappear() => _animator.SetTrigger(_disappearHash);
    public void PlayAttack() => _animator.SetTrigger(_attackHash);

    private void OnAttackFrame() => AttackFramePlayed?.Invoke();
  }
}