using FrameWork.Protocol;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TransverterCreater
{
    const string className = "MsgTransverter";
    static List<Type> typeList = new List<Type>();

    [MenuItem("Tools/Protocol/生成转换器")]
    static void GenerateAnalysisCode()
    {
        GenerateList();

        string csharpContent = GenerateCSharpContent();

        string SavePath = Application.dataPath + "/Script/" + ProtocolHelper.PathName + "/" + className + ".cs";
        ResourceIOTool.WriteStringByFile(SavePath, csharpContent);

        AssetDatabase.Refresh();
    }

    static void GenerateList()
    {
        typeList.Clear();

        Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();

        for (int i = 0; i < types.Length; i++)
        {
            if ((types[i].IsSubclassOf(typeof(PursueCommandBase))
                || types[i].IsSubclassOf(typeof(SyncCommandBase)))
                && !types[i].IsAbstract
                )
            {
                typeList.Add(types[i]);
            }
        }
    }

    static string GenerateCSharpContent()
    {
        string csharpContent = "";

        csharpContent += "using Protocol;\n";
        csharpContent += "using System;\n";
        csharpContent += "using System.Collections.Generic;\n";
        csharpContent += "using UnityEngine;\n";

        csharpContent += "\n";
        csharpContent += "//指令解析类\n";
        csharpContent += "//该类自动生成，请勿修改\n";
        csharpContent += "public class " + className + "\n";
        csharpContent += "{\n";

        csharpContent += "\t#region 外部调用\n";

        csharpContent += "\tpublic static void Init()\n";
        csharpContent += "\t{\n";

        for (int i = 0; i < typeList.Count; i++)
        {
            csharpContent += "\t\tGlobalEvent.AddTypeEvent<" + typeList[i].FullName + ">(" + GenerateReceviceFunctionName(typeList[i]) + ");\n";
        }

        csharpContent += "\t}\n";
        csharpContent += "\n";
        csharpContent += "\tpublic static void Dispose()\n";
        csharpContent += "\t{\n";

        for (int i = 0; i < typeList.Count; i++)
        {
            csharpContent += "\t\tGlobalEvent.RemoveTypeEvent<"+ typeList[i].FullName + ">(" + GenerateReceviceFunctionName(typeList[i]) + ");\n";
        }

        csharpContent += "\t}\n";

        csharpContent += "\t#endregion\n\n";

        csharpContent += "\t#region 事件接收\n";

        for (int i = 0; i < typeList.Count; i++)
        {
            csharpContent += GenerateReceviceCommandContent(typeList[i]);
        }

        csharpContent += "\t#endregion\n";

        csharpContent += "}\n";

        return csharpContent;
    }

    static string GenerateProtocolName(Type type)
    {
        string protocalName = type.Name;
        protocalName = protocalName.ToLower();
        return protocalName;
    }

    static string GenerateReceviceFunctionName(Type type)
    {
        return "Recevice" + type.Name;
    }

    static string GenerateReceviceCommandContent(Type type)
    {
        string content = "";

        content += GetTab(1) + "static void " + GenerateReceviceFunctionName(type) + "("+ type.FullName + " e , params object[] objs)\n";
        content += GetTab(1) + "{\n";

        if(type.IsSubclassOf(typeof(PursueCommandBase)))
        {
            content += GetTab(2) + "CommandRouteService.RecevicePursueCommand(e);\n";
        }
        else
        {
            content += GetTab(2) + "CommandRouteService.ReceviceSyncCommand(e);\n";
        }
        content += GetTab(1) + "}\n";

        return content;
    }

    static string GetTab(int tabCount)
    {
        string tabContent = "";

        for (int i = 0; i < tabCount; i++)
        {
            tabContent += "\t";
        }

        return tabContent;
    }
}
