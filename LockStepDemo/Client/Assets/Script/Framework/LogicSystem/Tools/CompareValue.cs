using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareValue  {

    public static List<string> GetCompareType(Type valueType)
    {
        List<string> names = new List<string>();
        if (valueType.FullName == typeof(int).FullName || valueType.FullName == typeof(float).FullName)
        {
            names.Add("Greater");
            names.Add("Greater_Equal");
            names.Add("Equal");
            names.Add("Less");
            names.Add("Less_Equal");
            names.Add("NoEqual");
            
        }
        else if (valueType.FullName == typeof(bool).FullName || valueType.FullName == typeof(string).FullName || valueType.FullName == typeof(Vector2).FullName || valueType.FullName == typeof(Vector3).FullName )
        {
            names.Add("Equal");
            names.Add("NoEqual");

        }
        return names;
    }

    public static bool Compare(object value,object value1, string compareType)
    {
        Type valueType = value.GetType();
        if (valueType.FullName != value1.GetType().FullName)
        {
            Debug.LogError("比较的两个值的类型不相同。 Value: " + valueType + "  value1:" + value1.GetType());
            return false;
        }
        if (valueType.FullName == typeof(float).FullName)
        {
            return CompareFloatValue(compareType, (float)value, (float)value1);
        }
        else if (valueType.FullName == typeof(int).FullName)
        {
            value = Convert.ChangeType(value, typeof(float));
            value1 = Convert.ChangeType(value1, typeof(float));
            return CompareFloatValue(compareType, (float)value, (float)value1);
        }
        else
        {
            if (valueType.FullName == typeof(bool).FullName)
            {
                return CompareBoolValue(compareType, (bool)value, (bool)value1);
            }
            else if (valueType.FullName == typeof(string).FullName)
            {
                return CompareStringValue(compareType, value.ToString(), value1.ToString());
            }
            else if (valueType.FullName == typeof(Vector2).FullName)
            {
                return CompareVector2Value(compareType, (Vector2)value, (Vector2)value1);
            }
            else if (valueType.FullName == typeof(Vector3).FullName)
            {
                return CompareVector3Value(compareType, (Vector3)value, (Vector3)value1);
            }
            else
                Debug.LogError("不支持的比较类型：Type:" + valueType);
        }
        return false;
    }

    public static bool CompareFloatValue(string type, float v0, float v1)
    {
        switch (type)
        {
            case "Greater":
                return v0 > v1;
            case "Greater_Equal":
                return v0 >= v1;
            case "Equal":
                return v0 == v1;
            case "Less":
                return v0 < v1;
            case "Less_Equal":
                return v0 <= v1;
            case "NoEqual":
                return v0 != v1;
        }
        return true;
    }
    public static bool CompareBoolValue(string type, bool v0, bool v1)
    {
        switch (type)
        {
            case "Equal":
                return v0 == v1;
            case "NoEqual":
                return v0 != v1;
        }
        return true;
    }
    public static bool CompareStringValue(string type, string v0, string v1)
    {
        switch (type)
        {
            case "Equal":
                return v0 == v1;
            case "NoEqual":
                return v0 != v1;
        }
        return true;
    }
    public static bool CompareVector3Value(string type, Vector3 v0, Vector3 v1)
    {
        switch (type)
        {
            case "Equal":
                return v0 == v1;
            case "NoEqual":
                return v0 != v1;
        }
        return true;
    }
    public static bool CompareVector2Value(string type, Vector2 v0, Vector2 v1)
    {
        switch (type)
        {
            case "Equal":
                return v0 == v1;
            case "NoEqual":
                return v0 != v1;
        }
        return true;
    }
}
