using HDJ.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SelectLogicObjectEditor : EditorWindow {
  public static  SelectLogicObjectEditor instance;
    public static void GetWindowNew(LogicComponentType componentType, List<string> dataList)
    {
        instance = EditorWindow.GetWindow<SelectLogicObjectEditor>();
        instance.position = new Rect(Screen.width / 2, Screen.height / 2, 500, 500);
        instance.dataList = dataList;
        instance.isNew = true;
        instance.Init(componentType, "");

    }

    public static void GetWindowEdit(LogicComponentType componentType,List<string> dataList,int indexEdit, string componetPathName)
    {
         instance =EditorWindow.GetWindow<SelectLogicObjectEditor>();
        instance.position = new Rect(Screen.width/2,Screen.height/2,500,500);
        instance.indexEdit = indexEdit;
        instance.dataList = dataList;
        instance.isNew = false;
        instance.Init(componentType, componetPathName);
        
    }
 
    LogicComponentType logicComponetType;
    bool isNew = false;
    ClassValue nc;
   List< string> dataList;
    int indexEdit;
    void Init(LogicComponentType componentType, string componetPathName)
    {
        logicComponetType = componentType;
        string logicFileUseType = "";
        if (StateMachineEditorWindow.Instance)
            logicFileUseType = LogicSystemEditorWindow.logicFileUseType;
        componetNameArr = CompontNameAttributeUtils.GetCompontNameAttributeArray(componentType, logicFileUseType);

        if (isNew == false)
        {
            nc = JsonUtils.JsonToClassOrStruct<ClassValue>(dataList[indexEdit]);
           
            if (!string.IsNullOrEmpty(nc.ScriptName))
            {
                value = nc.GetValue();
                for (int i = 0; i < componetNameArr.Length; i++)
                {
                    if (componetNameArr[i] == componetPathName)
                    {
                        selectInt0 = i;
                        return;
                    }
                }
            }
        }
    }
    private int selectInt0 = 0;
    string[] componetNameArr;
   public object value = null;
   private object editorExtendClassNameValue =null;
    void OnGUI()
    {
        if (componetNameArr.Length == 0)
            return;
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        
        selectInt0 = EditorGUILayout.Popup(logicComponetType.ToString(), selectInt0, componetNameArr);

        GUILayout.EndHorizontal();
        GUILayout.Space(15);

      ComponentNameAttribute tn= CompontNameAttributeUtils.GetCompontNameAttribute(logicComponetType, componetNameArr[selectInt0]);
         value = GetInstance(tn.className, value);
      Type extendEditorType = EditorExtendAttributeUtils.GetEditorExtendType(typeof(EditorExtendBase), value.GetType());// GetEditorExtendType(value.GetType()); 
      if (extendEditorType != null)
      {
          if (editorExtendClassNameValue == null || editorExtendClassNameValue.GetType() != extendEditorType)
          {
              if (editorExtendClassNameValue != null)
              {
                  ((EditorExtendBase)editorExtendClassNameValue).Close();
              }
              editorExtendClassNameValue = Activator.CreateInstance(extendEditorType);
          }
          ((EditorExtendBase)editorExtendClassNameValue).EditorOverrideClassGUI(value);
          GUILayout.Space(10);
          ((EditorExtendBase)editorExtendClassNameValue).EditorExtendGUI(value);
      }
      else
      {
          if (editorExtendClassNameValue != null)
          {
              ((EditorExtendBase)editorExtendClassNameValue).Close();
          }
         
          LogicSystemAttributeEditorGUI.DrawInternalVariableGUI(value);
      }
        GUILayout.Space(15);
        if (value != null)
        {
            //Debug.Log(" ValueType  :" + value.GetType() + "  baseType:" + value.GetType().BaseType);
            GUILayout.Label(((LogicComponentBase)value).ToExplain());
        }
        GUILayout.Space(15);
       
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("确定", GUILayout.Width(120)))
        {
            nc= new ClassValue (value);
            string temp = JsonUtils.ClassOrStructToJson(nc);
            if (isNew)
            {
                dataList.Add(temp);
            }
            else
            {
                dataList[indexEdit] = temp;
            }
            Close();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("取消", GUILayout.Width(120)))
        {
            Close();
        }

        GUILayout.EndHorizontal();
    }
    void OnDestroy()
    {
        if (editorExtendClassNameValue != null)
        {
            ((EditorExtendBase)editorExtendClassNameValue).Close();
            editorExtendClassNameValue = null;
        }
    }

    private object GetInstance(string className, object obj)
    {
        Type t = ReflectionUtils.GetTypeByTypeFullName(className);

        if (obj == null)
            obj = Activator.CreateInstance(t);
        else
        {
            if (t.Name != obj.GetType().Name)
            {
                obj = Activator.CreateInstance(t);
            }
        }

        return obj;
    }
    /// <summary>
    /// 获取组件扩展复写Editor脚本type
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    //private Type GetEditorExtendType(Type targetType)
    //{
    //   Type[] childTypes = ReflectionUtils.GetChildTypes(typeof(EditorExtendBase));

    //   foreach (Type ch in childTypes)
    //   {
    //       object[] ss = ch.GetCustomAttributes(typeof(EditorExtendAttribute),true);
    //       if(ss.Length>0)
    //       {
    //           EditorExtendAttribute le = (EditorExtendAttribute)ss[0];
    //           if (le.tagetExtend == targetType)
    //               return ch;
    //       }
    //       else
    //       {
    //           continue;
    //       }
    //   }
    //   return null;
    //}
}
