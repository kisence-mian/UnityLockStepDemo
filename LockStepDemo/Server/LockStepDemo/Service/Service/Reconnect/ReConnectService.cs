using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ReConnectService
{
    //掉线的玩家列表
    public Dictionary<string, ConnectionComponent> m_disConnectDict = new Dictionary<string, ConnectionComponent>();

    public void Init()
    {
        EventService.AddTypeEvent<Player>(OnPlayerLogin);
    }

    public void OnPlayerLogin(SyncSession session,Player player)
    {
        if(m_disConnectDict.ContainsKey(player.ID))
        {
            //重连吧少年
            PlayerMatchMsg_c msg = new PlayerMatchMsg_c();
            msg.isMatched = true;
            msg.predictTime = 0;

            ConnectionComponent conn = m_disConnectDict[player.ID];
            conn.m_session = session;

            conn.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerJoin, conn.Entity);

            ProtocolAnalysisService.SendMsg(session,msg);
        }
    }

    public void AddRecord(ConnectionComponent connect)
    {
        m_disConnectDict.Add(connect.m_session.player.ID, connect);

        connect.m_session = null;

        connect.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerExit, connect.Entity);
    }

    public void RemoveRecord(ConnectionComponent connect)
    {
        m_disConnectDict.Remove(connect.m_session.player.ID);
    }
}
