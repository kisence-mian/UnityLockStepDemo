using UnityEngine;
using UnityEditor;
using HDJ.Framework.Utils;
using System;
using System.Collections.Generic;

public  class ComponentsNameToConstEditorTool 
{
    //public const string ClientCodePath = "Assets/Script/Generate/";
    public const string CommontCodePath = "Assets/Script/Generate/";

    [MenuItem("Tools/ECS/Generate ECS Component Name")]
   // [InitializeOnLoadMethod]
    static void Generate()
    {
       // int compNum = PlayerPrefs.GetInt("ComponentCount", 0);

        Type[] allTypes = ReflectionUtils.GetChildTypes(typeof(ComponentBase));

        List<Type> userTypes = new List<Type>();

        foreach (var item in allTypes)
        {
            //if (item == typeof(RecordComponent<>))
            //    continue;
            if (item == typeof(SingletonComponent))
                continue;
            if (item == typeof(MomentComponentBase))
                continue;
            if (item == typeof(MomentSingletonComponent))
                continue;
            if (item == typeof(ViewComponent))
                continue;

                userTypes.Add(item);
        }

        string code = CreateCode(0, userTypes.ToArray());

        FileUtils.CreateTextFile(CommontCodePath + "ComponentType.cs", code);

        AssetDatabase.Refresh();
    }


    private static string CreateCode(int startID ,Type[] componentTypes)
    {
        string code = "using UnityEngine;\n\n";
        code += "//自动生成请勿更改\n\n";
           code+= "public  partial class ComponentType :ComponentTypeBase\n {\n";
       

        List<string> tempNames = new List<string>();
        for (int i = 0; i < componentTypes.Length; i++)
        {
            Type t = componentTypes[i];
            string name = t.Name;
            if (t.IsGenericType)
            {
                name = name.Remove(name.Length - 2);
               Type[] tempTypes = t.GetGenericArguments();
                for (int j = 0; j < tempTypes.Length; j++)
                {
                    name += "_" + tempTypes[j].Name;
                }
            }
            code += "\t public const int "+name+" = "+(startID+i)+";\n";
            tempNames.Add(name);
        }

        code += "\tpublic override int Count()\n";
        code += "\t  {\n";
        code += "\t\t return " + componentTypes.Length + ";\n";
        code += "\t   }\n\n";

        code += "\n\n";
        code += "\t public override int GetComponentIndex(string name) \n";
        code += "\t {\n";
        code += "\t\t switch (name) \n";
        code += "\t\t {\n\n";
        foreach (var item in tempNames)
        {
            code += "\t\t\t case \"" + item + "\" : \n";
            code += "\t\t\t\t return " + item + " ; \n";
        }


        code += "\t\t }\n";
        code += "\t  Debug.Log(\"未找到对应的组件 ：\" + name); \n";
        code += "\t return -1 ; \n";
        code += " \t }\n";
       
        code += "}\n";

        return code;
    }
}