using System;
using _CodeBase.HeroCode.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _CodeBase.UI
{
  public class SensitivitySlider : MonoBehaviour
  {
    [SerializeField] private Slider _slider;
    [Space(10)]
    [SerializeField] private HeroCameraSettings _cameraSettings;

    private void OnEnable() => _slider.onValueChanged.AddListener(OnSliderValueChange);
    private void OnDisable() => _slider.onValueChanged.RemoveListener(OnSliderValueChange);

    private void Start() => InitializeSlider();

    private void OnSliderValueChange(float newValue)
    {
      _cameraSettings.Sensitivity = Mathf.Lerp(_cameraSettings.MinMaxSensitivity.Min,
        _cameraSettings.MinMaxSensitivity.Max, newValue);
    }

    private void InitializeSlider()
    {
      _slider.value = Mathf.InverseLerp(_cameraSettings.MinMaxSensitivity.Min,
        _cameraSettings.MinMaxSensitivity.Max, _cameraSettings.Sensitivity);
    }
  }
}