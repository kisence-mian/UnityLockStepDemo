using LockStepDemo;
using LockStepDemo.Reflect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class DataGenerateTool
{
    static string ProjectPath
    {
        get
        {
            return System.Environment.CurrentDirectory + "\\..\\..\\..\\" + ReflectTool.GetProjectName();
        }
    }

    static string m_directoryPath;
    static List<String> m_dataNameList = new List<string>();

    public static void CreateAllClass()
    {
        FindAllDataName();

        for (int i = 0; i < m_dataNameList.Count; i++)
        {
            if (m_dataNameList[i] != null && m_dataNameList[i] != "None")
            {
                CreatDataCSharpFile(m_dataNameList[i], DataManager.GetData(m_dataNameList[i]));
            }
        }

        Debug.Log("Data count " + m_dataNameList.Count);
    }

    static void CreatDataCSharpFile(string dataName, DataTable data)
    {
        if (dataName.Contains("/"))
        {
            string[] tmp = dataName.Split('/');
            dataName = tmp[tmp.Length - 1];
        }

        string className = dataName + "Generate";
        string content = "";

        content += "using System;\n";
        content += "using UnityEngine;\n\n";

        content += @"//" + className + "类\n";
        content += @"//该类自动生成请勿修改，以避免不必要的损失";
        content += "\n";

        content += "public class " + className + " : DataGenerateBase \n";
        content += "{\n";

        content += "\tpublic string m_key;\n";

        //type
        List<string> type = new List<string>(data.m_tableTypes.Keys);

        Debug.Log("type count: " + type.Count);

        if (type.Count > 0)
        {
            for (int i = 1; i < data.TableKeys.Count; i++)
            {
                string key = data.TableKeys[i];
                string enumType = null;

                if (data.m_tableEnumTypes.ContainsKey(key))
                {
                    enumType = data.m_tableEnumTypes[key];
                }

                string note = ";";

                if (data.m_noteValue.ContainsKey(key))
                {
                    note = @"; //" + data.m_noteValue[key];
                }

                content += "\t";

                if (data.m_tableTypes.ContainsKey(key))
                {
                    //访问类型 + 字段类型  + 字段名
                    content += "public " + OutPutFieldName(data.m_tableTypes[key], enumType) + " m_" + key + note;
                }
                //默认字符类型
                else
                {
                    //访问类型 + 字符串类型 + 字段名 
                    content += "public " + "string" + " m_" + key + note;
                }

                content += "\n";
            }
        }

        content += "\n";

        content += "\tpublic override void LoadData(string key) \n";
        content += "\t{\n";
        content += "\t\tDataTable table =  DataManager.GetData(\"" + dataName + "\");\n\n";
        content += "\t\tif (!table.ContainsKey(key))\n";
        content += "\t\t{\n";
        content += "\t\t\tthrow new Exception(\"" + className + " LoadData Exception Not Fond key ->\" + key + \"<-\");\n";
        content += "\t\t}\n";
        content += "\n";
        content += "\t\tSingleData data = table[key];\n\n";

        content += "\t\tm_key = key;\n";

        if (type.Count > 0)
        {
            for (int i = 1; i < data.TableKeys.Count; i++)
            {
                string key = data.TableKeys[i];

                content += "\t\t";

                string enumType = null;

                if (data.m_tableEnumTypes.ContainsKey(key))
                {
                    enumType = data.m_tableEnumTypes[key];
                }

                if (data.m_tableTypes.ContainsKey(key))
                {
                    content += "m_" + key + " = data." + OutPutFieldFunction(data.m_tableTypes[key], enumType) + "(\"" + key + "\")";
                }
                //默认字符类型
                else
                {
                    content += "m_" + key + " = data." + OutPutFieldFunction(FieldType.String, enumType) + "(\"" + key + "\")";
                    Debug.LogWarning("字段 " + key + "没有配置类型！");
                }

                content += ";\n";
            }
        }

        content += "\t}\n";
        content += "}\n";

        string SavePath = ProjectPath + "/Generate/" + DataManager.c_directoryName + "/" + className + ".cs";

        FileTool.WriteStringByFile(SavePath, content.ToString());
    }

    static string OutPutFieldFunction(FieldType fileType, string enumType)
    {
        switch (fileType)
        {
            case FieldType.Bool: return "GetBool";
            case FieldType.Color: return "GetColor";
            case FieldType.Float: return "GetFloat";
            case FieldType.Int: return "GetInt";
            case FieldType.String: return "GetString";
            case FieldType.Vector2: return "GetVector2";
            case FieldType.Vector3: return "GetVector3";
            case FieldType.Enum: return "GetEnum<" + enumType + ">";
            case FieldType.StringArray: return "GetStringArray";
            default: return "";
        }
    }

    static string OutPutFieldName(FieldType fileType, string enumType)
    {
        switch (fileType)
        {
            case FieldType.Bool: return "bool";
            case FieldType.Color: return "Color";
            case FieldType.Float: return "float";
            case FieldType.Int: return "int";
            case FieldType.String: return "string";
            case FieldType.Vector2: return "Vector2";
            case FieldType.Vector3: return "Vector3";
            case FieldType.Enum: return enumType;
            case FieldType.StringArray: return "string[]";
            default: return "";
        }
    }

    static void FindAllDataName()
    {
        m_dataNameList = new List<string>();
        m_dataNameList.Add("None");
        m_directoryPath = ProjectPath + "/" + DataManager.c_directoryName;

        FindConfigName(m_directoryPath);
    }

    public static void FindConfigName(string path)
    {
        string[] allUIPrefabName = Directory.GetFiles(path);
        foreach (var item in allUIPrefabName)
        {
            if (item.EndsWith(".txt"))
            {
                m_dataNameList.Add(FileTool.RemoveExpandName(FileTool.GetDirectoryRelativePath(m_directoryPath + "/", item)));
            }
        }

        string[] dires = Directory.GetDirectories(path);
        for (int i = 0; i < dires.Length; i++)
        {
            FindConfigName(dires[i]);
        }
    }

}
