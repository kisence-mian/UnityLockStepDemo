using Protocol;
using SuperSocket.SocketBase;
using System;

namespace LockStepDemo.Service
{
    public class SyncSession : AppSession<SyncSession, ProtocolRequestBase>
    {
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();

            Debug.Log("OnSessionStarted ");
        }

        protected override void HandleUnknownRequest(ProtocolRequestBase requestInfo)
        {
            //解析并派发
            //ProtocolAnalysisService.AnalysisAndDispatchMessage(this, requestInfo);
        }
    }
}
