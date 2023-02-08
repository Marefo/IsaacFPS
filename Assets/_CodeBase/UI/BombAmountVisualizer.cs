using _CodeBase.HeroCode;
using TMPro;
using UnityEngine;

namespace _CodeBase.UI
{
  public class BombAmountVisualizer : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private HeroShooter _heroShooter;

    private bool _initialized;

    private void OnEnable() => _heroShooter.BombsAmountChanged += OnBombsAmountChange;
    private void OnDisable() => _heroShooter.BombsAmountChanged -= OnBombsAmountChange;

    private void OnBombsAmountChange(int newValue) => _textField.text = $"x{newValue}";
  }
}