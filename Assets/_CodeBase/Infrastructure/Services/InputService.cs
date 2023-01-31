using UnityEngine;

namespace _CodeBase.Infrastructure.Services
{
  public class InputService : MonoBehaviour
  {
    public Vector3 MovementInput => GetMovementInput();
    public Vector3 LookInput => GetLookInput();

    private InputActions _inputActions;

    private void Awake() => _inputActions = new InputActions();

    private void OnEnable() => _inputActions.Enable();

    private void OnDisable() => _inputActions.Disable();

    private Vector3 GetMovementInput()
    {
      Vector2 input = _inputActions.Game.Movement.ReadValue<Vector2>();
      return new Vector3(input.x, 0, input.y);
    }

    private Vector3 GetLookInput() => 
      _inputActions.Game.Look.ReadValue<Vector2>();
  }
}