using System;
using UnityEngine;

namespace _CodeBase.UI
{
  public class BossScreen : MonoBehaviour
  {
    public event Action Showed;
    
    [SerializeField] private GameObject _visual;
    [SerializeField] private BossScreenAnimator _animator;
    
    private void OnEnable() => _animator.Showed += OnShowed;
    private void OnDisable() => _animator.Showed -= OnShowed;

    private void OnShowed() => Showed?.Invoke();

    public void Show()
    {
      _visual.SetActive(true);
      _animator.Play();
    }

    public void Hide() => _visual.SetActive(false);
  }
}