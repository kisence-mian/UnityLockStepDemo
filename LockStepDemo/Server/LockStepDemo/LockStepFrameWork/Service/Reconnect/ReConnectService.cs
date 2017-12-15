using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

public class ReConnectService : ServiceBase
{
    //掉线的玩家列表
    public Dictionary<string, ConnectionComponent> m_disConnectDict = new Dictionary<string, ConnectionComponent>();

    public override  void OnInit(IServerConfig config)
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
            msg.matchInfo = new List<MatchPlayerInfo>();
            ProtocolAnalysisService.SendMsg(player.session, msg);

            ConnectionComponent conn = m_disConnectDict[player.playerID];
            conn.m_session = player.session;
            conn.m_session.m_connect = conn;

            m_disConnectDict.Remove(player.playerID);

            conn.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerReconnect, conn.Entity);
        }
    }

    public override void OnPlayerLogout(Player player)
    {
        if(player.session.m_connect != null)
        {
            AddRecord(player.session.m_connect);
        }
        //RemoveRecord(player);
    }

    //public override void OnSessionClose(SyncSession session, CloseReason reason)
    //{
    //    Debug.Log("ReConnectService OnSessionClose ");

    //    //掉线玩家维护一个id与world的映射，用以重连
    //    if (session.m_connect != null)
    //    {
    //        AddRecord(session.m_connect);
    //        //m_world.DestroyEntity(session.m_connect.Entity.ID);
    //    }
    //}

    public void AddRecord(ConnectionComponent connect)
    {
        if (!connect.Entity.World.IsFinish)
        {
            m_disConnectDict.Add(connect.m_session.player.playerID, connect);

            connect.m_session = null;
            connect.Entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_playerExit, connect.Entity);
        }
    }

    public void OnGameFinsih(params object[] objs)
    {
        WorldBase world = (WorldBase)objs[0];

        List<EntityBase> list = world.GetEntiyList(new string[] { "ConnectionComponent" });

        //移除所有的重连信息
        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();
            m_disConnectDict.Remove(cc.m_playerID);

            RemoveRecord(cc.m_playerID);
        }
    }

    public void RemoveRecord(string playerID)
    {
        if (m_disConnectDict.ContainsKey(playerID))
        {
            m_disConnectDict.Remove(playerID);
        }
    }
}
