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
        ConnectionComponent commandComp = session.m_connect;
        WorldBase world = session.m_connect.Entity.World;
        if (commandComp != null)
        {
            if (msg.frame > world.FrameCount)
            {
                //消息确认
                AffirmMsg amsg = new AffirmMsg();
                amsg.frame = msg.frame;
                amsg.time = msg.time;

                commandComp.m_commandList.Add(msg);
                //TODO 与预测一致不广播节约带宽
                List<EntityBase> list = world.GetEntiyList(new string[] { "ConnectionComponent" });

                for (int i = 0; i < list.Count; i++)
                {
                    ConnectionComponent cp = list[i].GetComp<ConnectionComponent>();
                    if (cp != commandComp)
                    {
                        ProtocolAnalysisService.SendMsg(cp.m_session, msg);
                    }
                }
            }
            else
            {
                //潜在的不同步威胁
                Debug.Log("帧数落后 丢弃玩家操作 world.FrameCount: " + world.FrameCount + " msg frame:" + msg.frame + " 预测列表计数 " + commandComp.m_forecastList.Count);
                //Debug.Log("接收玩家数据 " + Serializer.Serialize(msg));
                commandComp.m_lastInputCache = msg;

                //并且让这个玩家提前
                PursueMsg pmsg = new PursueMsg();
                pmsg.id = msg.id;
                pmsg.recalcFrame = msg.frame;
                pmsg.frame = world.FrameCount;
                pmsg.advanceCount = CalcAdvanceFrame(commandComp);
                pmsg.serverTime = ServiceTime.GetServiceTime();
                pmsg.m_commandList = new List<string>();
                //发送给玩家自己 服务器给他预测的操作，
                for (int i = 0; i < commandComp.m_forecastList.Count; i++)
                {
                    //Debug.Log("发送预测数据-> " + Serializer.Serialize(commandComp.m_forecastList[i]));
                    pmsg.m_commandList.Add(Serializer.Serialize(commandComp.m_forecastList[i]));
                }
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

        Debug.Log("ping " + commandComp.rtt);
    }

    static int CalcAdvanceFrame(ConnectionComponent connect)
    {
        int frame = connect.rtt / UpdateEngine.IntervalTime + 1;

        return frame;
    }
}
