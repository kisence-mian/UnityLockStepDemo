using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Csharp2Erlang
{
    public static string savePath;

    [MenuItem("Tools/ConvertTest")]
    public static void ConvertTest()
    {
        Debug.Log(Convert2Erlang(typeof(CharacterBase)));
    }

    public static string Convert2Erlang(Type csharpClass)
    {
        string content = "";

        content += "-module("+ CsharpName2ErlangName(csharpClass.Name) + ").\n\n";

        MethodInfo[] methodInfos =  csharpClass.GetMethods();

        for (int i = 0; i < methodInfos.Length; i++)
        {
            content += CsharpName2ErlangName(methodInfos[i].Name) + "(" + GetFunctionFieldsContent(methodInfos[i]) + ") -> \n";
            content += "\tok.\n\n";
        }

        return content;
    }

    static string CsharpName2ErlangName(string name)
    {
        name = name.Substring(0, 1).ToUpper() + name.Substring(1);

        return "cs" + name;
    }

    static string CsharpParameterName2ErlangParameterName(string name)
    {
        name = name.Substring(0, 1).ToUpper() + name.Substring(1);

        return name;
    }

    static string GetFunctionFieldsContent(MethodInfo method)
    {
        string content = "";

        ParameterInfo[] paras = method.GetParameters();

        for (int i = 0; i < paras.Length; i++)
        {
            content += CsharpParameterName2ErlangParameterName(paras[i].Name);

            if(i != paras.Length - 1)
            {
                content += ",";
            }
        }
        return content;
    }
}
