using DeJson;
using LockStepDemo.Event;
using LockStepDemo.ServiceLogic;
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
                            Debug.Log("Push player Command");
                            ProtocolAnalysisService.SendMsg(cp.m_session, msg);
                        }
                    }
                }
                else
                {
                    //TODO 潜在的不同步威胁
                    //发送给玩家自己 服务器给他预测的操作
                    Debug.Log("帧数落后 丢弃玩家操作 world.FrameCount: " + world.FrameCount + " msg frame:" + msg.frame);

                    commandComp.m_lastInputCache = comp;
                }
            }
        }
    }
}
