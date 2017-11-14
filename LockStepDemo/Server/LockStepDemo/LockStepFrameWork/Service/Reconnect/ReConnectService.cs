using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

public class ReConnectService : ServiceBase
{
    //掉线的玩家列表
    public Dictionary<string, ConnectionComponent> m_disConnectDict = new Dictionary<string, ConnectionComponent>();

    public override  void OnInit()
    {
        EventService.AddEvent(ServiceEventDefine.ServiceEvent.GameFinsih, OnGameFinsih);
    }

    public override void OnPlayerLogin(Player player)
    {
        if(m_disConnectDict.ContainsKey(player.playerID))
        {
            //重连吧少年
            PlayerMatchMsg_c msg = new PlayerMatchMsg_c();
            msg.isMatched = true;
            msg.predictTime = 0;

            ConnectionComponent conn = m_disConnectDict[player.playerID];
            conn.m_session = player.session;
            conn.m_session.m_connect = conn;

            m_disConnectDict.Remove(player.playerID);

            conn.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerJoin, conn.Entity);

            ProtocolAnalysisService.SendMsg(player.session,msg);
        }
    }

    public override void OnSessionClose(SyncSession session, CloseReason reason)
    {
        //掉线玩家维护一个id与world的映射，用以重连
        if (session.m_connect != null)
        {
            AddRecord(session.m_connect);
            //m_world.DestroyEntity(session.m_connect.Entity.ID);
        }
    }

    public void AddRecord(ConnectionComponent connect)
    {
        if (!connect.Entity.World.isFinish)
        {
            m_disConnectDict.Add(connect.m_session.player.playerID, connect);

            connect.m_session = null;

            connect.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerExit, connect.Entity);
        }
    }

    public void RemoveRecord(ConnectionComponent connect)
    {
        m_disConnectDict.Remove(connect.m_session.player.playerID);
    }

    public void OnGameFinsih(params object[] objs)
    {
        WorldBase world = (WorldBase)objs[0];

        world.eventSystem.DispatchEvent(GameUtils.c_scoreChange, null);
        List<PlayerComponent> rankList = world.GetSingletonComp<RankComponent>().rankList;
        int diamond = rankList.Count;

        for (int i = 0; i < rankList.Count; i++)
        {
            string playerID = rankList[i].Entity.GetComp<ConnectionComponent>().m_playerID;

            if (playerID != null && m_disConnectDict.ContainsKey(playerID))
            {
                m_disConnectDict.Remove(playerID);
            }
        }
    }
}
