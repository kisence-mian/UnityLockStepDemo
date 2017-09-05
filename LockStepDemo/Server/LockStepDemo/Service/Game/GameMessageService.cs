using DeJson;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Service.Game
{
    class GameMessageService<T> where T : PlayerCommandBase, new()
    {
        public static void Init()
        {
            EventService.AddTypeEvent<T>(ReceviceSyncMsg);
        }

        public static void Dispose()
        {
            EventService.RemoveTypeEvent<T>(ReceviceSyncMsg);
        }

        static Deserializer deserializer = new Deserializer();
        static void ReceviceSyncMsg(SyncSession session, T msg)
        {
            ConnectionComponent commandComp = session.m_connect;
            WorldBase world = session.m_connect.Entity.World;
            if (commandComp != null)
            {
                PlayerCommandBase comp = msg;
                comp.frame = msg.frame;

                if (msg.frame > world.FrameCount)
                {
                    commandComp.m_commandList.Add(comp);
                    //TODO 与预测一致不广播节约带宽
                    List<EntityBase> list = world.GetEntiyList(new string[] { "ConnectionComponent" });

                    for (int i = 0; i < list.Count; i++)
                    {
                        ConnectionComponent cp = list[i].GetComp<ConnectionComponent>();
                        if(cp != commandComp)
                        {
                            ProtocolAnalysisService.SendMsg(cp.m_session, msg);
                        } 
                    }
                }
                else
                {
                    //潜在的不同步威胁
                    
                    Debug.Log("帧数落后 丢弃玩家操作 world.FrameCount: " + world.FrameCount + " msg frame:" + msg.frame);

                    //发送给玩家自己 服务器给他预测的操作，
                    for (int i = 0; i < commandComp.m_forecastList.Count; i++)
                    {
                        ProtocolAnalysisService.SendMsg(session, commandComp.m_forecastList[i]);
                    }
                    commandComp.m_forecastList.Clear();

                    //并且让这个玩家提前
                    commandComp.m_lastInputCache = comp;

                    PursueMsg pmsg = new PursueMsg();
                    pmsg.frame = world.FrameCount;
                    pmsg.advanceCount = 1;

                    ProtocolAnalysisService.SendMsg(session, pmsg);
                }
            }
        }
    }
}
