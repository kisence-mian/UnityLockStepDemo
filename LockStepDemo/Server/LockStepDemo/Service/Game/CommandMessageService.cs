using DeJson;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommandMessageService<T> where T : PlayerCommandBase, new()
{
    public static void Init()
    {
        EventService.AddTypeEvent<T>(ReceviceSyncMsg);
        EventService.AddTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
    }

    public static void Dispose()
    {
        EventService.RemoveTypeEvent<T>(ReceviceSyncMsg);
        EventService.RemoveTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
    }

    static Deserializer deserializer = new Deserializer();
    static void ReceviceSyncMsg(SyncSession session, T msg)
    {
        //Debug.Log("ReceviceSyncMsg " + msg.id);

        ConnectionComponent connectComp = session.m_connect;
        WorldBase world = session.m_connect.Entity.World;
        if (connectComp != null)
        {
            if (msg.frame > world.FrameCount)
            {
                //消息确认
                AffirmMsg amsg = new AffirmMsg();
                amsg.frame = msg.frame;
                amsg.time = msg.time;

                BroadcastCommand(world, connectComp, msg,false);

                ProtocolAnalysisService.SendMsg(session, amsg);

                connectComp.m_commandList.Add(msg);
                connectComp.lastInputFrame = msg.frame;
            }
            else
            {
                //把玩家的这次上报当做最新的操作并转发
                Debug.Log("帧数落后  world.FrameCount: " + world.FrameCount + " msg frame:" + msg.frame + " 预测列表计数 " + connectComp.m_forecastList.Count);
                //Debug.Log("接收玩家数据 " + Serializer.Serialize(msg));
                connectComp.m_lastInputCache = msg;
                connectComp.lastInputFrame = world.FrameCount;

                //并且让这个玩家提前
                PursueMsg pmsg = new PursueMsg();
                pmsg.id = msg.id;
                pmsg.recalcFrame = msg.frame;
                pmsg.frame = world.FrameCount;
                pmsg.advanceCount = CalcAdvanceFrame(connectComp);
                pmsg.serverTime = ServiceTime.GetServiceTime();

                ProtocolAnalysisService.SendMsg(session, pmsg);
            }
        }
    }

    static void ReceviceAffirmMsg(SyncSession session, AffirmMsg msg)
    {
        ConnectionComponent commandComp = session.m_connect;
        commandComp.ClearForecast(msg.frame);

        int nowTime = ServiceTime.GetServiceTime();
        commandComp.rtt = nowTime - msg.time;

        //Debug.Log("rtt " + commandComp.rtt);
    }

    static int CalcAdvanceFrame(ConnectionComponent connect)
    {
        return 2;

        int frame = connect.rtt / UpdateEngine.IntervalTime + 1;

        return frame;
    }

    static void BroadcastCommand(WorldBase world, ConnectionComponent connectComp, T cmd, bool includeSelf)
    {
        //TODO 与预测一致不广播节约带宽
        List<EntityBase> list = world.GetEntiyList(new string[] { "ConnectionComponent" });

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cp = list[i].GetComp<ConnectionComponent>();
            if (!(includeSelf && cp != connectComp))
            {
                ProtocolAnalysisService.SendMsg(cp.m_session, cmd);
            }
        }
    }
}
