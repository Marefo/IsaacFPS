using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _CodeBase.Infrastructure.Services
{
  public class InputService : MonoBehaviour
  {
    public event Action AttackButtonClicked;
    public event Action ThrowGrenadeButtonClicked;
    
    public Vector3 MovementInput => GetMovementInput();
    public Vector3 LookInput => GetLookInput();

    private InputActions _inputActions;
    private bool _cameraRotationEnabled = true;

    private void Awake() => _inputActions = new InputActions();

    private void OnEnable()
    {
      _inputActions.Enable();
      _inputActions.Game.Attack.performed += OnAttackButtonClick;
      _inputActions.Game.ThrowGrenade.performed += OnThrowGrenadeButtonClick;
      _inputActions.Game.ChangeCameraRotationState.performed += OnChangeCameraRotationState;
    }

    private void OnDisable()
    {
      _inputActions.Disable();
      _inputActions.Game.Attack.performed -= OnAttackButtonClick;
      _inputActions.Game.ThrowGrenade.performed -= OnThrowGrenadeButtonClick;
      _inputActions.Game.ChangeCameraRotationState.performed -= OnChangeCameraRotationState;
    }

    private Vector3 GetMovementInput()
    {
      Vector2 input = _inputActions.Game.Movement.ReadValue<Vector2>();
      return new Vector3(input.x, 0, input.y);
    }

    private Vector3 GetLookInput()
    {
      return _cameraRotationEnabled ? _inputActions.Game.Look.ReadValue<Vector2>() : Vector3.zero;
    }

    private void OnAttackButtonClick(InputAction.CallbackContext obj) => AttackButtonClicked?.Invoke();

    private void OnThrowGrenadeButtonClick(InputAction.CallbackContext obj) => ThrowGrenadeButtonClicked?.Invoke();

    private void OnChangeCameraRotationState(InputAction.CallbackContext obj) => 
      _cameraRotationEnabled = !_cameraRotationEnabled;
  }
}