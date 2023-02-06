using System;
using _CodeBase.Units.Monsters.WormCode.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Units.Monsters.WormCode
{
  public class WormGround : MonoBehaviour
  {
    [SerializeField] private WormSettings _settings;

    public void Show(Action onComplete = null)
    {
      transform.DOKill();
      transform.localScale = Vector3.zero;
      transform.DOScale(Vector3.one, _settings.GroundAppearTime)
        .OnComplete(() => onComplete?.Invoke())
        .SetLink(gameObject);
    }

    public void Hide(Action onComplete = null)
    {
      transform.DOKill();
      transform.localScale = Vector3.one;
      transform.DOScale(Vector3.zero, _settings.GroundDisappearTime)
        .OnComplete(() => onComplete?.Invoke())
        .SetLink(gameObject);
    }
  }
}