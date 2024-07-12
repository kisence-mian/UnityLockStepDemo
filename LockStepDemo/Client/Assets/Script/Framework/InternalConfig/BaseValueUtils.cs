using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BaseValueUtils
{
    public static bool IsSupportBaseValueParseJson(Type t,bool isContainsEnum =true)
    {
        if (t.IsPrimitive || t.FullName == typeof(string).FullName )
            return true;
        if (isContainsEnum && t.IsEnum)
            return true;
        return false;
    }
    public static bool IsSupportJsonToBaseValue(Type t)
    {
        if (t.IsPrimitive || t == typeof(string) )
            return true;
        return false;
    }




}
