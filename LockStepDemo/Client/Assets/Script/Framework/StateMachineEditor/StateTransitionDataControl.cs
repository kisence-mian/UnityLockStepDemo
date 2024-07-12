using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateTransitionDataControl  {
    private static List<StateTransition> transitions = new List<StateTransition>();

    public static StateTransition[] GetAllStateTransition()
    {
        return transitions.ToArray();
    }
    public static StateTransition AddNewStateTransition(MachineState fromState, MachineState toState)
    {
        
        if (fromState == null || toState == null)
            return null;
        StateTransition st = new StateTransition();
        st.fromState = fromState;
        st.toState = toState;
        fromState.stateTransitions.Add(st);
        transitions.Add(st);
        //Debug.Log("AddNewStateTransition :" + fromState.showContent.text + " to:" + toState.showContent.text);
        return st;
    }

    public static bool DeleteStateTransition(StateTransition st)
    {
        if (st == null || !transitions.Contains(st))
            return false;
        st.fromState.stateTransitions.Remove(st);
        transitions.Remove(st);
        return true;
    }
}
