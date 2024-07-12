using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using HDJ.Framework.Tools;
using HDJ.Framework.Utils;


/// <summary>
/// 脚本变量值
/// </summary>
[System.Serializable]
public class BaseValue
{
    /// <summary>
    /// 变量名字
    /// </summary>
    public string name="";
    /// <summary>
    /// 类型
    /// </summary>
    public string typeName="";

    public Type ValueFullType
    {
        get
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            if(valueType == ValueType.CsharpBaseValue || valueType == ValueType.ClassOrStruct || valueType == ValueType.Enum)
            {
                return ReflectionUtils.GetTypeByTypeFullName(typeName);
            }
            else if (valueType == ValueType.Array)
            {
               return ReflectionUtils.GetTypeByTypeFullName(typeName+"[]");
            }
            else if (valueType == ValueType.Dictionary)
            {
                string[] tempArr = typeName.Split(',');
                Type type = ReflectionUtils.GetTypeByTypeFullName(tempArr[0]);
                Type type1 = ReflectionUtils.GetTypeByTypeFullName(tempArr[1]);
                Type dicType = typeof(Dictionary<,>).MakeGenericType(type, type1);
                return dicType;
            }
            else if (valueType == ValueType.List)
            {
                Type type = ReflectionUtils.GetTypeByTypeFullName(typeName);
                Type dicType = typeof(List<>).MakeGenericType(type);
                return dicType;
            }
            return null;
        }
    }

     [SerializeField]
    private ValueType valueType = ValueType.CsharpBaseValue;
    [SerializeField]
    private string value = "";
    
    private object cacheObje = null;
    public BaseValue() { }
    public BaseValue(string vName, object vValue)
    {
        SetValue(vName, vValue);
    }
    public void SetValue(object vVlaue)
    {
        SetValue(name, vVlaue);
    }
    public void SetValue(string vName, object vValue)
    {
        if (vValue == null)
        {
            Debug.LogError("variableValue is null, variableName:" + vName);
            return;
        }
        cacheObje = vValue;

        this.name = vName;
        Type type = vValue.GetType();
        typeName = type.FullName;

        if (BaseValueUtils.IsSupportBaseValueParseJson(type,false))
        {
            value = vValue.ToString();
            valueType = ValueType.CsharpBaseValue;
        }
        else if (type.IsArray)
        {
            typeName = type.GetElementType().FullName;
            value = JsonUtils.ArrayToJson(vValue);
            valueType = ValueType.Array;
        }
        else if (type.IsGenericType)
        {
            if (typeof(List<>).Name == type.Name)
            {
                Type t = type.GetGenericArguments()[0];
                typeName = t.FullName;
                value = JsonUtils.ListToJson(vValue);
                valueType = ValueType.List;
            }
            else if(typeof(Dictionary<,>).Name == type.Name)
            {
                Type[] tArr = type.GetGenericArguments();
                typeName =tArr[0].FullName+","+tArr[1].FullName;
                valueType = ValueType.Dictionary;
                value = JsonUtils.DictionaryToJson(vValue);
            }
        }
        else if (type.IsEnum)
        {
            value = ((int)vValue).ToString();
            valueType = ValueType.Enum;
        }
        else
        {
            if (type.IsClass || type.IsValueType)
            {
                valueType = ValueType.ClassOrStruct;
                value = JsonUtils.ClassOrStructToJson(vValue);
            }
        }
    }

    public object GetValue()
    {
        object obj = null;
        if (string.IsNullOrEmpty(typeName) || (string.IsNullOrEmpty(value) && typeName != typeof(string).FullName))
            return obj;
        if (cacheObje != null)
            return cacheObje;

        Type type = null;
        Type type1 = null;
        if (valueType != ValueType.Dictionary) {
            type = ReflectionUtils.GetTypeByTypeFullName(typeName);
        }
        else
        {
            string[] tempArr = typeName.Split(',');
            type = ReflectionUtils.GetTypeByTypeFullName(tempArr[0]);
            type1 = ReflectionUtils.GetTypeByTypeFullName(tempArr[1]);
        }
        try
        {
            if (valueType == ValueType.CsharpBaseValue)
            {
                if (typeName == typeof(string).FullName)
                    obj = value;
                else
                    obj = Convert.ChangeType(value, type);
            }
            else if (valueType == ValueType.Array)
            {
                obj = JsonUtils.JsonToArray(value, type);
            }
            else if (valueType == ValueType.List)
            {
                obj = JsonUtils.JsonToList(value, type);
                
            }
            else if (valueType == ValueType.Dictionary)
            {
                obj = JsonUtils.JsonToDictionary(value, type, type1);
            }
            else if (valueType == ValueType.Enum)
            {
                int temp = int.Parse(value);
                try
                {
                    obj = Enum.ToObject(type, temp);
                }
                catch
                {
                    obj = Enum.ToObject(type, 0);
                    Debug.LogError("枚举转换失败name:" + name + " value : " + value + "  typeName:" + typeName);
                }
            }
            else if (valueType == ValueType.ClassOrStruct)
            {
                obj = JsonUtils.JsonToClassOrStruct(value, Type.GetType(typeName));
            }
           
        }
        catch (Exception e)
        {
            Debug.LogError("Error name:" + name + " value : " + value + "  typeName:" + typeName +" valueType:"+valueType+"\n"+e);
           
        }

        cacheObje = obj;
        return obj;
    }
    public bool EqualTo(BaseValue v)
    {
        if (v == null) return false;

        if (typeName.Equals(v.typeName) && value.Equals(v.value) && valueType.Equals(v.valueType))
            return true;

        return false;
    }

    /// <summary>
    /// 特殊值类型
    /// </summary>
    public enum ValueType
    {
        CsharpBaseValue,
        List,
        Dictionary,
        ClassOrStruct,
        Enum,
        Array,
    }
}


