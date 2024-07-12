using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MachineStateGUIDataControl  {

    public static List<MachineStateGUI> allMachineStateGUI = new List<MachineStateGUI>();

    public static MachineStateGUI AddNewMachineStateGUI(MachineState state, Vector2 pos,string name)
    {
        MachineStateGUI ms = ScriptableObject.CreateInstance<MachineStateGUI>();
        ms.state = state;
        ms.position = pos;
        ms.name = name;
        if (string.IsNullOrEmpty(name))
        {
            ms.name = "New State " + state.id;
        }
        allMachineStateGUI.Add(ms);
        return ms;
    }
    public static bool DeleteMachineStateGUI(MachineStateGUI state)
    {
        if (allMachineStateGUI.Contains(state))
        {
            List<StateTransitionArrowLine> lineList = new List<StateTransitionArrowLine>();
            lineList.AddRange(state.FromArrowLines);
            lineList.AddRange(state.ToArrowLines);
            foreach (var item in lineList)
            {
                StateTransitionArrowLineDataControl.DeleteStateTransitionArrowLine(item);
            }
            lineList.Clear();
            StateMachineDataControl.DeleteMachineState(state.state);
            allMachineStateGUI.Remove(state);
            return true;
        }
        return false;
    }
    public static Rect GetMachineStateGUIRect(MachineState state)
    {
        foreach (var item in allMachineStateGUI)
        {
            if (item.state == state)
                return item.GUIRect;
        }

        return Rect.zero;
    }
    public static MachineStateGUI GetMachineStateGUI(MachineState state)
    {
        foreach (var item in allMachineStateGUI)
        {
            if (item.state == state)
                return item;
        }

        return null;
    }

    public static void ClearAll()
    {
        MachineStateGUI[] AllData = allMachineStateGUI.ToArray();
        foreach (var item in AllData)
        {
            DeleteMachineStateGUI(item);
        }
    }
}
