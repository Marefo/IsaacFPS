using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _CodeBase.Units.Monsters
{
  public class MonsterStateMachine : MonoBehaviour
  {
    public bool IsHeroInRoomZone { get; private set; }
    public Hero Hero { get; protected set; }
    
    [SerializeField] protected Monster _monster;
    [SerializeField] protected NavMeshAgent _agent;
    
    protected StateMachine _stateMachine;

    protected virtual void OnEnable() => _monster.Initialized += OnInitialize;
    protected virtual void OnDisable() => _monster.Initialized -= OnInitialize;
    protected virtual void Update() => _stateMachine.Update();
    protected virtual void FixedUpdate() => _stateMachine.FixedUpdate();
    protected virtual void OnDrawGizmos()
    {
      if(Application.isPlaying == false) return;
      _stateMachine.OnDrawGizmos();
    }

    protected virtual void OnDestroy()
    {
      _monster.RoomZone.Entered -= OnRoomZoneEnter;
      _monster.RoomZone.Canceled -= OnRoomZoneCancel;
    }

    protected virtual void OnInitialize()
    {
      _monster.RoomZone.Entered += OnRoomZoneEnter;
      _monster.RoomZone.Canceled += OnRoomZoneCancel;
    }
    
    private void OnRoomZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      Hero = hero;
      IsHeroInRoomZone = true;
    }

    private void OnRoomZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      Hero = null;
      IsHeroInRoomZone = false;
    }
  }
}