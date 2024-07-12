using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MachineStateGUI))]
public class MachineStateGUIInspector : Editor {

    private MachineStateGUI state;
    protected override void OnHeaderGUI()
    {
        if (state == null)
            state = (MachineStateGUI)target;

        if (state.state == null)
            return;

        EditorGUIUtility.SetIconSize(Vector2.one * 32);
        GUIContent cc = EditorGUIUtility.IconContent("Animation Icon");
        cc.text = "";
        GUILayout.BeginHorizontal("LargeButton");
        GUILayout.Space(5);
        GUILayout.Label(cc);
       
        GUILayout.BeginVertical();
        GUILayout.Space(2);
        GUILayout.Label("ID : " + state.state.id);
        GUILayout.Space(2);
        state.name = EditorGUILayout.TextField(state.name);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
    Dictionary<StateBaseBehaviour, object> editorInstance = new Dictionary<StateBaseBehaviour, object>();
    public override void OnInspectorGUI()
    {
        if (state.state == null)
            return;
        GUILayout.BeginVertical("box");
        foreach (var item in state.ToArrowLines)
        {
            MachineStateGUI fromState = MachineStateGUIDataControl.GetMachineStateGUI(item.transitions[0].fromState);
            MachineStateGUI toState = MachineStateGUIDataControl.GetMachineStateGUI(item.transitions[0].toState);
            GUILayout.Label(fromState.name + " -> " + toState.name, "box");
        }
        GUILayout.EndVertical();
        GUILayout.Space(5);
        for (int i = 0; i < state.state.stateBaseBehaviours.Count; i++)
        {
            StateBaseBehaviour item = state.state.stateBaseBehaviours[i];
            object eInstance = null;
            if (!editorInstance.ContainsKey(item) || editorInstance[item] == null)
            {
                eInstance = EditorExtendAttributeUtils.GetEditorExtend(typeof(StateBehaviourGUIBase), item.GetType());
                editorInstance.Add(item, eInstance);
            }
            else
            {
                eInstance = editorInstance[item];
            }
                GUILayout.BeginVertical("IN GameObjectHeader");
            GUIContent cc = EditorGUIUtility.IconContent("cs Script Icon");
            cc.text = item.GetType().Name;
            GUILayout.Label(cc);
            GUILayout.EndVertical();
            GUILayout.Space(7);
            if (eInstance == null)
            {
               item=  (StateBaseBehaviour)EditorDrawGUIUtil.DrawClassData(item);
            }
            else
            {
                StateBehaviourGUIBase temp = (StateBehaviourGUIBase)eInstance;
                temp.target = item;
                temp.stateGUI = state;

                temp.OnInspectorGUI();
            }
        }
    }
}
