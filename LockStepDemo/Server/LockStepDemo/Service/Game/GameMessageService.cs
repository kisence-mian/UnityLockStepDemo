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
            WorldBase world = session.m_entity.World;
            if (commandComp != null)
            {
                PlayerCommandBase comp = msg;
                comp.frame = msg.frame;

                if (msg.frame > world.FrameCount)
                {
                    commandComp.m_commandList.Add(comp);
                    //TODO 广播操作
                    
                }
                else
                {
                    //TODO 潜在的不同步威胁
                    Debug.Log("帧数落后 丢弃玩家操作 world.FrameCount: " + world.FrameCount + " msg frame:" + msg.frame);

                    commandComp.m_lastInputCache = comp;
                }
            }
        }
    }
}
