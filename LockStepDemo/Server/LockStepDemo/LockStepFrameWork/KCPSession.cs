using Protocol;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KCPSession : AppSession<KCPSession, ProtocolRequestBase>
{
    public Player player;
    public ConnectionComponent m_connect;
    public KCP m_kcp;
    //public EntityBase m_entity;

    protected override void OnSessionStarted()
    {
        base.OnSessionStarted();
        Debug.Log("OnSessionStarted ");

        UInt32 conv = 0;
        //KCP.ikcp_decode32u(buf, 0, ref conv);

        init_kcp(conv);
    }

    void init_kcp(UInt32 conv)
    {
        m_kcp = new KCP(conv, (byte[] buf, int size) =>
        {
            base.Send(buf,0,buf.Length);
        });

        m_kcp.NoDelay(1, 10, 2, 1);
    }


    protected override void HandleUnknownRequest(ProtocolRequestBase requestInfo)
    {
        try
        {
            //解析并派发
            //ProtocolAnalysisService.AnalysisAndDispatchMessage(this, requestInfo);
        }
        catch (Exception e)
        {
            Debug.LogError("AnalysisAndDispatchMessage :" + requestInfo.Key + "\nException: " + e.ToString());
        }
    }

    protected override void OnSessionClosed(CloseReason reason)
    {
        base.OnSessionClosed(reason);
    }
}
