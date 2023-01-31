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
    [FormerlySerializedAs("orientation")]
    [Space(10)]
    [SerializeField] private Transform _orientation;

    private float _rotationX;
    private float _rotationY;
    private Vector3 _offset;
    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      _offset = transform.position - _orientation.transform.position;
      Lock();
    }

    private void Update()
    {
      transform.position = _orientation.transform.position + _offset;
    }

    private void LateUpdate()
    {
      Look();
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