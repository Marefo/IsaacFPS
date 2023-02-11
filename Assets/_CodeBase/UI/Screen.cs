using _CodeBase.Etc;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.UI
{
  public class Screen : MonoBehaviour
  {
    public bool IsVisible => _visual.localScale != Vector3.zero;
    
    [SerializeField] private Transform _visual;

    public virtual void Open()
    {
      CursorVisibilityController.Show();
      _visual.DOKill();
      _visual.DOScale(Vector3.one, 0.15f)
        .SetUpdate(true)
        .SetLink(gameObject);
    }

    public virtual void Close()
    {
      CursorVisibilityController.Hide();
      _visual.DOKill();
      _visual.DOScale(Vector3.zero, 0.15f)
        .SetUpdate(true)
        .SetLink(gameObject);
    }
  }
}