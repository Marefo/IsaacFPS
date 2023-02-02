using _CodeBase.Logging;

namespace _CodeBase.StateMachineCode
{
  public abstract class State
  {
    public virtual void Enter()
    {
      MyDebug.Log($"Enter {GetType().Name}", MyDebug.DebugColor.green);
    }

    public virtual void Exit()
    {
      MyDebug.Log($"Exit {GetType().Name}", MyDebug.DebugColor.red);
    }
    
    public virtual void Update() {}
    public virtual void FixedUpdate() {}
    public virtual void OnDrawGizmos() {}
  }
}