using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Points
{
  public class Point : MonoBehaviour
  {
    public event Action PositionChanged;
    public event Action<Point> Released;
		
    public bool Available { get; protected set; } = true;
    public Vector3 Position => transform.position;
    public Quaternion TargetQuaternionRotation { get; private set; }

    [field: SerializeField] public bool HasTargetRotation;
    [field: SerializeField, ShowIf("HasTargetRotation")] public Vector3 TargetRotation;
		
    public virtual void Take() => Available = false;

    public virtual void SetTargetRotation(Quaternion targetRotation) => 
      TargetQuaternionRotation = targetRotation;

    public virtual void Release()
    {
      Available = true;
      Released?.Invoke(this);
    }

    public void OnPositionChange() => PositionChanged?.Invoke();
  }
}