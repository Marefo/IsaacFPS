using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _CodeBase.Infrastructure.Services
{
  public class InputService : MonoBehaviour
  {
    public event Action AttackButtonClicked;
    public event Action ThrowGrenadeButtonClicked;
    public event Action MenuButtonClicked;
    
    public Vector3 MovementInput => GetMovementInput();
    public Vector3 LookInput => GetLookInput();
    public bool Enabled { get; private set; } = true;

    private InputActions _inputActions;
    private bool _cameraRotationEnabled = true;

    private void Awake() => _inputActions = new InputActions();

    private void OnEnable()
    {
      _inputActions.Enable();
      _inputActions.Game.Attack.performed += OnAttackButtonClick;
      _inputActions.Game.ThrowGrenade.performed += OnThrowGrenadeButtonClick;
      _inputActions.Game.Menu.performed += OnMenuButtonClick;
      _inputActions.Game.ChangeCameraRotationState.performed += OnChangeCameraRotationState;
    }

    private void OnDisable()
    {
      _inputActions.Disable();
      _inputActions.Game.Attack.performed -= OnAttackButtonClick;
      _inputActions.Game.ThrowGrenade.performed -= OnThrowGrenadeButtonClick;
      _inputActions.Game.Menu.performed -= OnMenuButtonClick;
      _inputActions.Game.ChangeCameraRotationState.performed -= OnChangeCameraRotationState;
    }

    public void Enable() => Enabled = true;
    public void Disable() => Enabled = false;
    
    private Vector3 GetMovementInput()
    {
      Vector2 input = _inputActions.Game.Movement.ReadValue<Vector2>();
      return Enabled ? new Vector3(input.x, 0, input.y) : Vector3.zero;
    }

    private Vector3 GetLookInput()
    {
      return Enabled && _cameraRotationEnabled ? _inputActions.Game.Look.ReadValue<Vector2>() : Vector3.zero;
    }

    private void OnMenuButtonClick(InputAction.CallbackContext obj) => MenuButtonClicked?.Invoke();

    private void OnAttackButtonClick(InputAction.CallbackContext obj)
    {
      if(Enabled == false) return;
      AttackButtonClicked?.Invoke();
    }

    private void OnThrowGrenadeButtonClick(InputAction.CallbackContext obj)
    {
      if(Enabled == false) return;
      ThrowGrenadeButtonClicked?.Invoke();
    }

    private void OnChangeCameraRotationState(InputAction.CallbackContext obj) => 
      _cameraRotationEnabled = !_cameraRotationEnabled;
  }
}