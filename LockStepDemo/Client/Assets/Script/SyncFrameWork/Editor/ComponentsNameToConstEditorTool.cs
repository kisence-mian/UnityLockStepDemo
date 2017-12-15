using UnityEngine;
using UnityEditor;
using HDJ.Framework.Utils;
using System;
using System.Collections.Generic;

public  class ComponentsNameToConstEditorTool 
{
    public const string ClientCodePath = "Assets/Script/Generate/";
    public const string CommontCodePath = "Assets/Script/Generate/";

    [MenuItem("Window/ECS Component Name To Constant Tool", priority = 303)]
    [RuntimeInitializeOnLoadMethod]
    static void DoIt()
    {
       // int compNum = PlayerPrefs.GetInt("ComponentCount", 0);

        Type[] allTypes = ReflectionUtils.GetChildTypes(typeof(ComponentBase));
        //if (compNum == allTypes.Length)
        //    return;
        Type[] viewTypes = ReflectionUtils.GetChildTypes(typeof(ViewComponent));

        List<Type> userTypes = new List<Type>();

        foreach (var item in allTypes)
        {
            if (item == typeof(RecordComponent<>))
                continue;
            if (item == typeof(SingletonComponent))
                continue;
            if (item == typeof(MomentComponentBase))
                continue;
            if (item == typeof(MomentSingletonComponent))
                continue;
            if (item == typeof(ViewComponent))
                continue;
            bool isHave = false;
            foreach (var tempItem in viewTypes)
            {
                if (item == tempItem)
                    isHave = true;
            }

            if (!isHave)
                userTypes.Add(item);
        }

        string code = CreateCode(0, userTypes.ToArray());

        FileUtils.CreateTextFile(CommontCodePath + "ComponentType.cs", code);

        code = CreateCode(userTypes.Count, viewTypes);

        FileUtils.CreateTextFile(ClientCodePath + "ComponentViewType.cs", code);

        AssetDatabase.Refresh();
    }

    private static string CreateCode(int startID ,Type[] componentTypes)
    {
        string code = "public partial class ComponentType \n {\n";
        for (int i = 0; i < componentTypes.Length; i++)
        {
            Type t = componentTypes[i];
            string name = t.Name;
            if (t.IsGenericType)
            {
               name = name.Replace("`","");
               Type[] tempTypes = t.GetGenericArguments();
                for (int j = 0; j < tempTypes.Length; j++)
                {
                    name += "_" + tempTypes[j].Name;
                }
            }
            code += "\t public const int "+name+" = "+(startID+i)+";";
            code += "\n";
        }

        code += "}\n";

        return code;
    }
}