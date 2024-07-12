using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Profiling;

public class StateMachineEditorWindow : EditorWindow
{
    public static CallBack OnDrawLeftPartGUI;
    public static CallBack OnDrawTopToolBarGUI;
    public static CallBack<MachineStateGUI> OnCreateMachineStateGUI;
    public static CallBack<MachineStateGUI> OnDeleteMachineStateGUI;
    public static CallBack<StateTransition> OnAddStateTransition;
    public static CallBack<StateTransition> OnDeleteStateTransition;
    public static CallBack<StateTransitionArrowLine> OnDeleteStateTransitionArrowLine;
    public static CallBack OnDestroyWindow;

    public static StateMachineEditorWindow Instance;
    [MenuItem("Tool/StateMachineEditorWindow")]
    public static void OpenWindow()
    {
        if (Instance == null)
        {
            Instance = GetWindow<StateMachineEditorWindow>();
            Instance.Show();
            Instance.Focus(); 
        }
        Instance.OnDestroy();
    }
    void OnEnable()
    {
        Instance = this;
    }
    Rect ndoeControlRange;
    void OnGUI()
    {
        MachineStateInputEventController.PlayerControlUse();
        DrawLeftToolAreaGUI();
        DrawToolBarGUI();
        ndoeControlRange = new Rect(leftToolAreaRect.xMax, topRightToolBarRect.yMax, Screen.width - leftToolAreaRect.width, Screen.height - topRightToolBarRect.height);
      
        StateMachineBGGUI.BeginGUI(ndoeControlRange, StateMachineEditorGUI.stateMachineMaxRange);
        MachineStateInputEventController.OnMachineStateMouseRightClickMenu();
        BeginWindows();
        StateTransitionGUI.DrawTempArrowTransition();
        StateMachineEditorGUI.DrawAllStateMachineGUI();
             
        EndWindows();
        StateMachineBGGUI.EndGUI();
    }

    public Rect leftToolAreaRect;
    private float leftToolAreaRect_with = 350;
    public float LeftToolAreaRect_with {
        get
        {
            return leftToolAreaRect_with;
        }

        set
        {
            leftToolAreaRect_with = Mathf.Clamp(value, 300, Screen.width + 300);
        }
    }
    void DrawLeftToolAreaGUI()
    {
        leftToolAreaRect = new Rect(0, 0, leftToolAreaRect_with, Screen.height);

        GUILayout.BeginArea(leftToolAreaRect,Styles.graphBackground);
        if (OnDrawLeftPartGUI != null)
            OnDrawLeftPartGUI();
        GUILayout.EndArea();
      
    }

    public Rect topRightToolBarRect;
    void DrawToolBarGUI()
    {
        topRightToolBarRect = new Rect(leftToolAreaRect.xMax, 0, Screen.width - leftToolAreaRect.width, 25f);

        GUIStyle style = "flow overlay header lower left";
        GUILayout.BeginArea(topRightToolBarRect, style);
        GUILayout.BeginHorizontal(style);

        if (OnDrawTopToolBarGUI != null)
            OnDrawTopToolBarGUI();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void OnDestroy()
    {
        OnDrawLeftPartGUI = null;
        OnDrawTopToolBarGUI = null;
        OnCreateMachineStateGUI = null;
        OnDeleteMachineStateGUI = null;
        OnAddStateTransition = null;
        OnDeleteStateTransition = null;
        OnDeleteStateTransitionArrowLine = null;
        MachineDataController.ClearAllData();

        if (OnDestroyWindow != null)
            OnDestroyWindow();
    }
}
