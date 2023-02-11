using System;
using UnityEngine;

namespace _CodeBase.UI
{
  public class BossScreenAnimator : MonoBehaviour
  {
    public event Action Showed;
    
    [SerializeField] private Animator _animator;
    
    private readonly int _playHash = Animator.StringToHash("Play");
    
    public void Play() => _animator.SetTrigger(_playHash);
    
    private void OnAnimationFinish() => Showed?.Invoke();
  }
}