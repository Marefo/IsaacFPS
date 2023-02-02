using System.Collections.Generic;
using System.Linq;
using _CodeBase.IndicatorCode;
using _CodeBase.UI.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.UI
{
  public class HealthVisualizer : MonoBehaviour
  {
    [SerializeField] private HeartUI _heartUIPrefab;
    [SerializeField] private Transform _heartsUIParent;
    [SerializeField] private Health _health;
    [Space(10)] 
    [SerializeField] private HeartSettings _heartSettings;
    
    private float _showedHeathValue => _hearts.Sum(heart => heart.Value);
    
    private bool _initialized;
    private readonly List<HeartUI> _hearts = new List<HeartUI>();

    private void OnEnable() => _health.HealthAmountChanged += OnHealthValueChange;
    private void OnDisable() => _health.HealthAmountChanged -= OnHealthValueChange;

    private void OnHealthValueChange(int newValue)
    {
      if (_initialized == false)
      {
        _initialized = true;
        int spawnAmount = Mathf.FloorToInt(newValue / _heartSettings.HeartValue);
        SpawnHearts(spawnAmount);
        return;
      }

      if (_showedHeathValue > newValue)
      {
        float difference = _showedHeathValue - newValue;
        ChangeHeartsState(difference, true, false);
      }
      else if (newValue > _showedHeathValue)
      {
        float difference = newValue - _showedHeathValue;
        ChangeHeartsState(difference, false, true);
      }
    }

    private void SpawnHearts(int amount)
    {
      for (int i = 0; i < amount; i++)
      {
        HeartUI heart = Instantiate(_heartUIPrefab, _heartsUIParent);
        heart.ChangeValue(_heartSettings.HeartValue);
        _hearts.Add(heart);
      }
    }

    private void ChangeHeartsState(float difference, bool decrease, bool firstItems)
    {
      while (difference != 0)
      {
        HeartUI currentHeart;
        
        if(firstItems)
         currentHeart = _hearts.First(heart => decrease ? heart.Value > 0 : heart.Value < _heartSettings.HeartValue);
        else
          currentHeart = _hearts.Last(heart => decrease ? heart.Value > 0 : heart.Value < _heartSettings.HeartValue);

        if (decrease)
        {
          float affect = difference >= _heartSettings.HeartValue ? _heartSettings.HeartValue : difference;
          float newDifference = Mathf.Clamp(difference - currentHeart.Value, 0, _health.MaxValue);
          currentHeart.ChangeValue(currentHeart.Value - affect);
          difference = newDifference;
        }
        else
        {
          float affect = difference >= _heartSettings.HeartValue ? _heartSettings.HeartValue : difference;
          float newDifference = Mathf.Clamp(difference - currentHeart.Value, 0, _health.MaxValue);
          currentHeart.ChangeValue(currentHeart.Value + affect);
          difference = newDifference;
        }
      }
    }
  }
}