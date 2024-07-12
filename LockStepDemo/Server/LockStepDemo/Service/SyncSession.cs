using Protocol;
using SuperSocket.SocketBase;
using System;


public class SyncSession : AppSession<SyncSession, ProtocolRequestBase>
{
    public Player player;

    public WorldBase m_gameWorld;
    public ConnectionComponent m_connect;
    //public EntityBase m_entity;

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
