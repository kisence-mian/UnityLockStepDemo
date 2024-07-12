using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 显示组件类型和菜单位置
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ComponentNameAttribute : Attribute {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="menuName"></param>
    /// <param name="componentUseTypes">组件使用范围（地图逻辑，AI等）,不填写为所有类型都可以使用</param>
    public ComponentNameAttribute(LogicComponentType type, string menuName,string[] componentUseTypes =null)
    {
        this.menuName = menuName;
        componentType = type;
        this.componentUseTypes = componentUseTypes;
    }
    public string menuName="";
    public LogicComponentType componentType;
    public string className = "";
    //组件使用范围
    string[] componentUseTypes;
    public bool CompareComponentUseTypes(string logicFileUseType)
    {
        if (string.IsNullOrEmpty(logicFileUseType)) return false;
        if (componentUseTypes == null) return true;

        foreach(string ss in componentUseTypes)
        {
            if (ss == logicFileUseType) return true;
        }

        return false;

    }
}

/// <summary>
/// 不在默认Editor GUI里面显示
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class NoShowInEditorAttribute : Attribute { }

/// <summary>
/// 能使用内部变量（针对可使用或不使用内部变量）
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class InternalValueAttribute : Attribute { }
/// <summary>
/// 能使用内部变量名字菜单（仅对string类型变量起作用）
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class InternalValueMenuAttribute : Attribute
{
    public string[] menuTypeNames;
   public InternalValueMenuAttribute(Type[] valueMenuTypes=null )
    {
        if (valueMenuTypes == null)
        {
            menuTypeNames = null;
            return;
        }
        List<string> ss = new List<string>();
        foreach (Type t in valueMenuTypes)
        {
            ss.Add(t.FullName);
        }
        menuTypeNames = ss.ToArray();
    }
}


