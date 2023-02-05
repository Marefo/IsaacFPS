using _CodeBase.Data;
using _CodeBase.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Etc
{
  public class AgentRandomHeightSetter : MonoBehaviour
  {
    [SerializeField] private Range _height;
    [Space(10)]
    [SerializeField] private NavMeshAgent _agent;
    
    private void Start() => SetUpHeight();

    private void SetUpHeight()
    {
      _agent.enabled = false;
      float height = _height.GetRandomValue();
      _agent.height = height;
      _agent.baseOffset = height / 2;
      transform.position += Vector3.up * _agent.baseOffset;
      _agent.enabled = true;
    }
  }
}