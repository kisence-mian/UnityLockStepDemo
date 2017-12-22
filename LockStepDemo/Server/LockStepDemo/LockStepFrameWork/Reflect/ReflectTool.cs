using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


public class ReflectTool
{
    public static Type[] GetTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes();
    }

    public static string ProjectPath
    {
        get
        {
            return System.Environment.CurrentDirectory + "\\..\\..\\..\\" + ReflectTool.GetProjectName();
        }
    }

    public static string GetProjectName()
    {
        return Assembly.GetExecutingAssembly().FullName.Split(',')[0];
    }

    public static Type[] GetChildTypes(Type parentType, bool isContainsAllChild = true)
    {
        List<Type> lstType = new List<Type>();
        Assembly assem = Assembly.GetAssembly(parentType);
        foreach (Type tChild in assem.GetTypes())
        {
            if (tChild.BaseType == parentType)
            {
                lstType.Add(tChild);
                if (isContainsAllChild)
                {
                    Type[] temp = GetChildTypes(tChild, isContainsAllChild);
                    if (temp.Length > 0)
                        lstType.AddRange(temp);
                }
            }
        }
        return lstType.ToArray();
    }
}

