using LockStepDemo.Service;
using Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
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
                var bootstrap = BootstrapFactory.CreateBootstrap();

                if (!bootstrap.Initialize())
                {
                    Console.WriteLine("Failed to initialize!");
                    Console.ReadKey();
                    return;
                }

                var result = bootstrap.Start();

                Console.WriteLine("Start result: {0}!", result);

                if (result == StartResult.Failed)
                {
                    Console.WriteLine("Failed to start!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Press key 'q' to stop it!");

                while (Console.ReadKey().KeyChar != 'q')
                {
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();

                //Stop the appServer
                bootstrap.Stop();

                Console.WriteLine("The server was stopped!");
                Console.ReadKey();



                //var appServer = new SyncService();

                ////Setup the appServer
                //if (!appServer.Setup(7500)) //Setup with listening port
                //{
                //    Console.WriteLine("Failed to setup!");
                //    Console.ReadKey();
                //    return;
                //}

                //Console.WriteLine();

                ////Try to start the appServer
                //if (!appServer.Start())
                //{
                //    Console.WriteLine("Failed to start!");
                //    Console.ReadKey();
                //    return;
                //}

                //Console.WriteLine("The server started successfully, press key 'q' to stop it!");

                //while (Console.ReadKey().KeyChar != 'q')
                //{
                //    Console.WriteLine();
                //    continue;
                //}

                ////Stop the appServer
                //appServer.Stop();

                //Console.WriteLine("The server was stopped!");
                //Console.ReadKey();
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
