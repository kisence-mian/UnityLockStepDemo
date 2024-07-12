using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using HDJ.Framework.Utils;

/// <summary>
/// 脚本数据
/// </summary>
[System.Serializable]
public class ClassValue
{

    public string ScriptName = "";
    public List<BaseValue> fieldValues = new List<BaseValue>();
    //public List<BaseValue> propertyValues = new List<BaseValue>();

    public ClassValue(object value)
    {
        SetValue(value);
    }
    public void SetValue(object value)
    {
        if (value == null)
            return;
        fieldValues.Clear();

        Type type = value.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo[] fields = type.GetFields(flags );
        ScriptName = type.FullName;
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo f = fields[i];
            if (f.IsNotSerialized)
                continue;
            if (f.IsStatic || f.IsLiteral)
                continue;
            if (f.IsFamily || f.IsPrivate)
            {
                object[] atts = f.GetCustomAttributes(typeof(SerializeField), true);
                if (atts == null || atts.Length == 0)
                    continue;
            }
            object v = f.GetValue(value);
            if (v == null)
                continue;
            BaseValue scriptValue = new BaseValue(f.Name, v);
            fieldValues.Add(scriptValue);
        }

        //PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //for (int i = 0; i < propertyInfos.Length; i++)
        //{
        //    try
        //    {
        //        scriptValue = new BaseValue(propertyInfos[i].Name, propertyInfos[i].GetValue(value, null));
        //        propertyValues.Add(scriptValue);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e.ToString());
        //        continue;
        //    }
        //}
    }

    public object GetValue(GameObject obj = null)
    {
        if (string.IsNullOrEmpty(ScriptName))
            return null;
        Type type = ReflectionUtils.GetTypeByTypeFullName(ScriptName);
        object classObj = null;
        if (obj == null)
            classObj = ReflectionUtils.CreateDefultInstance(type);
        else
        {
            classObj = obj.GetComponent(type);
            if (classObj == null)
                classObj = obj.AddComponent(type);
        }
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        for (int i = 0; i < fieldValues.Count; i++)
        {
            BaseValue fInfo = fieldValues[i];
            FieldInfo f = type.GetField(fInfo.name, flags);
            if (f.Name == fInfo.name)
            {
                try
                {
                    f.SetValue(classObj, fInfo.GetValue());
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }
            }
        }



        //PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //for (int i = 0; i < propertyInfos.Length; i++)
        //{
        //    try
        //    {
        //        for (int j = 0; j < propertyValues.Count; j++)
        //        {
        //            BaseValue pinfo = propertyValues[i];
        //            if (propertyInfos[i].Name == pinfo.name)
        //            {
        //                propertyInfos[i].SetValue(classObj, pinfo.GetValue(), null);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //        continue;
        //    }
        //}

        return classObj;
    }
}
