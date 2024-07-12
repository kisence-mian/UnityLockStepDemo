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

        //消息确认
        AffirmMsg amsg = new AffirmMsg();
        amsg.frame = msg.frame;
        amsg.time = msg.time;
        ProtocolAnalysisService.SendMsg(session, amsg);

        ConnectionComponent connectComp = session.m_connect;

        if (connectComp != null)
        {
            WorldBase world = connectComp.Entity.World;

            if (msg.frame > world.FrameCount)
            {
                BroadcastCommand(world, connectComp, msg,false);

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

        int nowTime = ServiceTime.GetServiceTime();
        commandComp.rtt = nowTime - msg.time;

        try
        {
            if (commandComp.unConfirmFrame.Contains(msg.frame))
            {
                commandComp.unConfirmFrame.Remove(msg.frame);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(msg.frame + " error: " + e.ToString());
        }


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
                //补发未确认的帧
                for (int j = 0; j < cp.unConfirmFrame.Count; j++)
                {
                    CommandComponent msg = (CommandComponent)cp.GetCommand(cp.unConfirmFrame[j]);
                    ProtocolAnalysisService.SendMsg(cp.m_session, msg);
                }

                cp.unConfirmFrame.Add(cmd.frame);
                ProtocolAnalysisService.SendMsg(cp.m_session, cmd);
            }
        }
    }
}
