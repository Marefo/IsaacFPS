using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _CodeBase.Infrastructure.Services
{
  public class InputService : MonoBehaviour
  {
    public event Action AttackButtonClicked;
    
    public Vector3 MovementInput => GetMovementInput();
    public Vector3 LookInput => GetLookInput();

    private InputActions _inputActions;

    private void Awake() => _inputActions = new InputActions();

    private void OnEnable()
    {
      _inputActions.Enable();
      _inputActions.Game.Attack.performed += OnAttackButtonClick;
    }

    private void OnDisable()
    {
      _inputActions.Disable();
      _inputActions.Game.Attack.performed -= OnAttackButtonClick;
    }

    private Vector3 GetMovementInput()
    {
      Vector2 input = _inputActions.Game.Movement.ReadValue<Vector2>();
      return new Vector3(input.x, 0, input.y);
    }

    private Vector3 GetLookInput() => 
      _inputActions.Game.Look.ReadValue<Vector2>();

    private void OnAttackButtonClick(InputAction.CallbackContext obj) => AttackButtonClicked?.Invoke();
  }
}