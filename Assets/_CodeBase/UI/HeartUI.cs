using _CodeBase.UI.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _CodeBase.UI
{
  public class HeartUI : MonoBehaviour
  {
    public float Value { get; private set; }
    
    [SerializeField] private Sprite _fullSprite;
    [SerializeField] private Sprite _halfSprite;
    [SerializeField] private Sprite _emptySprite;
    [Space(10)] 
    [SerializeField] private Image _image;
    [Space(10)] 
    [SerializeField] private HeartSettings _settings;

    public void ChangeValue(float value)
    {
      Value = value;
      UpdateSprite();
      PlayPunchScaleVfx();
    }

    private void UpdateSprite()
    {
      Sprite newSprite;
      
      if (Value > _settings.HeartValue / 2)
        newSprite = _fullSprite;
      else if (Value > 0 && Value <= _settings.HeartValue / 2)
        newSprite = _halfSprite;
      else
        newSprite = _emptySprite;

      _image.sprite = newSprite;
    }

    private void PlayPunchScaleVfx()
    {
      transform.DOKill();
      transform.DOPunchScale(Vector3.one * _settings.Punch, _settings.Duration).SetLink(gameObject);
    }
  }
}