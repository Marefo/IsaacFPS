using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _CodeBase.StateMachineCode
{
  public class StateMachine
  {
    private State _activeState;
    private readonly Dictionary<Type, State> _states = new Dictionary<Type, State>();
    private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
    private readonly List<Transition> _anyStateTransitions = new List<Transition>();
    private List<Transition> _currentStateTransitions = new List<Transition>();

    public void Update()
    {
      Transition triggeredTransition = GetTriggeredTransition();
      
      if (triggeredTransition != null)
        EnterState(triggeredTransition.To);
      
      _activeState?.Update();
    }

    public void FixedUpdate() => _activeState?.FixedUpdate();
    public void OnDrawGizmos() => _activeState?.OnDrawGizmos();

    public void EnterState(State state)
    {
      if (state == _activeState) return;
      ChangeState(state);
      ChangeActiveTransitions();
      _activeState.Enter();
    }

    public void AddStates(Dictionary<Type, State> states)
    {
      foreach (var state in states) 
        AddState(state.Key, state.Value);
    }
    
    public void AddState(Type state, State instance)
    {
      if(_states.ContainsKey(state)) return;
      _states.Add(state, instance);
    }

    public TState GetState<TState>() where TState : State => 
      _states[typeof(TState)] as TState;
    
    public void AddTransition(State from, State to, Func<bool> predicate)
    {
      bool isStateRegistered = _transitions.TryGetValue(from.GetType(), out List<Transition> transitions);
      
      if (isStateRegistered == false) 
        transitions = CreateStateTransitionList(@from);

      transitions.Add(new Transition(to, predicate));
    }

    private List<Transition> CreateStateTransitionList(State @from)
    {
      List<Transition> transitions;
      transitions = new List<Transition>();
      _transitions[@from.GetType()] = transitions;
      return transitions;
    }

    public void AddAnyTransition(State state, Func<bool> predicate) => 
      _anyStateTransitions.Add(new Transition(state, predicate));

    private Transition GetTriggeredTransition()
    {
      foreach (var transition in _anyStateTransitions)
      {
        if(transition.Condition() == false) continue;
        return transition;
      }

      foreach (var transition in _currentStateTransitions)
      {
        if(transition.Condition() == false) continue;
        return transition;
      }

      return null;
    }

    private void ChangeActiveTransitions()
    {
      _transitions.TryGetValue(_activeState.GetType(), out _currentStateTransitions);
      _currentStateTransitions ??= new List<Transition>();
    }

    private void ChangeState(State state)
    {
      _activeState?.Exit();
      _activeState = state;
    }
  }
}