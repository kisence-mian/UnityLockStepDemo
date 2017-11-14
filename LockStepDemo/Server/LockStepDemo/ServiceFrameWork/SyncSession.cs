using Protocol;
using SuperSocket.SocketBase;
using System;


public class SyncSession : AppSession<SyncSession, ProtocolRequestBase>
{
    public Player player;

    public WorldBase m_gameWorld;
    public ConnectionComponent m_connect;
    //public KCP m_kcp;
    //public EntityBase m_entity;

    protected override void OnSessionStarted()
    {
        base.OnSessionStarted();
        Debug.Log("OnSessionStarted ");

        //m_kcp = new KCP((UInt32)new Random((int)DateTime.Now.Ticks).Next(1, Int32.MaxValue), (byte[] buf, int size) =>
        //{
        //    Send(buf,0, size);
        //});
    }

    protected override void HandleUnknownRequest(ProtocolRequestBase requestInfo)
    {
        try
        {
            //解析并派发
            ProtocolAnalysisService.AnalysisAndDispatchMessage(this, requestInfo);
        }
        catch(Exception e)
        {
            Debug.LogError("AnalysisAndDispatchMessage :" + requestInfo.Key + "\nException: " + e.ToString());
        }
    }

    protected override void OnSessionClosed(CloseReason reason)
    {
        base.OnSessionClosed(reason);
    }
}
