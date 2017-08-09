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
            ConnectionComponent commandComp = session.m_connect;
            if (commandComp != null)
            {
                Type type = Type.GetType(msg.info.m_compName);

                if (type!= null)
                {
                    PlayerCommandBase comp = (PlayerCommandBase)deserializer.Deserialize(msg.info.m_compName, msg.info.content);
                    commandComp.m_commandList.Add(comp);

                    Debug.Log(msg.info.content);
                }
            }
            else
            {

            }
        }
    }
}
