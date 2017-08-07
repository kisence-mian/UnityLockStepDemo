using LockStepDemo.Reflect;
using Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LockStepDemo.Protocol
{
    public class ProtocolTool
    {
        static string ProjectPath
        {
            get
            {
                return System.Environment.CurrentDirectory + "\\..\\..\\..\\" + ReflectTool.GetProjectName();
            }
        }

        static List<Type> ModuleList = new List<Type>();
        static List<Type> msgList = new List<Type>();
        static List<Type> StructList = new List<Type>();

        public const string PathName = "Generate/Protocol";
        const string AnalysisCodeName = "ProtocolAnalysisService";
        const string ProtocolClassName = "ProtocolMessageClass";

        const string ModulePostfix = "Module";

        const int c_modMaxMessageCount = 100; //一个模块下最大允许有多少消息 建议是10的幂

        #region c# -> protocol

        public static void StartGenerate()
        {
            GenerateList();

            string protocolContent = GeneratePrototalContent();
            string protocolList = GeneratePrototalList();

            Console.WriteLine(protocolContent);

            string ProtocolSavePath = ProjectPath + "/Network/" + ProtocolReceiveFilter.c_ProtocolFileName + ".txt";
            FileTool.WriteStringByFile(ProtocolSavePath, protocolContent);

            string MethodSavePath = ProjectPath + "/Network/" + ProtocolReceiveFilter.c_methodNameInfoFileName + ".txt";
            FileTool.WriteStringByFile(MethodSavePath, protocolList);
        }

        /// <summary>
        /// 读取消息发送模式，如果不加默认为Both
        /// </summary>
        static string GenerateProtocolMessage(Type type)
        {
            string content = "";

            SendMode mode = GetSendMode(type);

            if (mode == SendMode.ToClient)
            {
                content += "message m_" + GenerateProtocolName(type) + "_c\n";
                content += GenerateProtocolMessageNoHead(type);
            }
            else if (mode == SendMode.ToServer)
            {
                content += "message m_" + GenerateProtocolName(type) + "_s\n";
                content += GenerateProtocolMessageNoHead(type);
            }
            else
            {
                content += "message m_" + GenerateProtocolName(type) + "_s\n";
                content += GenerateProtocolMessageNoHead(type);

                content += "message m_" + GenerateProtocolName(type) + "_c\n";
                content += GenerateProtocolMessageNoHead(type);
            }

            return content;
        }

        static SendMode GetSendMode(Type type)
        {
            object[] modes = type.GetCustomAttributes(typeof(MessageModeAttribute), true);

            SendMode mode = SendMode.Both;

            if (modes.Length > 0)
            {
                MessageModeAttribute att = (MessageModeAttribute)modes[0];
                mode = att.Mode;
            }

            return mode;
        }

        static string GenerateProtocolMessageNoHead(Type type)
        {
            string content = "";

            content += "{\n";

            FieldInfo[] fields = type.GetFields();

            int index = 1;

            for (int i = 0; i < fields.Length; i++)
            {
                content += GetTab(1) + GenerateProtocolMessageField(fields[i], ref index);
            }

            content += "}\n";

            return content;
        }

        static string GenerateProtocolMessageField(FieldInfo field, ref int count)
        {
            if (field.FieldType == typeof(int))
            {
                if (field.GetCustomAttributes(typeof(Int8Attribute), true).Length > 0)
                {
                    return "required int8 " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
                }
                else if (field.GetCustomAttributes(typeof(Int16Attribute), true).Length > 0)
                {
                    return "required int16 " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
                }
                else
                {
                    return "required int32 " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
                }
            }
            else if (field.FieldType == typeof(bool))
            {
                return "required bool " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
            }
            else if (field.FieldType == typeof(float))
            {
                return "required double " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
            }
            else if (field.FieldType.IsSubclassOf(typeof(Enum)))
            {
                return "required int8 " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
            }
            else if (field.FieldType == typeof(Vector3))
            {
                return "required double " + GenerateProtocolFieldName(field) + "x = " + count++ + ";\n"
                  + GetTab(1) + "required double " + GenerateProtocolFieldName(field) + "y = " + count++ + ";\n"
                  + GetTab(1) + "required double " + GenerateProtocolFieldName(field) + "z = " + count++ + ";\n";
            }
            else if (field.FieldType.Name == typeof(List<>).Name)
            {
                string content = "repeated ";
                Type type = field.FieldType.GetGenericArguments()[0];

                content += GetTypeName(type).ToLower() + " " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";

                return content;
            }

            else
            {
                return "required string " + GenerateProtocolFieldName(field) + " = " + count++ + ";\n";
            }
        }

        static string GetTypeName(Type type)
        {
            if (type == typeof(int))
            {
                if (type.GetCustomAttributes(typeof(Int8Attribute), true).Length > 0)
                {
                    return "int8";
                }
                else if (type.GetCustomAttributes(typeof(Int16Attribute), true).Length > 0)
                {
                    return "int16";
                }
                else
                {
                    return "int32";
                }
            }
            else if (type == typeof(bool))
            {
                return "bool";
            }
            else if (type == typeof(float))
            {
                return "double";
            }
            else if (type.IsSubclassOf(typeof(Enum)))
            {
                return "int8";
            }
            else if (type == typeof(string))
            {
                return "string";
            }
            else
            {
                return type.Name;
            }
        }

        static Regex replace = new Regex(@"^(\w+)(_s|_c)$");

        static string GenerateProtocolName(Type type)
        {
            string protocalName = type.Name;
            protocalName = protocalName.ToLower();

            string result = replace.Match(protocalName).Groups[1].Value;
            if (result == "")
            {
                result = protocalName;
            }

            //Console.WriteLine(protocalName + " ->" + result);

            return result;
        }

        static string GenerateProtocolFieldName(FieldInfo field)
        {
            string protocalName = field.Name;

            protocalName = protocalName.ToLower();

            return protocalName;
        }

        static string GenerateReceviceFunctionName(Type type)
        {
            return "Recevice" + type.Name;
        }

        static string GenerateSendFunctionName(Type type)
        {
            return "Send" + type.Name;
        }

        #endregion

        #region Protocol -> c#

        static List<string> s_SubStruct = new List<string>();

        public static void GenerateProtocolToCsharp()
        {
            string log = "自动生成 c# 类：->\n";

            s_SubStruct = new List<string>();
            string path = ProjectPath + "/Network/" + ProtocolReceiveFilter.c_ProtocolFileName + ".txt";
            string content = FileTool.ReadStringByFile(path);

            Dictionary<string, List<Dictionary<string, object>>> protocolInfo = ProtocolReceiveFilter.ReadProtocolInfo(content);

            path = ProjectPath + "/Network/" + ProtocolReceiveFilter.c_methodNameInfoFileName + ".txt";
            content = FileTool.ReadStringByFile(path);

            Dictionary<int, string> methodNameInfo;
            Dictionary<string, int> methodIndexInfo;

            ProtocolReceiveFilter.ReadMethodNameInfo(out methodNameInfo, out methodIndexInfo, content);

            string output = "using System.Collections.Generic;\n";
            output += "namespace Protocol\n{\n";
            output += "\t//Protocol消息文件\n";
            output += "\t//该文件自动生成，请勿修改，以避免不必要的损失\n";

            string currentModuleName = null;
            bool isFold = false;
            //根据模块，生成模块类、并折叠代码
            foreach (var item in methodNameInfo)
            {
                int tab = 1;

                if (isFold)
                {
                    tab = 2;
                }

                //判断这个是不是一个模块声明
                //是，生成模块类
                if (!item.Value.Contains("_")
                    && item.Key < 100
                    )
                {
                    if (isFold)
                    {
                        output += GetTab(1) + "}\n";
                        output += GetTab(1) + "#endregion \n\n";
                        isFold = false;
                    }

                    isFold = true;
                    output += GetTab(1) + "#region Module " + item.Value + "\n";
                    output += GetTab(1) + "namespace " + item.Value + "Module\n";
                    output += GetTab(1) + "{\n";

                    //判断是否已经有这个模块类
                    if (!GetExitsModule(item.Value))
                    {
                        output += GenerateProtocolModuleClass(tab, item.Value, item.Key);
                    }
                    else
                    {
                        log += "跳过了 " + item.Value + " 模块\n";
                    }

                    currentModuleName = item.Value;
                }
                //否，生成消息类
                else
                {
                    string className = GetMessageNmae(item.Value);

                    //Console.WriteLine(className + " -> " + item.Value);

                    if (GetAimType(className) == null)
                    {
                        string name = "m_" + item.Value + "_c";
                        if (protocolInfo.ContainsKey(name))
                        {
                            output += GenerateProtocolClass(tab, SendMode.ToClient, currentModuleName, item.Value + "_c", protocolInfo[name]);
                        }

                        name = "m_" + item.Value + "_s";
                        if (protocolInfo.ContainsKey(name))
                        {
                            output += GenerateProtocolClass(tab, SendMode.ToServer, currentModuleName, item.Value + "_s", protocolInfo[name]);
                        }
                    }
                    else
                    {
                        log += ("跳过了 " + className + " 类\n");

                        string name = "m_" + item.Value + "_c";
                        if (protocolInfo.ContainsKey(name))
                        {
                            GenerateProtocolClass(tab, SendMode.ToClient, currentModuleName, item.Value + "_c", protocolInfo[name]);
                        }

                        name = "m_" + item.Value + "_s";
                        if (protocolInfo.ContainsKey(name))
                        {
                            GenerateProtocolClass(tab, SendMode.ToServer, currentModuleName, item.Value + "_s", protocolInfo[name]);
                        }
                    }
                }
            }

            if (isFold)
            {
                output += GetTab(1) + "}\n";
                output += GetTab(1) + "#endregion \n\n";
                isFold = false;
            }

            //子结构
            output += GetTab(1) + "#region Struct\n";
            for (int i = 0; i < s_SubStruct.Count; i++)
            {
                try
                {
                    output += GenerateProtocolClass(2, SendMode.Both, null, s_SubStruct[i], protocolInfo[s_SubStruct[i]], true);
                }
                catch
                {
                    throw new Exception("s_SubStruct[i] ->" + s_SubStruct[i]);
                }
            }
            output += GetTab(1) + "#endregion \n";

            output += "}\n";

            string SavePath = ProjectPath + "/" + PathName + "/" + ProtocolClassName + ".cs";
            FileTool.WriteStringByFile(SavePath, output);

            Console.WriteLine(output);

            Console.WriteLine(log);
        }

        static string GenerateProtocolModuleClass(int tab, string ModuleName, int ModuleCode)
        {
            string content = GetTab(tab) + "[Module(" + ModuleCode + " , \"" + ModuleName + "\")]\n";

            content += GetTab(tab) + "public abstract class " + ModuleName + ModulePostfix + " : IProtocolMessageInterface {}\n\n";

            return content;
        }

        static string GenerateProtocolClass(int tab, SendMode mode, string ModuleName, string ClassName, List<Dictionary<string, object>> data, bool isStruct = false)
        {
            if (ModuleName == null)
            {
                ModuleName = "IProtocolMessageInterface";
            }
            else
            {
                if (GetExitsModule(ModuleName))
                {
                    ModuleName = GetModuleByName(ModuleName);
                }
                else
                {
                    ModuleName = ModuleName + ModulePostfix;
                }
            }

            string content = GetTab(tab) + "public class " + ClassName + " : " + ModuleName + " \n";

            if (isStruct)
            {
                content = GetTab(tab) + "public class " + ClassName + " : IProtocolStructInterface \n";
            }
            else
            {
                content = GetTab(tab) + "[MessageMode(SendMode." + mode + ")] \n";
                content += GetTab(tab) + "public class " + ClassName + " : " + ModuleName + " \n";
            }

            content += GetTab(tab) + "{\n";

            foreach (var item in data)
            {
                content += GetAttributeContent(tab + 1, item);
                content += GetTab(tab + 1) + "public " + GetTypeName(item) + " " + GetFieldName(item) + ";\n";
            }

            content += GetTab(tab) + "}\n";
            return content;
        }

        static string GetTypeName(Dictionary<string, object> currentFeidInfo)
        {
            int type = (int)currentFeidInfo["type"];
            int repeat = (int)currentFeidInfo["spl"];
            string content = "";

            if (type == ProtocolReceiveFilter.TYPE_int32
                || type == ProtocolReceiveFilter.TYPE_int16
                || type == ProtocolReceiveFilter.TYPE_int8
                )
            {
                content = "int";
            }
            else if (type == ProtocolReceiveFilter.TYPE_string)
            {
                content = "string";
            }
            else if (type == ProtocolReceiveFilter.TYPE_double)
            {
                content = "float";
            }
            else if (type == ProtocolReceiveFilter.TYPE_bool)
            {
                content = "bool";
            }
            else
            {
                content = (string)currentFeidInfo["vp"];

                if (!s_SubStruct.Contains(content))
                {
                    s_SubStruct.Add(content);
                }
            }

            if (repeat == ProtocolReceiveFilter.RT_repeated)
            {
                content = "List<" + content + ">";
            }

            return content;
        }

        static string GetAttributeContent(int tab, Dictionary<string, object> currentFeidInfo)
        {
            string content = "";

            int type = (int)currentFeidInfo["type"];

            if (type == ProtocolReceiveFilter.TYPE_int16)
            {
                content = GetTab(tab) + "[Int16]\n";
            }
            else if (type == ProtocolReceiveFilter.TYPE_int8)
            {
                content = GetTab(tab) + "[Int8]\n";
            }

            return content;
        }

        static string GetFieldName(Dictionary<string, object> currentFeidInfo)
        {
            return (string)currentFeidInfo["name"];
        }

        static Type GetAimType(string name)
        {
            Type[] types = ReflectTool.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(CsharpProtocolInterface).IsAssignableFrom(types[i])
                    && types[i].Name.ToLower() == name.ToLower()
                    )
                {
                    return types[i];
                }
            }

            return null;
        }

        static string GetModuleName(Type type, bool inherit)
        {
            object[] objx = type.GetCustomAttributes(typeof(ModuleAttribute), inherit);
            if (objx.Length != 0)
            {
                return ((ModuleAttribute)objx[0]).ModuleName;
            }
            else
            {
                return null;
            }
        }

        static string GetModuleByName(string name)
        {
            Type[] types = ReflectTool.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(CsharpProtocolInterface).IsAssignableFrom(types[i])
                    && GetModuleName(types[i], false) == name
                    )
                {
                    return types[i].FullName;
                }
            }

            throw new Exception("GetModuleByName Fail ! " + name);
        }

        static bool GetExitsModule(string name)
        {
            Type[] types = ReflectTool.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(CsharpProtocolInterface).IsAssignableFrom(types[i])
                    && GetModuleName(types[i], false) == name
                    )
                {
                    return true;
                }
            }

            return false;
        }

        static string GetMessageNmae(string name)
        {
            string[] tmp = name.Split('_');
            string result = "";

            for (int i = 1; i < tmp.Length; i++)
            {
                result += tmp[i];

                if (i != tmp.Length - 1)
                {
                    result += "_";
                }
            }

            if (result == "")
            {
                return name;
            }

            return result;
        }

        #endregion

        #region 生成解析代码
        public static void GenerateAnalysisCode()
        {
            GenerateList();
            string csharpContent = GenerateCSharpContent();

            Console.WriteLine(csharpContent);

            string SavePath = ProjectPath + "/" + PathName + "/" + AnalysisCodeName + ".cs";
            FileTool.WriteStringByFile(SavePath, csharpContent);
        }

        static void GenerateList()
        {
            msgList.Clear();

            Type[] types = ReflectTool.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(IProtocolMessageInterface).IsAssignableFrom(types[i])
                    && types[i] != typeof(IProtocolMessageInterface)
                    && types[i] != typeof(CsharpProtocolInterface)
                    && !types[i].IsAbstract
                    )
                {
                    msgList.Add(types[i]);
                }

                if (typeof(IProtocolMessageInterface).IsAssignableFrom(types[i])
                    && types[i].IsAbstract

                    )
                {
                    ModuleList.Add(types[i]);
                }
            }

            //进行排序
            msgList.Sort(sort);

            StructList.Clear();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(IProtocolStructInterface).IsAssignableFrom(types[i])
                    && types[i] != typeof(IProtocolStructInterface)
                    )
                {
                    StructList.Add(types[i]);
                }
            }
        }

        static string GenerateCSharpContent()
        {
            string csharpContent = "";

            csharpContent += "#pragma warning disable\n";
            csharpContent += "using LockStepDemo.Event;\n";
            csharpContent += "using LockStepDemo;\n";
            csharpContent += "using LockStepDemo.Service;\n";
            csharpContent += "using Protocol;\n";
            csharpContent += "using System;\n";
            csharpContent += "using System.Collections.Generic;\n";
            //csharpContent += "using UnityEngine;\n";

            csharpContent += "\n";
            csharpContent += "//指令解析类\n";
            csharpContent += "//该类自动生成，请勿修改\n";
            csharpContent += "public static class " + AnalysisCodeName + "\n";
            csharpContent += "{\n";

            //csharpContent += "\t#region 外部调用\n";

            //csharpContent += "\tpublic static void Init()\n";
            //csharpContent += "\t{\n";

            //for (int i = 0; i < msgList.Count; i++)
            //{
            //    if (GetSendMode(msgList[i]) == SendMode.ToClient
            //         || GetSendMode(msgList[i]) == SendMode.Both)
            //    {
            //        csharpContent += "\t\tInputManager.AddListener<InputNetworkMessageEvent>(\"" + GenerateProtocolName(msgList[i]) + "\"," + GenerateReceviceFunctionName(msgList[i]) + ");\n";
            //    }
            //}

            //csharpContent += "\t}\n";
            //csharpContent += "\n";
            //csharpContent += "\tpublic static void Dispose()\n";
            //csharpContent += "\t{\n";

            //for (int i = 0; i < msgList.Count; i++)
            //{
            //    if (GetSendMode(msgList[i]) == SendMode.ToClient
            //         || GetSendMode(msgList[i]) == SendMode.Both)
            //    {
            //        csharpContent += "\t\tInputManager.RemoveListener<InputNetworkMessageEvent>(\"" + GenerateProtocolName(msgList[i]) + "\"," + GenerateReceviceFunctionName(msgList[i]) + ");\n";
            //    }
            //}

            //csharpContent += "\t}\n";

            csharpContent += "\t#region 消息发送\n";

            for (int i = 0; i < msgList.Count; i++)
            {
                if (GetSendMode(msgList[i]) == SendMode.ToClient
                     || GetSendMode(msgList[i]) == SendMode.Both)
                {
                    csharpContent += GenerateSendCommandContent(msgList[i]);
                }
            }

            csharpContent += "\t#endregion\n\n";

            csharpContent += "\t#region 事件接收\n";

            csharpContent += GenerateReceviceFunction();

            for (int i = 0; i < msgList.Count; i++)
            {
                if (GetSendMode(msgList[i]) == SendMode.ToServer
                    || GetSendMode(msgList[i]) == SendMode.Both)
                {
                    csharpContent += GenerateReceviceCommandContent(msgList[i]);
                }
            }

            csharpContent += "\t#endregion\n";

            csharpContent += "}\n";

            return csharpContent;
        }

        static string GeneratePrototalList()
        {
            string content = "";
            int index = 1;

            List<Type> ModuleList = new List<Type>();
            Type[] types = ReflectTool.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (typeof(IProtocolMessageInterface).IsAssignableFrom(types[i])
                    && types[i] != typeof(IProtocolMessageInterface)
                    && types[i] != typeof(CsharpProtocolInterface)
                    && types[i].IsAbstract
                    && GetIsModule(types[i])
                    )
                {
                    ModuleList.Add(types[i]);
                }
            }

            ModuleList.Sort(sort);

            //无模块消息
            {
                int count = 0;
                List<string> nameList = new List<string>();

                for (int i = 0; i < msgList.Count; i++)
                {
                    if (GetModuleName(msgList[i], true) == null)
                    {
                        string nameTmp = GenerateProtocolName(msgList[i]);

                        if (!nameList.Contains(nameTmp))
                        {
                            nameList.Add(nameTmp);
                        }
                    }
                }

                for (int i = 0; i < nameList.Count; i++)
                {
                    content += i + "," + nameList[i] + "\n";

                    count++;

                    if (count >= c_modMaxMessageCount)
                    {
                        throw new Exception("无模块下的消息超过了 " + c_modMaxMessageCount + " ！");
                    }
                }
            }

            for (int i = 0; i < ModuleList.Count; i++)
            {
                int count = 0;
                List<string> nameList = new List<string>();

                if (i != 0)
                {
                    content += "\n";
                }

                //生成模块头
                object[] objs = ModuleList[i].GetCustomAttributes(typeof(ModuleAttribute), true);

                if (objs.Length == 0)
                {
                    throw new Exception(ModuleList[i] + " 必须要有 Module 特性 ！");
                }

                ModuleAttribute mod = (ModuleAttribute)objs[0];
                index = mod.MessageCode;
                content += index + "," + mod.ModuleName + "\n";
                index = index * c_modMaxMessageCount + 1;

                for (int j = 0; j < msgList.Count; j++)
                {
                    if (msgList[j].IsSubclassOf(ModuleList[i]))
                    {
                        string nameTmp = GenerateProtocolName(msgList[j]);
                        if (!nameList.Contains(nameTmp))
                        {
                            nameList.Add(nameTmp);
                        }
                    }
                }

                for (int j = 0; j < nameList.Count; j++)
                {
                    content += index++ + "," + nameList[j] + "\n";

                    count++;

                    if (count >= c_modMaxMessageCount)
                    {
                        throw new Exception(mod.ModuleName + "模块下的消息超过了 " + c_modMaxMessageCount + " ！");
                    }
                }
            }

            return content;
        }

        static string GeneratePrototalContent()
        {
            string content = "package all;\n\n";

            for (int i = 0; i < msgList.Count; i++)
            {
                content += GenerateProtocolMessage(msgList[i]);
                content += "\n";
            }

            //Console.WriteLine("StructList " + StructList.Count);

            for (int i = 0; i < StructList.Count; i++)
            {
                content += "message " + GenerateProtocolName(StructList[i]) + "\n";
                content += GenerateProtocolMessageNoHead(StructList[i]);
            }

            return content;
        }

        static int sort(Type x, Type y)
        {
            int vx = 0;
            int vy = 0;

            object[] objx = x.GetCustomAttributes(typeof(ModuleAttribute), true);
            if (objx.Length != 0)
            {
                vx = ((ModuleAttribute)objx[0]).MessageCode;
            }

            object[] objy = y.GetCustomAttributes(typeof(ModuleAttribute), true);
            if (objy.Length != 0)
            {
                vy = ((ModuleAttribute)objy[0]).MessageCode;
            }

            return vx.CompareTo(vy);
        }

        static bool GetIsModule(Type type)
        {
            object[] objx = type.GetCustomAttributes(typeof(ModuleAttribute), false);
            if (objx.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string GenerateReceviceFunction()
        {
            string content = "";

            content += GetTab(1) + "public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)\n";
            content += GetTab(1) + "{\n";

            content += GetTab(2) + "switch (cmd.Key)\n";
            content += GetTab(2) + "{\n";

            for (int i = 0; i < msgList.Count; i++)
            {
                if (GetSendMode(msgList[i]) == SendMode.ToServer
                    || GetSendMode(msgList[i]) == SendMode.Both)
                {
                    content += GenerateReceviceIfContent(msgList[i], i != 0);
                }
            }

            if (msgList.Count > 0)
            {
                content += GetTab(3) + "default:\n";
                content += GetTab(3) + "Debug.LogError(\"SendCommand Exception : 不支持的消息类型!\" + cmd.Key);\n";
                content += GetTab(4) + "break;\n";
            }

            content += GetTab(2) + "}\n";

            content += GetTab(1) + "}\n";

            return content;
        }

        static string GenerateReceviceIfContent(Type type, bool isElseIf)
        {
            string content = "";
            content += GetTab(3) + "case  \"" + GenerateProtocolName(type) + "\":";

            content += GenerateReceviceFunctionName(type) + "(session , cmd);";

            content += "break;\n";

            return content;
        }

        static string GenerateSendCommandContent(Type type)
        {
            string content = "";

            content += GetTab(1) + "public static void SendMsg(this SyncSession session," + type.FullName + " msg)\n";
            content += GetTab(1) + "{\n";
            //content += GetTab(2) + type.FullName + " e = (" + type.FullName + ")msg;\n";
            content += GenerateSerializeClassContent(2, type, "data", "msg");

            content += GetTab(2) + "session.SendMsg(\"" + GenerateProtocolName(type) + "\",data);\n";
            content += GetTab(1) + "}\n";

            return content;
        }

        static string GenerateSerializeClassContent(int tab, Type type, string aimName, string sourceName)
        {
            string content = "";

            content += GetTab(tab) + "Dictionary<string, object> " + aimName + " = new Dictionary<string, object>();\n";

            FieldInfo[] fields = type.GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                content += GenerateSerializeFieldContent(tab, fields[i], aimName, sourceName);
            }

            return content;
        }

        static string GenerateSerializeFieldContent(int tab, FieldInfo field, string aimName, string sourceName)
        {
            string content = "";
            if (field.FieldType.IsSubclassOf(typeof(Enum)))
            {
                content += GetTab(tab) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "\", (int)" + sourceName + "." + field.Name + ");\n";
            }

            else if (field.FieldType == typeof(Vector3))
            {
                content += GetTab(tab) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "x\", " + sourceName + "." + field.Name + ".x);\n";
                content += GetTab(tab) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "y\", " + sourceName + "." + field.Name + ".y);\n";
                content += GetTab(tab) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "z\", " + sourceName + "." + field.Name + ".z);\n";
            }

            else if (isBaseType(field.FieldType))
            {
                content += GetTab(tab) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "\", " + sourceName + "." + field.Name + ");\n";
            }

            else if (field.FieldType.Name == typeof(List<>).Name)
            {
                Type typeTmp = field.FieldType.GetGenericArguments()[0];

                if (isBaseType(typeTmp))
                {
                    content += GetTab(tab) + "{\n";

                    content += GetTab(tab + 1) + "List<object> list = new List<object>();\n";
                    content += GetTab(tab + 1) + "for(int i = 0;i <" + sourceName + "." + field.Name + ".Count ; i++)\n";

                    content += GetTab(tab + 1) + "{\n";
                    content += GetTab(tab + 2) + "list.Add( " + sourceName + "." + field.Name + "[i]);\n";
                    content += GetTab(tab + 1) + "}\n";
                    content += GetTab(tab + 1) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "\",list);\n";

                    content += GetTab(tab) + "}\n";
                }
                else
                {
                    content += GetTab(tab) + "{\n";

                    content += GetTab(tab + 1) + "List<Dictionary<string, object>> list" + tab + " = new List<Dictionary<string, object>>();\n";
                    content += GetTab(tab + 1) + "for(int i" + tab + " = 0;i" + tab + " <" + sourceName + "." + field.Name + ".Count ; i" + tab + "++)\n";

                    content += GetTab(tab + 1) + "{\n";
                    content += GenerateSerializeClassContent(tab + 2, typeTmp, "data" + tab, sourceName + "." + field.Name + "[i" + tab + "]");
                    content += GetTab(tab + 2) + "list" + tab + ".Add( data" + tab + ");\n";
                    content += GetTab(tab + 1) + "}\n";
                    content += GetTab(tab + 1) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "\",list" + tab + ");\n";

                    content += GetTab(tab) + "}\n";
                }
            }

            else
            {
                content += GetTab(tab + 1) + "{\n";
                content += GenerateSerializeClassContent(tab + 2, field.FieldType, "data" + tab, sourceName + "." + field.Name);
                content += GetTab(tab + 2) + aimName + ".Add(\"" + GenerateProtocolFieldName(field) + "\",data" + tab + ");\n";
                content += GetTab(tab + 1) + "}\n";
            }

            return content;
        }

        static string GenerateReceviceCommandContent(Type type)
        {
            string content = "";

            content += GetTab(1) + "static void " + GenerateReceviceFunctionName(type) + "(SyncSession session ,ProtocolRequestBase e)\n";
            content += GetTab(1) + "{\n";

            content += GenerateAnalysisClassContent(2, type, "msg", "e.m_data");

            content += GetTab(2) + "\n";

            content += GetTab(2) + "EventService.DispatchTypeEvent(session,msg);\n";

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

        static string GenerateAnalysisContent(int tab, FieldInfo field, string aimName, string sourceName)
        {
            string content = "";

            if (field.FieldType != typeof(Vector3))
            {
                if (field.FieldType == typeof(int))
                {
                    content += GetTab(tab) + aimName + "." + field.Name + " = " + "(int)" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                }
                else if (field.FieldType == typeof(bool))
                {
                    content += GetTab(tab) + aimName + "." + field.Name + " = " + "(bool)" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                }
                else if (field.FieldType == typeof(float))
                {
                    content += GetTab(tab) + aimName + "." + field.Name + " = " + "(float)(double)" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                }
                else if (field.FieldType.IsSubclassOf(typeof(Enum)))
                {
                    content += GetTab(tab) + aimName + "." + field.Name + " = " + "(" + field.FieldType.FullName + ")" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                }

                else if (field.FieldType == typeof(string))
                {
                    content += GetTab(tab) + aimName + "." + field.Name + " = " + "" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"].ToString();\n";
                }

                else if (field.FieldType.Name == typeof(List<>).Name)
                {
                    Type tmp = field.FieldType.GetGenericArguments()[0];
                    if (isBaseType(tmp))
                    {
                        content += GetTab(tab) + aimName + "." + field.Name + " = " + "(List<" + tmp.Name + ">)" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                    }
                    else
                    {
                        content += GetTab(tab) + "{\n";

                        content += GetTab(tab + 1) + "List<Dictionary<string, object>> data" + tab + " = (List<Dictionary<string, object>>)" + sourceName + "[\"" + GenerateProtocolFieldName(field) + "\"];\n";
                        content += GetTab(tab + 1) + "List<" + tmp + "> list" + tab + " = new List<" + tmp + ">();\n";
                        content += GetTab(tab + 1) + "for (int i" + tab + " = 0; i" + tab + " < data" + tab + ".Count; i" + tab + "++)\n";
                        content += GetTab(tab + 1) + "{\n";

                        content += GenerateAnalysisClassContent(tab + 2, tmp, "tmp" + tab + "", "data" + tab + "[i" + tab + "]");

                        content += GetTab(tab + 2) + "list" + tab + ".Add(tmp" + tab + ");\n";

                        content += GetTab(tab + 1) + "}\n";


                        content += GetTab(tab + 1) + aimName + "." + field.Name + " =  list" + tab + ";\n";
                        content += GetTab(tab) + "}\n";
                    }
                }

                else
                {
                    content += GetTab(tab) + "{\n";
                    content += GenerateAnalysisClassContent(tab + 1, field.FieldType, "tmp" + tab + "", sourceName);

                    content += GetTab(tab + 1) + aimName + "." + field.Name + " = " + "tmp" + tab + ";\n";

                    content += GetTab(tab) + "}\n";
                }
            }
            else
            {
                content += GetTab(tab) + aimName + "." + field.Name + " = new Vector3(); \n";
                content += GetTab(tab) + aimName + "." + field.Name + ".x = (float)(double)" + sourceName + "[\"" + field.Name.ToLower() + "x\"];\n";
                content += GetTab(tab) + aimName + "." + field.Name + ".y = (float)(double)" + sourceName + "[\"" + field.Name.ToLower() + "y\"];\n";
                content += GetTab(tab) + aimName + "." + field.Name + ".z = (float)(double)" + sourceName + "[\"" + field.Name.ToLower() + "z\"];\n";
            }

            return content;
        }

        static string GenerateAnalysisClassContent(int tab, Type type, string aimName, string sourceName)
        {
            string content = "";

            content += GetTab(tab) + type.FullName + " " + aimName + " = new " + type.FullName + "();\n";

            FieldInfo[] fields = type.GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                content += GenerateAnalysisContent(tab, fields[i], aimName, sourceName);
            }

            return content;
        }

        static bool isBaseType(Type type)
        {
            if (type == typeof(int))
            {
                return true;
            }
            else if (type == typeof(bool))
            {
                return true;
            }

            else if (type == typeof(float))
            {
                return true;
            }

            else if (type.IsSubclassOf(typeof(Enum)))
            {
                return true;
            }

            else if (type == typeof(string))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 清空生成代码
        public static void ClearAnalysisCode()
        {
            string SavePath = Application.dataPath + "/Script/" + PathName;
            FileTool.DeleteDirectory(SavePath);
            //AssetDatabase.Refresh();
        }

        public static void GenerateEmptyAnalysisCode()
        {
            msgList.Clear();

            string csharpContent = GenerateCSharpContent();

            string SavePath = ProjectPath + "/" + PathName + "/" + AnalysisCodeName + ".cs";
            FileTool.WriteStringByFile(SavePath, csharpContent);
        }
        #endregion
    }
}
