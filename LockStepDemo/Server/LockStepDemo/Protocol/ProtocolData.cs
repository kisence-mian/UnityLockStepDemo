using LockStepDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Protocol
{
    class ProtocolData
    {
        public const int TYPE_string = 1;
        public const int TYPE_int32 = 2;
        public const int TYPE_double = 3;
        public const int TYPE_bool = 4;
        public const int TYPE_custom = 5;

        public const int TYPE_int8 = 6;
        public const int TYPE_int16 = 7;
        public const int RT_repeated = 1;
        public const int RT_equired = 0;

        public const string c_ProtocolFileName = "ProtocolInfo";
        public const string c_methodNameInfoFileName = "MethodInfo";

        static Dictionary<int, string> s_methodNameInfo;
        static Dictionary<string, int> s_methodIndexInfo;
        static Dictionary<string, List<Dictionary<string, object>>> s_protocolInfo;

        public static Dictionary<int, string> MethodNameInfo {
            get {

                if(s_methodNameInfo == null)
                {
                    Init();
                }

                return s_methodNameInfo;
            }
            set => s_methodNameInfo = value; }
        public static Dictionary<string, int> MethodIndexInfo {
            get {

                if (s_methodIndexInfo == null)
                {
                    Init();
                }

                return s_methodIndexInfo;
            }
            set => s_methodIndexInfo = value; }
        public static Dictionary<string, List<Dictionary<string, object>>> ProtocolInfo {
            get {

                if (s_protocolInfo == null)
                {
                    Init();
                }

                return s_protocolInfo;
            }
            set => s_protocolInfo = value; }

        static void Init()
        {
            s_protocolInfo = ReadProtocolInfo(FileTool.ReadStringByFile(Environment.CurrentDirectory + "/Network/" + c_ProtocolFileName + ".txt"));
            ReadMethodNameInfo(
                out s_methodNameInfo,
                out s_methodIndexInfo,
                FileTool.ReadStringByFile(Environment.CurrentDirectory + "/Network/" + c_methodNameInfoFileName + ".txt"));
        }
   

    #region 读取protocol信息

    public static Dictionary<string, List<Dictionary<string, object>>> ReadProtocolInfo(string content)
    {
        Dictionary<string, List<Dictionary<string, object>>> protocolInfo = new Dictionary<string, List<Dictionary<string, object>>>();

        AnalysisProtocolStatus currentStatus = AnalysisProtocolStatus.None;
        List<Dictionary<string, object>> msgInfo = new List<Dictionary<string, object>>();
        Regex rgx = new Regex(@"^message\s(\w+)");

        string[] lines = content.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string currentLine = lines[i];

            if (currentStatus == AnalysisProtocolStatus.None)
            {
                if (currentLine.Contains("message"))
                {
                    string msgName = rgx.Match(currentLine).Groups[1].Value;

                    msgInfo = new List<Dictionary<string, object>>();

                    if (protocolInfo.ContainsKey(msgName))
                    {
                        //Logger.Debug("protocol 有重复的Key! :" + msgName);
                        //Debug.LogError("protocol 有重复的Key! :" + msgName);
                    }
                    else
                    {
                        protocolInfo.Add(msgName, msgInfo);
                    }

                    currentStatus = AnalysisProtocolStatus.Message;
                }
            }
            else
            {
                if (currentLine.Contains("}"))
                {
                    currentStatus = AnalysisProtocolStatus.None;
                    msgInfo = null;
                }
                else
                {
                    if (currentLine.Contains("required"))
                    {
                        Dictionary<string, object> currentFeidInfo = new Dictionary<string, object>();

                        currentFeidInfo.Add("spl", RT_equired);

                        AddName(currentLine, currentFeidInfo);
                        AddType(currentLine, currentFeidInfo);

                        msgInfo.Add(currentFeidInfo);
                    }
                    else if (currentLine.Contains("repeated"))
                    {
                        Dictionary<string, object> currentFeidInfo = new Dictionary<string, object>();

                        currentFeidInfo.Add("spl", RT_repeated);

                        AddName(currentLine, currentFeidInfo);
                        AddType(currentLine, currentFeidInfo);

                        msgInfo.Add(currentFeidInfo);
                    }
                }
            }
        }

        return protocolInfo;
    }

    static Regex m_TypeRgx = new Regex(@"^\s+\w+\s+(\w+)\s+\w+");

    static void AddType(string currentLine, Dictionary<string, object> currentFeidInfo)
    {
        if (currentLine.Contains("int32"))
        {
            currentFeidInfo.Add("type", TYPE_int32);
        }
        else if (currentLine.Contains("int16"))
        {
            currentFeidInfo.Add("type", TYPE_int16);
        }
        else if (currentLine.Contains("int8"))
        {
            currentFeidInfo.Add("type", TYPE_int8);
        }
        else if (currentLine.Contains("string"))
        {
            currentFeidInfo.Add("type", TYPE_string);
        }
        else if (currentLine.Contains("double"))
        {
            currentFeidInfo.Add("type", TYPE_double);
        }
        else if (currentLine.Contains("bool"))
        {
            currentFeidInfo.Add("type", TYPE_bool);
        }
        else
        {
            currentFeidInfo.Add("type", TYPE_custom);
            currentFeidInfo.Add("vp", m_TypeRgx.Match(currentLine).Groups[1].Value);
        }
    }

    static Regex m_NameRgx = new Regex(@"^\s+\w+\s+\w+\s+(\w+)");

    static void AddName(string currentLine, Dictionary<string, object> currentFeidInfo)
    {
        currentFeidInfo.Add("name", m_NameRgx.Match(currentLine).Groups[1].Value);
    }

    enum AnalysisProtocolStatus
    {
        None,
        Message
    }

    #endregion

    #region 读取消息号映射

    public static void ReadMethodNameInfo(out Dictionary<int, string> methodNameInfo, out Dictionary<string, int> methodIndexInfo, string content)
    {
        methodNameInfo = new Dictionary<int, string>();
        methodIndexInfo = new Dictionary<string, int>();

        Regex rgx = new Regex(@"^(\d+),(\w+)");

        string[] lines = content.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(","))
            {
                var res = rgx.Match(lines[i]);

                string index = res.Groups[1].Value;
                string indexName = res.Groups[2].Value;

                methodNameInfo.Add(int.Parse(index), indexName);
                methodIndexInfo.Add(indexName, int.Parse(index));
            }
        }
    }



        #endregion
    }
}
