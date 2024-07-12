using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

public  class MachineStateGUI : ScriptableObject
{

    public MachineState state;
    public Vector2 position;
    public Rect GUIRect
    {
        get
        {
            return new Rect(position,size);
        }
    }

    public MachineStateUIStateEnum uiState = MachineStateUIStateEnum.Normal;
    public Styles.Color styleColor { get {
            Styles.Color c = StateMachineUtils.GetStyleColorByState(uiState);
            if (uiState == MachineStateUIStateEnum.Normal)
                c = NormalStateColor;
            return c;
        } }

    private Styles.Color normalStateColor;
    public Styles.Color NormalStateColor
    {
        get
        {
            return normalStateColor;
        }
        set
        {
            if (value == Styles.Color.Blue || value == Styles.Color.Green)
                normalStateColor = Styles.Color.Gray;
            else
                normalStateColor = value;
        }
    }

    public StateTransitionArrowLine[] FromArrowLines { get {return StateTransitionArrowLineDataControl.GetMachineStateFromOtherStateArrowLine (state); } }
    public StateTransitionArrowLine[] ToArrowLines { get { return StateTransitionArrowLineDataControl.GetMachineStateToOtherStateArrowLine(state); } }


    


    public static Vector2 size = new Vector2(160f, 35f);
	public static void OnGUI()
    {
        foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
        {
            bool isOn = item.uiState == MachineStateUIStateEnum.Normal ? false : true;
            GUIStyle style = Styles.GetNodeStyle("node", item.styleColor, isOn);
            GUI.Box(item.GUIRect, item.name, style);
           item.position = DragLimit(item.GUIRect);
        }
    }
    private static Vector2 DragLimit(Rect rect)
    {
        float x = rect.x- StateMachineBGGUI.extraLenth*2;
        float y = rect.y - StateMachineBGGUI.extraLenth * 2;
         x = Mathf.Clamp(x, StateMachineBGGUI.bgRect.x, StateMachineBGGUI.bgRect.xMax);
         y = Mathf.Clamp(y, StateMachineBGGUI.bgRect.y, StateMachineBGGUI.bgRect.yMax);
        x += StateMachineBGGUI.extraLenth * 2;
        y += StateMachineBGGUI.extraLenth * 2;
        return new Vector2(x, y);
    }

 

   
}
