using UnityEngine;
using UnityEngine.UI;

namespace _CodeBase.UI.Buttons
{
  public abstract class ButtonUI : MonoBehaviour
  {
    [SerializeField] private Button _button;

    protected virtual void OnEnable() => _button.onClick.AddListener(OnClick);
    protected virtual void OnDisable() => _button.onClick.RemoveListener(OnClick);

    protected abstract void OnClick();
  }
}