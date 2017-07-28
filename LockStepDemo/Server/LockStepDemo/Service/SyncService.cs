using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using LockStepDemo.Protocol;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Logger.Debug("SyncService Setup");

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Logger.Debug("SyncService OnStarted");

            base.OnStarted();
        }

        protected override void OnSystemMessageReceived(string messageType, object messageData)
        {
            base.OnSystemMessageReceived(messageType, messageData);
        }

        
    }
}
