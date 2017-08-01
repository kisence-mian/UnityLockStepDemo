using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using Protocol;
using SuperSocket.SocketBase.Logging;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
#region 静态方法

        static bool isDebug = false;

        private static ILog s_logger;

        public static ILog Loger { get => s_logger;}

        public static void Log(string content)
        {

        }

        public static void Error(string content)
        {
            
        }

#endregion

        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Console.WriteLine("SyncService Setup");
            base.Logger.Debug("SyncService Setup");

            Debug.SetLogger(Logger,true);

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Console.WriteLine("SyncService OnStarted");
            base.Logger.Debug("SyncService OnStarted");

            base.OnStarted();
        }

        protected override bool RegisterSession(string sessionID, SyncSession appSession)
        {
            Console.WriteLine("SyncService RegisterSession");

            return base.RegisterSession(sessionID, appSession);
        }

        protected override SyncSession CreateAppSession(ISocketSession socketSession)
        {
            Console.WriteLine("SyncService CreateAppSession");

            return base.CreateAppSession(socketSession);
        }

        protected override void OnSessionClosed(SyncSession session, CloseReason reason)
        {
            Console.WriteLine("SyncService OnSessionClosed");

            base.OnSessionClosed(session, reason);
        }

        protected override void OnNewSessionConnected(SyncSession session)
        {
            Console.WriteLine("SyncService OnNewSessionConnected");

            base.OnNewSessionConnected(session);
        }

        protected override void OnSystemMessageReceived(string messageType, object messageData)
        {
            ProtocolRequestBase msg = (ProtocolRequestBase)messageData;

            Console.WriteLine(" msg.Key" + msg.Key);
        }

    }
}
