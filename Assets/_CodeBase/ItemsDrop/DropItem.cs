using System;
using _CodeBase.ItemsDrop.Data;
using UnityEngine;

namespace _CodeBase.ItemsDrop
{
  public class DropItem : MonoBehaviour
  {
    public event Action Pushed;
    
    [SerializeField] private Rigidbody _rigidbody;
    [Space(10)] 
    [SerializeField] private DropItemSettings _settings;

    public void Push(Vector3 direction)
    {
      _rigidbody.AddForce(direction * _settings.PushForce, ForceMode.Impulse);
      Pushed?.Invoke();
    }
  }
}