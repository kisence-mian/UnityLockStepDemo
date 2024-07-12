using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class LogicSystemEditorWindow 
{

    public static void ShowWindow(string logicDataName,string logicFileUseType)
    {
        StateMachineEditorWindow.OpenWindow();
        LogicSystemEditorWindow.logicDataName = logicDataName;
        LogicSystemEditorWindow.logicFileUseType = logicFileUseType;
        LogicSystemEditorWindow.Init();

    }
    private static string logicDataName = "";
    public static string logicFileUseType = "";

    public static LogicObjectContainer data;
    /// <summary>
    /// 设置内部变量的值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetInternalValue(string name, object value)
    {
        BaseValue bv = data.GetBaseValue(name);
        if (bv != null)
        {            
            bv.SetValue(value);
        }
    }
    /// <summary>
    /// 获取内部变量的值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static object GetInternalValue(string name)
    {
        BaseValue bv = data.GetBaseValue(name);
        if (bv != null)
            return bv.GetValue();
        else
        {
            Debug.LogError("不存在内部变量：" + name);
            return null;
        }
    }

    //public EditorWindow animatorWindow;
    static void Init()
    {
        data = LogicObjectDataController.GetDataFromFile(logicDataName);
        if (data.logicObjs.Count == 0)
        {
            LogicObject r = CreateLogicObject("开始",new Vector2(500,400));
            data.startId = r.id;
        }

        Dictionary<int, MachineStateGUI> allState = new Dictionary<int, MachineStateGUI>();
        foreach (var item in data.logicObjs)
        {
            MachineStateGUI msg=  MachineDataController.AddNewMachineStateGUI(item.editorPos, item.name);
            LogicObjectBehaviour be = new LogicObjectBehaviour();
            msg.state.stateBaseBehaviours.Add(be);
            be.state = msg.state;
            be.logicObj = item;
            bool isStartModel = LogicSystemEditorWindow.data.startId == item.id;
          msg.NormalStateColor = isStartModel ? UnityEditor.Graphs.Styles.Color.Yellow : UnityEditor.Graphs.Styles.Color.Gray;
            if (!isStartModel && item.isSupportAlwaysActive)
                msg.NormalStateColor = UnityEditor.Graphs.Styles.Color.Red;
            allState.Add(item.id, msg);
        }
        foreach (var item in data.logicObjs)
        {
            foreach (var child in item.childObjects)
            {
                MachineDataController.AddNewTransitionGUI(allState[item.id].state, allState[child].state);
            }
               
        }

        StateMachineEditorWindow.OnDrawLeftPartGUI = OnGUI;
        StateMachineEditorWindow.OnCreateMachineStateGUI = CreateNewModel;

        internalValueTypeList.Add(typeof(string).FullName);
        internalValueTypeList.Add(typeof(int).FullName);
        internalValueTypeList.Add(typeof(float).FullName);
        internalValueTypeList.Add(typeof(Vector2).FullName);
        internalValueTypeList.Add(typeof(Vector3).FullName);
        internalValueTypeList.Add(typeof(bool).FullName);
    }
    static List<string> internalValueTypeList = new List<string>();
    static LogicObject CreateLogicObject(string name,Vector2 pos)
     {
         LogicObject r = new LogicObject();
         r.name = name;
         r.id = GetMaxID(data.logicObjs);
         r.editorPos = pos;
         data.logicObjs.Add(r);
        return r;
     }
    public static void AddLogicObject(LogicObject r)
    {
        r.id = GetMaxID(data.logicObjs);
        data.logicObjs.Add(r);
    }

     public static int toolbarOption = 0;
    static public string[] toolbarTexts = { "内部变量", "备份组件" };
    static void OnGUI()
    {
        //titleContent.text = "逻辑系统编辑器";
        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Logic File Name：" + logicDataName);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(2);

        toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts);
        switch (toolbarOption)
        {
            case 0:
                InternalVariableGUI();
                break;
            case 1:
                LogicObjectBackUpEditor.Instance.BackupGUI();
             
                break;
          
        }       
  
        GUILayout.Space(7);
        if (GUILayout.Button("保存"))
        {
            UpdateStateDataInfo();
            LogicObjectDataController.SaveData(logicDataName,logicFileUseType, data);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "保存成功", "OK");
        }
        GUILayout.Space(25);
       
    }
    static public void CreateNewModel(MachineStateGUI msg)
    {
        LogicObject r = CreateLogicObject("新建逻辑模块", msg.position);
        LogicObjectBehaviour be = new LogicObjectBehaviour();
        be.logicObj = r;
        msg.state.stateBaseBehaviours.Add(be);
        msg.name = r.name;
        
    }
    static public int GetMaxID(List<LogicObject> objs)
  {
        if (objs.Count == 0)
        {
           return 0;
        }
        else
        {
            int max = 0;
            foreach (LogicObject o in objs)
            {
                int k = o.id;
                if (k > max)
                    max = k;
            }
           return max + 1;
        }

    }
  static  private Vector2 pos = Vector2.zero;
    static private string internalValueName = "";
    static private string internalValueType = "";
    static void InternalVariableGUI()
    {
        GUIStyle style0 = "IN GameObjectHeader";
        style0.fontSize = 12;
        GUILayout.Label("新建内部变量", style0);
        //GUIStyle style = "flow overlay header lower left";
       
        GUILayout.BeginHorizontal("box");
        GUILayout.BeginVertical();
         internalValueName = EditorDrawGUIUtil.DrawBaseValue ("名字", internalValueName).ToString();
          internalValueType= EditorDrawGUIUtil.DrawPopup("数据类型", internalValueType, internalValueTypeList);
        GUILayout.EndVertical();
          if (GUILayout.Button("+", GUILayout.Width(35)))
          {
              if (!string.IsNullOrEmpty(internalValueName) && data.GetBaseValue(internalValueName) == null)
              {
                  data.internalValueList.Add(new BaseValue(internalValueName, LogicSystemEditorTools.GetDefultValueByTypeName(internalValueType)));
              }
              else
              {
                  EditorUtility.DisplayDialog("错误", "名字不能重复！", "OK");
              }
          }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        pos = GUILayout.BeginScrollView(pos, true, true);
        GUILayout.BeginVertical("box");
      
         
        foreach (BaseValue vt in data.internalValueList)
        {
            GUILayout.BeginHorizontal("box");
            GUILayout.BeginVertical();
            vt.name = EditorDrawGUIUtil.DrawBaseValue ("名字("+vt.typeName+")", vt.name).ToString();
            object v = EditorDrawGUIUtil.DrawBaseValue("Value",vt.GetValue());
            vt.SetValue(v);
            GUILayout.EndVertical();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                data.internalValueList.Remove(vt);
                return;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

 
    static public void UpdateStateDataInfo()
    {
        if(data!=null && StateMachineEditorWindow.Instance != null)
        {
            foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
            {
                LogicObjectBehaviour be = (LogicObjectBehaviour)item.state.stateBaseBehaviours[0];
                be.logicObj.name = item.name;
                be.logicObj.editorPos = item.position;
                List<int> childList = new List<int>();
                foreach (StateTransition st in item.state.stateTransitions)
                {
                    LogicObjectBehaviour be1 = (LogicObjectBehaviour)st.toState.stateBaseBehaviours[0];
                    childList.Add(be1.logicObj.id);
                }
                be.logicObj.childObjects = childList;

            }
        }
    }


    }
