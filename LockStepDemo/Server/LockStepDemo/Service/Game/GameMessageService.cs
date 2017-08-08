using DeJson;
using LockStepDemo.Event;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Service.Game
{
    class GameMessageService
    {
        public static void Init()
        {
            EventService.AddTypeEvent<ChangeComponentMsg>(ReceviceSyncMsg);
        }

        public static void Dispose()
        {
            EventService.RemoveTypeEvent<ChangeComponentMsg>(ReceviceSyncMsg);
        }

        static Deserializer deserializer = new Deserializer();
        static void ReceviceSyncMsg(SyncSession session, ChangeComponentMsg msg)
        {
            WorldBase world = session.m_gameWorld;
            if (world != null)
            {
                Type type = Type.GetType(msg.info.m_compName);
                if(type!= null && type.IsSubclassOf(typeof(PlayerCommandBase)))
                {
                    ComponentBase comp = (ComponentBase)deserializer.Deserialize(msg.info.m_compName, msg.info.content);
                }
            }
            else
            {

            }
        }
    }
}
