using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachineDataControl {

    static int internalID = 0;
    static int GetNewID { get { return internalID++; } }
    private static Dictionary<int, MachineState> allMachineState = new Dictionary<int, MachineState>();
   

    public static MachineState[] GetAllMachineState()
    {
        List<MachineState> list = new List<MachineState>(allMachineState.Values);
        return list.ToArray();
    }
    public static MachineState GetMachineState(int id)
    {
        MachineState ms = null;
        allMachineState.TryGetValue(id, out ms);
        return ms;
    }


    public static bool ContainsMachineState(MachineState ms)
    {
        if (ms == null)
            return false;
        return allMachineState.ContainsKey(ms.id);
    }

    public static MachineState AddNewMachineState()
    {
        int id = GetNewID;
        MachineState ms =new MachineState();
        ms.id = id;
        allMachineState.Add(id, ms);
        return ms;
    }
    public static bool DeleteMachineState(MachineState ms)
    {
        if (ms == null)
            return false;
        StateTransition[] transitions = StateTransitionDataControl.GetAllStateTransition();
        List<StateTransition> removeList = new List<StateTransition>();
        for (int i = 0; i < transitions.Length; i++)
        {
            StateTransition st = transitions[i];
            if(st.fromState ==ms || st.toState == ms)
            {
                removeList.Add(st);
            }
        }

        for (int i = 0; i < removeList.Count; i++)
        {
            StateTransitionDataControl.DeleteStateTransition(removeList[i]);
        }
        allMachineState.Remove(ms.id);
        return true;
    }



}
