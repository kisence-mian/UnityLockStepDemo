using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using Protocol;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {

        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Console.WriteLine("SyncService Setup");
            Logger.Debug("SyncService Setup");

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Console.WriteLine("SyncService OnStarted");
            Logger.Debug("SyncService OnStarted");

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
