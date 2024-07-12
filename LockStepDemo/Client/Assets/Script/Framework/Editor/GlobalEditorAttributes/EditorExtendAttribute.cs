using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 标明是自定义组件EditorGUI
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EditorExtendAttribute : Attribute
{
    public Type tagetExtend;
    public EditorExtendAttribute(Type tagetExtend)
    {
        this.tagetExtend = tagetExtend;

    }

}
