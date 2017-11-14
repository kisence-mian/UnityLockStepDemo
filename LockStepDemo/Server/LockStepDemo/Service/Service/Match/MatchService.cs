using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MatchService : ServiceBase
{
    const int roomPeopleNum = 2;
    List<Player> matchList = new List<Player>(); //匹配列表

    public override void OnInit()
    {
        EventService.AddTypeEvent<PlayerMatchMsg_s>(ReceviceMatchMsg);
    }

    void ReceviceMatchMsg(SyncSession session, PlayerMatchMsg_s msg)
    {
        Debug.Log("ReceviceMatchMsg");

        if (session.player == null)
        {
            Debug.Log("玩家未登录 " + session.SessionID);
            return;
        }

        if (msg.isCancel)
        {
            CancelMatch(session.player);
        }
        else
        {
            Match(session.player);
        }
    }

    public override void OnPlayerLogout(Player player)
    {
        CancelMatch(player);
    }

    void CancelMatch(Player player)
    {
        if(matchList.Contains(player))
        {
            matchList.Remove(player);
        }
    }

    void Match(Player player)
    {
        if (!matchList.Contains(player))
        {
            matchList.Add(player);

            PlayerMatchMsg_c msg = new PlayerMatchMsg_c();
            msg.predictTime = 300;
            msg.isMatched = false;

            ProtocolAnalysisService.SendMsg(player.session,msg);

            //TODO 潜在的线程不安全隐患 ---> 公共的matchList
            if (matchList.Count >= roomPeopleNum)
            {
                //构造玩家列表
                Player[] tmp = new Player[roomPeopleNum];

                for (int i = 0; i < roomPeopleNum; i++)
                {
                    tmp[i] = matchList[i];
                }

                //将已匹配到的玩家从列表中删除
                for (int i = 0; i < roomPeopleNum; i++)
                {
                    matchList.Remove(tmp[i]);
                }

                StartGame(tmp);
            }
        }
        else
        {
            Debug.Log("已经在匹配队列了！");
        }
    }

    void StartGame(Player[] players)
    {
        WorldBase world = WorldManager.CreateWorld<DemoWorld>();

        //TODO 模式选择没用了
        world.SyncRule = SyncRule.Status;

        world.m_RandomSeed = new Random().Next(); //随机一个种子

        for (int i = 0; i < players.Length; i++)
        {
            ConnectionComponent conn = new ConnectionComponent();
            conn.m_session = players[i].session;
            conn.m_playerID = players[i].playerID;

            SyncComponent sc = new SyncComponent();

            world.CreateEntityImmediately("Player"+ players[i].playerID, conn, sc);

            players[i].session.m_connect = conn;

            world.eventSystem.DispatchEvent(ServiceEventDefine.c_playerJoin, conn.Entity);

            Debug.Log("Send Game Start");
            //派发游戏开始消息
            PlayerMatchMsg_c msg = new PlayerMatchMsg_c();
            msg.predictTime = 0;
            msg.isMatched = true;

            ProtocolAnalysisService.SendMsg(players[i].session, msg);
        }

        world.IsStart = true;
    }
}
