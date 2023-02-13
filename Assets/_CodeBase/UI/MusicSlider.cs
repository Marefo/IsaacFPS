using _CodeBase.Audio.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _CodeBase.UI
{
  public class MusicSlider : MonoBehaviour
  {
    [SerializeField] private Slider _slider;
    [Space(10)] 
    [SerializeField] private AudioData _audioData;

    private void OnEnable() => _slider.onValueChanged.AddListener(OnSliderValueChange);
    private void OnDisable() => _slider.onValueChanged.RemoveListener(OnSliderValueChange);

    private void Start() => InitializeSlider();

    private void OnSliderValueChange(float newValue) => _audioData.SetMusicVolume(newValue);

    private void InitializeSlider() => _slider.value = _audioData.MusicVolume;
  }
}