using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class StateMachineEditorGUI  {

    public static Rect stateMachineMaxRange =Rect.zero;
    public static void DrawAllStateMachineGUI()
    {
        stateMachineMaxRange = Rect.zero;
        foreach (var ms in MachineStateGUIDataControl.allMachineStateGUI)
        {
            Rect range = ms.GUIRect;
            stateMachineMaxRange.xMin = stateMachineMaxRange.xMin > range.xMin ? range.xMin : stateMachineMaxRange.xMin;
            stateMachineMaxRange.xMax = stateMachineMaxRange.xMax < range.xMax ? range.xMax : stateMachineMaxRange.xMax;
            stateMachineMaxRange.yMin = stateMachineMaxRange.yMin > range.yMin ? range.yMin : stateMachineMaxRange.yMin;
            stateMachineMaxRange.yMax = stateMachineMaxRange.yMax < range.yMax ? range.yMax : stateMachineMaxRange.yMax;
        }


            StateTransitionGUI.OnGUI();
        
        MachineStateGUI.OnGUI();
    }

    public static void HandleColorChangeEnd()
    {
        Handles.color = new Color(1, 1, 1, 1);
    }




}
