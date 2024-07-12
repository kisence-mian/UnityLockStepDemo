using HDJ.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorExtendAttributeUtils  {



    public static object GetEditorExtend(Type baseEditorExtendType, Type targetType)
    {
        Type t = GetEditorExtendType(baseEditorExtendType, targetType);
        if (t == null)
            return null;
        else
            return ReflectionUtils.CreateDefultInstance(t);
    }

    /// <summary>
    /// 获取组件扩展复写Editor脚本type
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public static Type GetEditorExtendType(Type baseEditorExtendType, Type targetType)
    {
        Type[] childTypes = ReflectionUtils.GetChildTypes(baseEditorExtendType);

        foreach (Type ch in childTypes)
        {
            object[] ss = ch.GetCustomAttributes(typeof(EditorExtendAttribute), true);
            if (ss.Length > 0)
            {
                EditorExtendAttribute le = (EditorExtendAttribute)ss[0];
                if (le.tagetExtend == targetType)
                    return ch;
            }
            else
            {
                continue;
            }
        }
        return null;
    }
}
