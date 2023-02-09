using _CodeBase.Logging;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _CodeBase.UI
{
  public class HoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public void OnPointerEnter(PointerEventData eventData)
    {
      transform.DOKill();
      transform.localScale = Vector3.one;
      transform.DOScale(Vector3.one * 1.2f, 0.25f).SetUpdate(true).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      transform.DOKill();
      transform.DOScale(Vector3.one, 0.15f).SetUpdate(true).SetLink(gameObject);
    }
  }
}