using System;
using _CodeBase.IndicatorCode.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.IndicatorCode
{
  public abstract class Indicator : MonoBehaviour
  {
    public event Action Changed;
    public event Action ValueCameToZero;
    public event Action ValueCameToMax;

    public int MaxValue { get; private set; }
    public int CurrentValue { get; private set; }
    public bool IsMaxValue => CurrentValue == MaxValue;
    public bool IsValueZero => CurrentValue == 0;

    [SerializeField] private IndicatorSettings _settings;

    private void Start() => Initialize();

    [Button()]
    public void IncreaseForTen() => ChangeValue(10);
    
    [Button()]
    public void DecreaseForTen() => ChangeValue(-10);

    public void Increase(int value) => ChangeValue(Mathf.Abs(value));
    public void Decrease(int value) => ChangeValue(-Mathf.Abs(value));
    
    public void ChangeToZero()
    {
      CurrentValue = 0;
      Changed?.Invoke();
      ValueCameToZero?.Invoke();
    }

    private void ChangeValue(int value)
    {
      CurrentValue = Mathf.Clamp(CurrentValue + value, 0, MaxValue);
      Changed?.Invoke();
      
      if(CurrentValue == 0)
        ValueCameToZero?.Invoke();
      else if(CurrentValue == MaxValue)
        ValueCameToMax?.Invoke();
    }

    private void Initialize()
    {
      MaxValue = _settings.MaxValue;
      CurrentValue = _settings.StartValue;
      Changed?.Invoke();
    }
  }
}