using LockStepDemo.ServiceLogic;
using Protocol;
using SuperSocket.SocketBase;
using System;

namespace LockStepDemo.Service
{
    public class SyncSession : AppSession<SyncSession, ProtocolRequestBase>
    {
        public ConnectionComponent m_connect;
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
            Debug.Log("OnSessionStarted ");
        }

        protected override void HandleUnknownRequest(ProtocolRequestBase requestInfo)
        {
            //解析并派发
            ProtocolAnalysisService.AnalysisAndDispatchMessage(this, requestInfo);
        }
    }
}
