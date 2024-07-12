using HDJ.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CompontNameAttributeUtils  {

  static  bool isInit = false;
  static List< ComponentNameAttribute> nameList = new List< ComponentNameAttribute>();
    private static void Init()
    {
        if (isInit)
            return;
        isInit = true;
        Type[] typeList = ReflectionUtils.GetChildTypes(typeof(LogicComponentBase));

        foreach (Type currentType in typeList)
        {
            var attribute = (ComponentNameAttribute)Attribute.GetCustomAttribute(currentType, typeof(ComponentNameAttribute));

            if (attribute != null)
            {
                attribute.className = currentType.FullName;
                nameList.Add( attribute);
            }
        }
    }
    public static string[] GetCompontNameAttributeArray(LogicComponentType componentType,string logicFileUseType)
    {
        Init();
      List< string> tempList = new List<string>();
       foreach (ComponentNameAttribute tn in nameList)
       {
           if (tn.componentType == componentType && tn.CompareComponentUseTypes(logicFileUseType))
           {
              tempList.Add(tn.menuName);
           }
       } 
       return tempList.ToArray();
    }

   

    public static ComponentNameAttribute GetCompontNameAttribute(LogicComponentType componentType, string menuName)
    {
        Init();
        foreach (ComponentNameAttribute tn in nameList)
        {
            if (tn.menuName == menuName && tn.componentType == componentType)
            {
                return tn;
            }
        }
        return null;
    }
    public static ComponentNameAttribute GetCompontNameAttributeByClassName(string className)
    {
        Init();
        foreach (ComponentNameAttribute tn in nameList)
        {
            if (tn.className == className)
            {
                return tn;
            }
        }
        return null;
    }
}




