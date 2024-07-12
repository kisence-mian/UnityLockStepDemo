using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HDJ.Framework.Utils;
using System;

public class LogicObjectDataController  {

    public const string c_directoryName = "LogicObjects/MapLogicObjects";
    public const string c_expandName = "txt";
    public static LogicObjectContainer GetDataFromFile(string dataName)
    {
#if UNITY_EDITOR
        string content = ResourceIOTool.ReadStringByFile(PathTool.GetAbsolutePath(
            ResLoadLocation.Resource, PathTool.GetRelativelyPath(c_directoryName,
                                                dataName,
                                                c_expandName)));
#else
        string content = ResourceManager.ReadTextFile(dataName);
#endif
        //string content = ResourcesManager.LoadTextFileByName(dataName);
        if (string.IsNullOrEmpty(content))
            return new LogicObjectContainer();
        return JsonUtils.JsonToClassOrStruct<LogicObjectContainer>(content);
    }

#if UNITY_EDITOR
    public static void SaveData(string dataName,string pathName, LogicObjectContainer data)
    {
        Type t = ReflectionUtils.GetTypeByTypeFullName("LogicSystemEditorTools");
        object[] paramArr = new object[] { dataName, pathName };
      object res =  ReflectionUtils.InvokMethod(t,null, "GetPath", ref paramArr);


        string path =res.ToString();
        string content =JsonUtils.ClassOrStructToJson(data);

        FileUtils.CreateTextFile(path, content);
        //if (!ResourcePathManager.ContainsFileName(dataName))
        //{
        //    ResourcePathManager.Clear();
        //}
        UnityEditor.AssetDatabase.Refresh();
    }
    //public static void DeleteFile(string dataName, string pathName)
    //{
    //    Type t = ReflectionUtils.GetTypeByTypeFullName("LogicSystemEditorTools");
    //    object[] paramArr = new object[] { dataName, pathName };
    //    object res = ReflectionUtils.InvokMethod(t, null, "GetPath", ref paramArr);
    //    string path = res.ToString();
    //   // string path = LogicSystemEditorTools.GetPath(dataName, pathName);
    //    FileUtils.DeleteFile(path);
    //    UnityEditor.AssetDatabase.Refresh();
    //}

#endif


}
