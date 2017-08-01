using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Debug
{
    static ILog s_logger;
    static bool s_isDebug;

    public static void SetLogger(ILog logger, bool isDebug)
    {
        s_logger = logger;
    }

    public static void Log(string content)
    {
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
            Console.WriteLine(content);
            Console.WriteLine("------------------------------ ERROR END ----------------------------");
        }
        s_logger.Error(content);
    }

    public static void LogWarning(string content)
    {
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
}
