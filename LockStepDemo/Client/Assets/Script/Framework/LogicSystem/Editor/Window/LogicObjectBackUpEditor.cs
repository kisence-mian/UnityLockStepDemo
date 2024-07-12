using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HDJ.Framework.Utils;
public class LogicObjectBackUpEditor 
{
    private static LogicObjectBackUpEditor instance;

    public static LogicObjectBackUpEditor Instance
    {
        get {
            if (instance == null)
            {
                instance = new LogicObjectBackUpEditor();
                instance.Init();
            }
            return LogicObjectBackUpEditor.instance; }
        
    }
    public const string pathAssetsPath = "Assets/Script/Framework/LogicSystem/Editor/Config/LogicObjectBackUpData.asset";
    MessageStringData msData;
    void Init()
    {
        msData = ScriptableObjectUtils.LoadCreateScriptableObject<MessageStringData>(pathAssetsPath);
    }

    private Vector2 scrollPos = Vector2.zero;
   public void BackupGUI()
    {
        if (msData == null) return;
        GUILayout.Space(6);
        scrollPos = GUILayout.BeginScrollView(scrollPos, true, true);
        foreach (MessageString ms in msData.mesList)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("名字：" + ms.name);
            if (GUILayout.Button("移除", GUILayout.Width(70)))
            {
                msData.mesList.Remove(ms);
                return;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            if (GUILayout.Button("添加到工程", GUILayout.Width(120)))
            {
                CreateItem(ms);
            }
            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    void CreateItem(MessageString ms)
    {
        LogicObject lo = JsonUtils.JsonToClassOrStruct<LogicObject>(ms.value);

        MachineStateGUI msg= MachineDataController.AddNewMachineStateGUI(new Vector2(500, 450), lo.name, false);
        LogicObjectBehaviour be = new LogicObjectBehaviour();
        be.logicObj = lo;
        msg.state.stateBaseBehaviours.Add(be);
        msg.name = lo.name;
        LogicSystemEditorWindow.AddLogicObject(lo);

    }

    public void SaveData(string name, LogicObject lo)
    {
        MessageString ms = new MessageString(name, JsonUtils.ClassOrStructToJson(lo));
        msData.mesList.Add(ms);
        EditorUtility.SetDirty(msData);
        
    }
}
