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
                //TODO 这里可能有问题
                //    //把玩家的这次上报当做最新的操作
                connectComp.m_lastInputCache = msg;
            }

            if (msg.frame > world.FrameCount)
            {
                //过快
                if (msg.frame - world.FrameCount > CalcAdvanceFrame(connectComp))
                {
                    SendPursueMsg(connectComp, 0.75f);
                }
                //适中
                else if (msg.frame - world.FrameCount == CalcAdvanceFrame(connectComp))
                {
                    SendPursueMsg(connectComp, 1f);
                }
            }
            else
            {
                if(world.FrameCount - msg.frame > 20)
                {
                    SendPursueMsg(connectComp, 8f);
                }
                else if(world.FrameCount - msg.frame > 10)
                {
                    SendPursueMsg(connectComp, 4f);
                }
                else
                {
                    SendPursueMsg(connectComp, 2f);
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

        //try
        //{
        //    lock (commandComp.m_unConfirmFrame)
        //    {
        //        if (commandComp.m_unConfirmFrame.ContainsKey(msg.index))
        //        {
        //            commandComp.m_unConfirmFrame.Remove(msg.index);
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(msg.index + " error: " + e.ToString());
        //}
    }

    static int CalcAdvanceFrame(ConnectionComponent connect)
    {
        //return 2;

        int frame = connect.rtt / UpdateEngine.IntervalTime + 1;

        frame = Math.Min(5, frame);

        return frame;
    }

    static void BroadcastCommand(WorldBase world, ConnectionComponent connectComp, T cmd, bool includeSelf)
    {
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
