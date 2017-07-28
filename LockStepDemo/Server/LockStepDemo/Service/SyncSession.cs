using LockStepDemo.Protocol;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Service
{
    class SyncSession : AppSession<SyncSession, ProtocolRequestBase>
    {
        protected override void HandleUnknownRequest(ProtocolRequestBase requestInfo)
        {
            base.HandleUnknownRequest(requestInfo);

            Send()
        }

      
    }
}
