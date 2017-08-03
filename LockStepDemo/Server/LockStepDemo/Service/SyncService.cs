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
        int updateInterval = 1000; //世界更新间隔ms

        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            //TODO 读取配置设置isDebug
            Debug.SetLogger(Logger,true);
            Debug.Log("SyncService Setup");

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Debug.Log("SyncService OnStarted");

            WorldManager.CreateWorld<DemoWorld>();
            UpdateEngine.Init(updateInterval);

            base.OnStarted();
        }

        protected override bool RegisterSession(string sessionID, SyncSession appSession)
        {
            Debug.Log("SyncService RegisterSession");

            return base.RegisterSession(sessionID, appSession);
        }

        protected override SyncSession CreateAppSession(ISocketSession socketSession)
        {
            Debug.Log("SyncService CreateAppSession");

            return base.CreateAppSession(socketSession);
        }

        protected override void OnSessionClosed(SyncSession session, CloseReason reason)
        {
            Debug.Log("SyncService OnSessionClosed");

            base.OnSessionClosed(session, reason);
        }

        protected override void OnNewSessionConnected(SyncSession session)
        {
            Debug.Log("SyncService OnNewSessionConnected");

            base.OnNewSessionConnected(session);
        }
    }
}
