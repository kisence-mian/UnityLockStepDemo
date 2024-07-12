using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StateTransitionArrowLine))]
public class StateTransitionArrowLineInspector : Editor {

    StateTransitionArrowLine line;
    protected override void OnHeaderGUI()
    {
        if (line == null)
            line = (StateTransitionArrowLine)target;
        if (line.transitions.Count == 0)
            return;
        EditorGUIUtility.SetIconSize(Vector2.one * 32);
        GUIContent cc = EditorGUIUtility.IconContent("d_UnityEditor.VersionControl");
        cc.text = "";
        GUILayout.BeginHorizontal("LargeButton");
        GUILayout.Space(5);
        GUILayout.Label(cc);

        GUILayout.BeginVertical();
        GUILayout.Space(2);
        MachineStateGUI mag0 = MachineStateGUIDataControl.GetMachineStateGUI(line.transitions[0].fromState);
        MachineStateGUI mag1 = MachineStateGUIDataControl.GetMachineStateGUI(line.transitions[0].toState);
        GUILayout.Label(mag0.name+ " -> " + mag1.name);
        GUILayout.Space(2);
        GUILayout.Label(line.transitions.Count + " Transitions");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        GUILayout.BeginVertical("box");
        for (int i = 0; i < line.transitions.Count; i++)
        {
            StateTransition st = line.transitions[i];
            MachineStateGUI mag0 = MachineStateGUIDataControl.GetMachineStateGUI(st.fromState);
            MachineStateGUI mag1 = MachineStateGUIDataControl.GetMachineStateGUI(st.toState);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(mag0.name + " -> " + mag1.name);
            if (line.transitions.Count>1 && GUILayout.Button("-"))
            {
                MachineDataController.DeleteStateTransition(line, st);
                return;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
