using _CodeBase.IndicatorCode;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _CodeBase.UI
{
  public class BossHealthVisualizer : MonoBehaviour
  {
    [SerializeField] private GameObject _visual;
    [SerializeField] private Image _healthFill;

    private Health _currentBossHealth;
    
    public void RegisterBoss(Health health)
    {
      _visual.SetActive(true);
      _currentBossHealth = health;
      health.HealthAmountChanged += OnHealthAmountChange;
    }

    public void UnRegisterBoss(Health health)
    {
      _visual.SetActive(false);
      _currentBossHealth = null;
      health.HealthAmountChanged -= OnHealthAmountChange;
    }

    private void OnHealthAmountChange(int currentValue)
    {
      float targetAmount = Mathf.InverseLerp(0, _currentBossHealth.MaxValue, currentValue);
      _healthFill.DOKill();
      _healthFill.DOFillAmount(targetAmount, 0.1f).SetLink(gameObject);
    }
  }
}