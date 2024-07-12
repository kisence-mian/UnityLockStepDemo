using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigTool  {

	public static string Value2TableStringValue(object value)
    {
        Type type = value.GetType();
        if(type.FullName == typeof(Vector3).FullName)
        {
            return ((Vector3)value).ToSaveString();
        }
        if (type.FullName == typeof(Vector2).FullName)
        {
            return ((Vector2)value).ToSaveString();
        }
        if (type.FullName == typeof(Color).FullName)
        {
            return ((Color)value).ToSaveString();
        }
        if (type.FullName == typeof(int).FullName)
        {
            return value.ToString();
        }
        if (type.FullName == typeof(float).FullName)
        {
            return value.ToString();
        }
        if (type.FullName == typeof(bool).FullName)
        {
            return value.ToString();
        }
        if (type.FullName == typeof(string).FullName)
        {
            return value.ToString();
        }
        if (type.IsEnum)
        {
            return value.ToString();
        }
        if (type.IsArray)
        {
            return ((string[])value).ToSaveString();
        }

        return value.ToString();
    }
}
