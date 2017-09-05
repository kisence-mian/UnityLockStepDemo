using LockStepDemo.Service;
using Protocol;
using System;

namespace LockStepDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(FileTool.ReadStringByFile(Environment.CurrentDirectory + "/Network/" + ProtocolReceiveFilter.c_ProtocolFileName + ".txt"));

            try
            {
                var appServer = new SyncService();

                //Setup the appServer
                if (!appServer.Setup(7500)) //Setup with listening port
                {
                    Console.WriteLine("Failed to setup!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine();

                //Try to start the appServer
                if (!appServer.Start())
                {
                    Console.WriteLine("Failed to start!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("The server started successfully, press key 'q' to stop it!");

                while (Console.ReadKey().KeyChar != 'q')
                {
                    Console.WriteLine();
                    continue;
                }

                //Stop the appServer
                appServer.Stop();

                Console.WriteLine("The server was stopped!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                Console.ReadKey();
                Console.ReadKey();
            }
        }
    }
}
