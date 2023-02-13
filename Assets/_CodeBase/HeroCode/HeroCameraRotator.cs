using System;
using _CodeBase.Etc;
using _CodeBase.HeroCode.Data;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logging;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _CodeBase.HeroCode
{
  public class HeroCameraRotator : MonoBehaviour
  {
    [SerializeField] private Transform _orientation;
    [Space(10)]
    [SerializeField] private HeroCameraSettings _settings;

    private float _rotationX;
    private float _rotationY;
    private Vector3 _offsetFromHero;
    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      _offsetFromHero = transform.position - _orientation.transform.position;
      CursorVisibilityController.Hide();
    }

    private void Update() => FollowHero();

    private void LateUpdate() => Look();

    private void FollowHero()
    {
      Vector3 targetPosition = _orientation.transform.position + _offsetFromHero;
      targetPosition.y = transform.position.y;
      transform.position = targetPosition;
    }

    private void Look()
    {
      float mouseX = _inputService.LookInput.x * Time.deltaTime * _settings.Sensitivity;
      float mouseY = _inputService.LookInput.y * Time.deltaTime * _settings.Sensitivity;

      _rotationY += mouseX;
      _rotationX -= mouseY;
      _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

      transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
      _orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
    }
  }
}