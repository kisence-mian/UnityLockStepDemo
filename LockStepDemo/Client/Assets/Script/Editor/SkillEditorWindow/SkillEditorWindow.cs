using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SkillEditorWindow : EditorWindow {

	[MenuItem("Tools/技能表编辑窗口")]
    static void OpenWindow()
    {
        GetWindow<SkillEditorWindow>();

    }

    ConfigEditorUserData<SkillDataGenerate> skillDataConfig = new ConfigEditorUserData<SkillDataGenerate>();
    ConfigEditorUserData<SkillStatusDataGenerate> skillStatuDataConfig = new ConfigEditorUserData<SkillStatusDataGenerate>();
    private void OnEnable()
    {
        skillDataConfig. configDataDic = DataGenerateManager<SkillDataGenerate>.GetAllData();
        skillDataConfig. keys = new List<string>(skillDataConfig.configDataDic.Keys);
        skillDataConfig.dataTable = DataGenerateManager<SkillDataGenerate>.GetDataTable();

        skillStatuDataConfig.configDataDic = DataGenerateManager<SkillStatusDataGenerate>.GetAllData();
        skillStatuDataConfig.keys = new List<string>(skillStatuDataConfig.configDataDic.Keys);
        skillStatuDataConfig.dataTable = DataGenerateManager<SkillStatusDataGenerate>.GetDataTable();

    }

    private int toolbarOption = 0;
    private string[] toolbarTexts = { "SkillData", "SkillStatusData" };

    private string selectKey = "";
    private Vector2 pos = Vector2.zero;
    private void OnGUI()
    {
        toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts, GUILayout.Width(Screen.width));
        switch (toolbarOption)
        {
            case 0:
                DrawConfigGUI(skillDataConfig);
                break;
            case 1:
                DrawConfigGUI(skillStatuDataConfig);
                break;
        }

        if (GUILayout.Button("Save"))
        {
            skillDataConfig.UpdateChangeDataToDataTable();
            DataEditorWindow.SaveData(skillDataConfig.dataTable.m_tableName, skillDataConfig.dataTable);
            skillStatuDataConfig.UpdateChangeDataToDataTable();
            DataEditorWindow.SaveData(skillStatuDataConfig.dataTable.m_tableName, skillStatuDataConfig.dataTable);
            // MemoryManager.FreeHeapMemory();
            AssetDatabase.Refresh();
            MemoryManager.FreeHeapMemory();
            OnEnable();
            Debug.Log("保存成功");
        }
        //if (GUILayout.Button("Clear"))
        //{
        //    MemoryManager.FreeHeapMemory();
        //}
        //if (GUILayout.Button("Re"))
        //{
        //    OnEnable();
        //    Debug.Log(" skillDataConfig. configDataDic:" + skillDataConfig.configDataDic.Count);
        //    Debug.Log(" skillDataConfig. keys:" + skillDataConfig.keys.Count);
        //    Debug.Log(" skillDataConfig.dataTable:" + skillDataConfig.dataTable.Count);
        //}

        //if (GUILayout.Button("Test"))
        //{
        //    DataTable dt =  DataManager.GetData("skillData");

        //    Debug.Log(dt.TableIDs.Count);
        //}
    }

    private void DrawConfigGUI<T>(ConfigEditorUserData<T> conf) where T : DataGenerateBase, new()
    {
        selectKey = EditorDrawGUIUtil.DrawPopup("key", selectKey, conf.keys);

        pos = GUILayout.BeginScrollView(pos);
        T d = null;
        conf.configDataDic.TryGetValue(selectKey, out d);
        if (d != null)
        {
            EditorDrawGUIUtil.DrawClassData(d, null, (f) =>
            {
                string s;
                string k = f.Name.Replace("m_", "");
                conf.dataTable.m_noteValue.TryGetValue(k, out s);
                if (!string.IsNullOrEmpty(s))
                    EditorGUILayout.HelpBox(s, UnityEditor.MessageType.Warning);
            });
        }

        GUILayout.EndScrollView();
    }
}

public class ConfigEditorUserData<T> where T : DataGenerateBase, new()
{
    public List<string> keys = new List<string>();
    public Dictionary<string, T> configDataDic = new Dictionary<string, T>();

    public DataTable dataTable = new DataTable();


    public void UpdateChangeDataToDataTable()
    {
        Type t = typeof(T);
        FieldInfo[] fs = t.GetFields();

        foreach (var id in configDataDic.Keys)
        {
            if (dataTable.ContainsKey(id))
            {
                T value = configDataDic[id];
                SingleData sd = dataTable[id];
                foreach (var f in fs)
                {
                    string name = f.Name.Replace("m_", "");
                   
                        object o = f.GetValue(value);
                        string v = ConfigTool.Value2TableStringValue(o);
                  //  Debug.Log("key :" + name + "   Pre: " +( sd.ContainsKey(name)? sd[name]:"") + "   value: " + v);
                    if (sd.ContainsKey(name))
                        sd[name] = v;
                    else
                        sd.Add(name, v);
                        
                    
                }
            }
        }
    }
}
