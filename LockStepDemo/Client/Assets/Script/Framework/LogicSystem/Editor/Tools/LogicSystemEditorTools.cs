using HDJ.Framework.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSystemEditorTools  {

    public static object GetDefultValueByTypeName(string typeName)
    {

        if (typeName == typeof(string).FullName)
            return "";
        else if (typeName == typeof(int).FullName)
            return 0;
        else if (typeName == typeof(float).FullName)
            return 0f;
        else if (typeName == typeof(Vector2).FullName)
            return Vector2.zero;
        else if (typeName == typeof(Vector3).FullName)
            return Vector3.zero;
        else if (typeName == typeof(bool).FullName)
            return false;

        return null;

    }
    public const string pathAssetsPath = "Assets/Script/Framework/LogicSystem/Editor/Config/LogicSystemEditorPath.asset";
   public  static string GetPath(string dataName, string pathName)
    {
        MessageStringData ms = GetMessageStringData();
        string path = ms.GetValue(pathName);
        path = path + "/" + dataName + ".txt";
        path = PathUtils.GetSpecialPath(path, SpecialPathType.Resources);
        Debug.Log("Path:" + path);

        return path;
    }
   public static MessageStringData GetMessageStringData()
   {
       return ScriptableObjectUtils.LoadCreateScriptableObject<MessageStringData>(pathAssetsPath);
    }
}
