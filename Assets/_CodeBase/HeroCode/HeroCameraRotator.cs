using System;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logging;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _CodeBase.HeroCode
{
  public class HeroCameraRotator : MonoBehaviour
  {
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;
    [FormerlySerializedAs("_offset")]
    [Space(10)]
    [SerializeField] private Vector3 _offsetFromHero;
    [Space(10)]
    [SerializeField] private Transform _orientation;

    private float _rotationX;
    private float _rotationY;
    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      _offsetFromHero = transform.position - _orientation.transform.position;
      Lock();
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
      float mouseX = _inputService.LookInput.x * Time.deltaTime * _sensitivityX;
      float mouseY = _inputService.LookInput.y * Time.deltaTime * _sensitivityY;

      _rotationY += mouseX;
      _rotationX -= mouseY;
      _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

      transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
      _orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
    }

    private void Lock()
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }
}