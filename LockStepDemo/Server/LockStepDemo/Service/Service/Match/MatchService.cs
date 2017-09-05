using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MatchService
{
    const int roomPeopleNum = 2;
    List<Player> matchList = new List<Player>(); //匹配列表

    public void Init()
    {
        EventService.AddTypeEvent<PlayerMatchMsg_s>(ReceviceMatchMsg);
    }

    void ReceviceMatchMsg(SyncSession session, PlayerMatchMsg_s msg)
    {
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
            if (matchList.Count > roomPeopleNum)
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
        world.IsStart = true;
        world.SyncRule = SyncRule.Status;

        for (int i = 0; i < players.Length; i++)
        {
            ConnectionComponent conn = new ConnectionComponent();
            conn.m_session = players[i].session;

            world.CreateEntity(conn);

            players[i].session.m_connect = conn;

            world.CreateEntity();

            PlayerMatchMsg_c msg = new PlayerMatchMsg_c();
            msg.predictTime = 0;
            msg.isMatched = true;

            ProtocolAnalysisService.SendMsg(players[i].session, msg);
        }
    }
}
