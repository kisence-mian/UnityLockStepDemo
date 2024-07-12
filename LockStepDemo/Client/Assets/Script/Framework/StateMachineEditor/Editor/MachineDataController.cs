using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MachineDataController
{
    public static MachineStateGUI AddNewMachineStateGUI(Vector2 postion, string name = null,bool isTriggerCreateEvent = true)
    {
        MachineState ms = StateMachineDataControl.AddNewMachineState();
        MachineStateGUI msg= MachineStateGUIDataControl.AddNewMachineStateGUI(ms,postion,name);
        if (isTriggerCreateEvent && StateMachineEditorWindow.OnCreateMachineStateGUI != null)
            StateMachineEditorWindow.OnCreateMachineStateGUI(msg);
        return msg;

    }
    public static void DeleteMachineStateGUI(MachineStateGUI state)
    {
       bool res =  MachineStateGUIDataControl.DeleteMachineStateGUI(state);
        if (res)
        {
            if (StateMachineEditorWindow.OnDeleteMachineStateGUI != null)
                StateMachineEditorWindow.OnDeleteMachineStateGUI(state);
        }
    }


    public static void AddNewTransitionGUI(MachineState from, MachineState to)
    {
        StateTransition st = StateTransitionDataControl.AddNewStateTransition(from, to);
        StateTransitionArrowLineDataControl.AddStateTransitionToCreateArrowLine(st);
        if (StateMachineEditorWindow.OnAddStateTransition != null)
            StateMachineEditorWindow.OnAddStateTransition(st);
    }
    public static void AddNewTransitionGUI(int fromStateID, int toStateID)
    {
        MachineState from= StateMachineDataControl.GetMachineState(fromStateID);
        MachineState to = StateMachineDataControl.GetMachineState(toStateID);
        AddNewTransitionGUI(from, to);
    }
    public static void DeleteStateTransitionArrowLine(StateTransitionArrowLine line)
    {
        bool res = StateTransitionArrowLineDataControl.DeleteStateTransitionArrowLine(line);
        if (res)
        {
            if (StateMachineEditorWindow.OnDeleteStateTransitionArrowLine != null)
                StateMachineEditorWindow.OnDeleteStateTransitionArrowLine(line);
        }
    }
    public static void AddStateBaseBehaviour(int stateID, StateBaseBehaviour behaviour)
    {
        MachineState state = StateMachineDataControl.GetMachineState(stateID);
        AddStateBaseBehaviour(state, behaviour);
    }
    public static void AddStateBaseBehaviour(MachineState state, StateBaseBehaviour behaviour)
    {
        state.stateBaseBehaviours.Add(behaviour);
    }
    public static void DeleteStateTransition(StateTransitionArrowLine line, StateTransition st)
    {
        if (line.transitions.Contains(st))
        {
            if (line.transitions.Count == 1)
            {
                StateTransitionArrowLineDataControl.DeleteStateTransitionArrowLine(line);
            }
            else
            {
                StateTransitionDataControl.DeleteStateTransition(st);
                line.transitions.Remove(st);
            }
            if (StateMachineEditorWindow.OnDeleteStateTransition != null)
                StateMachineEditorWindow.OnDeleteStateTransition(st);

        }
    }

    public static void ClearAllData()
    {
        MachineStateGUIDataControl.ClearAll();
    }

}
