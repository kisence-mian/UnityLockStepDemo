using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class LogicSystemAttributeEditorGUI  {

    public static void DrawInternalVariableGUI(object obj)
    {
        if (obj == null )
            return ;
        Type t = obj.GetType();
        FieldInfo[] fs = t.GetFields();
        GUILayout.BeginVertical("box");
        foreach (FieldInfo f in fs)
        {
            DrawAttributeGUI(obj, f);
        }
        GUILayout.EndVertical();


    }
    private static void DrawAttributeGUI(object obj, FieldInfo f)
    {
        LogicComponentBase td = obj as LogicComponentBase;

        NoShowInEditorAttribute ns = GetAttribute<NoShowInEditorAttribute>(f);
        if (ns != null)
        {
            return ;
        }
        InternalValueAttribute tempAtt = GetAttribute<InternalValueAttribute>(f);
        if (tempAtt != null)
        {
             DrawUseInternalValueGUI(td, f);
             return;
        }
        InternalValueMenuAttribute tempAtt1 = GetAttribute<InternalValueMenuAttribute>(f);
        if (tempAtt1 != null)
        {
            object value = f.GetValue(obj);
            if (value is string)
            {
              value =  DrawInternalValueMenu( f.Name, value.ToString(), tempAtt1.menuTypeNames);
                f.SetValue(obj, value);
                return;
            }
        }
        RangeAttribute tempAtt2 = GetAttribute<RangeAttribute>(f);
        if (tempAtt2 != null)
        {
            object value = f.GetValue(obj);
            if (value is float)
            {
              value =  EditorGUILayout.Slider(f.Name, (float)value, tempAtt2.min, tempAtt2.max);           
            }else if(value is int){
                value = EditorGUILayout.IntSlider(f.Name, (int)value, (int)tempAtt2.min, (int)tempAtt2.max);
            }
            f.SetValue(obj, value);
            return;
               
        }

        object value1 = EditorDrawGUIUtil.DrawBaseValue(f.Name, f.GetValue(obj));
        f.SetValue(obj, value1);
    }

    public static T GetAttribute<T>(FieldInfo f) where T : Attribute
    {
        foreach (Attribute a in f.GetCustomAttributes(true))
        {
            T ns = a as T;
            if (ns != null)
            {
                return ns;
            }

        }
        return null;
    }
    /// <summary>
    /// 绘制内部变量使用的GUI
    /// </summary>
    /// <param name="logicComponentObj"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    public static void DrawUseInternalValueGUI(LogicComponentBase td, FieldInfo f)
    {
        if (td == null)
            return ;
        InternalValueInfo info = td.GetInternalValueInfoByFieldName(f.Name);
        GUILayout.BeginHorizontal();
        if (info == null)
        {
            object temp = EditorDrawGUIUtil.DrawBaseValue(f.Name, f.GetValue(td));
            f.SetValue(td, temp);
        }
        else
        {
            if (StateMachineEditorWindow.Instance != null)
            {
                List<string> names = LogicSystemEditorWindow.data.GetInternalValueNamesByTypes(new string[] { f.FieldType.FullName });
                info.internalValueName = EditorDrawGUIUtil.DrawPopup(f.Name, info.internalValueName, names);
            }
        }
        if (GUILayout.Button("o", GUILayout.Width(25)))
        {
            if (info == null)
            {
                InternalValueInfo t = new InternalValueInfo();
                t.fieldName = f.Name;
                td.internalValueInfoList.Add(t);
            }
            else
            {
                td.internalValueInfoList.Remove(info);
            }

        }
        GUILayout.EndHorizontal();
      
    }
    /// <summary>
    /// 使用 绘制内部变量菜单
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="typeNames"></param>
    /// <returns></returns>
    public static object DrawInternalValueMenu(string name,string value,  string[] typeNames)
    {
        if (LogicSystemEditorWindow.data != null)
        {
          List<string>  names = LogicSystemEditorWindow.data.GetInternalValueNamesByTypes(typeNames);
            BaseValue v = LogicSystemEditorWindow.data.GetBaseValue(value);
            if (v != null)
            {
                name += "(" + v.typeName + ")";
            }
            return EditorDrawGUIUtil.DrawPopup(name, value, names);  
        }
        else
        {
            return EditorDrawGUIUtil.DrawBaseValue(name, value);
        }
      
    }
}
