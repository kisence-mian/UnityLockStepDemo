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
        EventService.AddTypeEvent<QueryCommand>(ReceviceQueryMsg);
        EventService.AddTypeEvent<VerificationMsg>(ReceviceVerificationMsg);
    }

    public static void Dispose()
    {
        EventService.RemoveTypeEvent<T>(ReceviceSyncMsg);
        EventService.RemoveTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        EventService.RemoveTypeEvent<VerificationMsg>(ReceviceVerificationMsg);
    }

    static Deserializer deserializer = new Deserializer();
    static void ReceviceSyncMsg(SyncSession session, T msg)
    {
        //Debug.Log("ReceviceSyncMsg " + msg.id + " content " +  Serializer.Serialize(msg));

        //消息确认
        AffirmMsg amsg = new AffirmMsg();
        amsg.index = msg.frame;
        amsg.time = msg.time;
        ProtocolAnalysisService.SendMsg(session, amsg);

        ConnectionComponent connectComp = session.m_connect;

        if (connectComp != null)
        {
            WorldBase world = connectComp.Entity.World;

            if (msg.frame > world.FrameCount)
            {
                connectComp.m_commandList.Add(msg);
                connectComp.lastInputFrame = msg.frame;

                BroadcastCommand(world, connectComp, msg, false);
            }
            else
            {
                //把玩家的这次上报当做最新的操作
                connectComp.m_lastInputCache = msg;
            }

            int aimFrame = world.FrameCount + CalcAdvanceFrame(connectComp);

            //Debug.Log("msg.frame " + msg.frame + " aimFrame " + aimFrame);

            if (msg.frame > aimFrame)
            {
                //过快

                if (msg.frame - aimFrame > 16)
                {
                    SendPursueMsg(connectComp, 0.5f);
                }

                if (msg.frame - aimFrame > 8)
                {
                    SendPursueMsg(connectComp, 0.65f);
                }

                else if (msg.frame - aimFrame > 4)
                {
                    SendPursueMsg(connectComp, 0.75f);
                }

                else if (msg.frame - aimFrame > 2)
                {
                    SendPursueMsg(connectComp, 0.85f);
                }

                else if(msg.frame - aimFrame > 0)
                {
                    SendPursueMsg(connectComp, 0.95f);
                }

                //适中
                else
                {
                    SendPursueMsg(connectComp, 1f);
                }
            }
            else
            {
                if (aimFrame - msg.frame > 32)
                {
                    SendPursueMsg(connectComp, 16f);
                }
                if (aimFrame - msg.frame > 16)
                {
                    SendPursueMsg(connectComp, 8f);
                }
                else if(aimFrame - msg.frame > 8)
                {
                    SendPursueMsg(connectComp, 4f);
                }
                else if (aimFrame - msg.frame > 4)
                {
                    SendPursueMsg(connectComp, 1.5f);
                }
                else if (aimFrame - msg.frame > 2)
                {
                    SendPursueMsg(connectComp, 1.25f);
                }
                else
                {
                    SendPursueMsg(connectComp, 1.05f);
                }
            }
        }
    }

    static void SendPursueMsg(ConnectionComponent connectComp, float speed)
    {
        if (connectComp.UpdateSpeed != speed)
        {
            PursueMsg pmsg = new PursueMsg();
            pmsg.updateSpeed = speed;

            pmsg.advanceCount = CalcAdvanceFrame(connectComp);

            connectComp.UpdateSpeed = speed;
            ProtocolAnalysisService.SendMsg(connectComp.m_session, pmsg);
        }
    }

    static void ReceviceAffirmMsg(SyncSession session, AffirmMsg msg)
    {
        ConnectionComponent commandComp = session.m_connect;

        //Debug.Log(" 收到确认消息 frame: " + msg.index + " id: " + commandComp.Entity.ID);

        int nowTime = ServiceTime.GetServiceTime();
        commandComp.rtt = nowTime - msg.time;
    }

    static void ReceviceQueryMsg(SyncSession session, QueryCommand msg)
    {
        ConnectionComponent connectComp = session.m_connect;

        if (connectComp != null)
        {
            WorldBase world = connectComp.Entity.World;

            EntityBase entity = world.GetEntity(msg.id);
            ConnectionComponent cc = entity.GetComp<ConnectionComponent>();

            T cmd = (T)cc.GetCommand(msg.frame);
            ProtocolAnalysisService.SendMsg(connectComp.m_session, cmd);
        }
    }

    static void ReceviceVerificationMsg(SyncSession session, VerificationMsg msg)
    {
        ConnectionComponent connectComp = session.m_connect;

        if (connectComp != null)
        {
            WorldBase world = connectComp.Entity.World;

            if(world.GetHash() == msg.hash)
            {

            }
            else
            {

            }
        }
    }

    static int CalcAdvanceFrame(ConnectionComponent connect)
    {
        //return 2;

        int frame = connect.rtt / UpdateEngine.IntervalTime + 1;
        frame = Math.Min(7, frame);

        frame = Math.Max(2, frame);

        //Debug.Log("RTT " + connect.rtt + " IntervalTime " + UpdateEngine.IntervalTime + " frame " + frame);

        return frame;
    }

    static void BroadcastCommand(WorldBase world, ConnectionComponent connectComp, T cmd, bool includeSelf)
    {
        cmd.time = ServiceTime.GetServiceTime();

        //TODO 与预测一致不广播节约带宽
        List<EntityBase> list = world.GetEntiyList(new string[] { "ConnectionComponent" });

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cp = list[i].GetComp<ConnectionComponent>();
            //if (!(includeSelf && cp != connectComp))
            {
                ProtocolAnalysisService.SendMsg(cp.m_session, cmd);
            }
        }
    }
}
