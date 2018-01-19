using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Debug
{
    static ILog s_logger;
    static bool s_isDebug;

    public static void SetLogger(ILog logger, bool isDebug)
    {
        s_isDebug = isDebug;
        s_logger = logger;
    }

    public static void Log(object content)
    {
        Log(content.ToString());
    }

    public static void Log(string content,bool isStackTrace = false)
    {
        if(isStackTrace)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            content +="\n"+ st.ToString();
        }

        if (s_logger == null)
        {
            Console.WriteLine("Debug not is init! Log:" + content);
            return;
        }

        if (s_isDebug)
        {
            Console.WriteLine(content);
        }

        s_logger.Debug(content);
    }


    public static void LogError(object content)
    {
        Log(content.ToString());
    }
    public static void LogError(string content)
    {
        if (s_logger == null)
        {
            Console.WriteLine("Debug not is init! Error:" + content);
            return;
        }

        if (s_isDebug)
        {
            Console.WriteLine("------------------------------ERROR BEGIN----------------------------");
            Console.WriteLine(content + "\n" + new System.Diagnostics.StackTrace().ToString());
            Console.WriteLine("------------------------------ ERROR END ----------------------------");
        }
        s_logger.Error(content);
    }

    public static void LogWarning(string content, bool isStackTrace = false)
    {
        if (isStackTrace)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            content += "\n" + st.ToString();
        }

        if (s_logger == null)
        {
            Console.WriteLine("Debug not is init! Warning:" + content);
            return;
        }

        if (s_isDebug)
        {
            Console.WriteLine("------------------------------WARN BEGIN----------------------------");
            Console.WriteLine(content);
            Console.WriteLine("------------------------------ WARN END ----------------------------");
        }
        s_logger.Warn(content);
    }

    public static void DrawRay(Vector3 pos, Vector3 dir,Color col,float time)
    {

    }
    public static void DrawLine(Vector3 start,Vector3 end,Color col,float time)
    {

    }

    public static void DrawLine(Vector3 start, Vector3 end, Color col)
    {

    }
}
