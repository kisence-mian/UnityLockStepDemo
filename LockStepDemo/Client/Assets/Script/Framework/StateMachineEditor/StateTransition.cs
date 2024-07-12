using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StateTransition 
{
    public MachineState fromState;
    public MachineState toState;

    public StateTransition() { }
    public StateTransition(MachineState fromState, MachineState toState)
    {
        this.fromState = fromState;
        this.toState = toState;
    }
}
