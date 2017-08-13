using LockStepDemo.GameLogic.System;
using LockStepDemo.Service;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.ServiceLogic.System
{
    class ConnectSystem : ServiceSystem
    {
        public override void Init()
        {
            AddEntityCompAddLisenter();
            AddEntityCompRemoveLisenter();
        }

        public override void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
        {
            if(compName == "ConnectionComponent")
            {
                List<EntityBase> list = GetEntityList();
                for (int i = 0; i < list.Count; i++)
                {
                    SyncComponent sc = list[i].GetComp<SyncComponent>();
                    sc.m_waitSyncList.Add((ConnectionComponent)component);
                }
            }
        }

        public override void OnEntityCompRemove(EntityBase entity, string compName, ComponentBase component)
        {
            if (compName == "ConnectionComponent")
            {
                List<EntityBase> list = GetEntityList();
                for (int i = 0; i < list.Count; i++)
                {
                    SyncComponent sc = list[i].GetComp<SyncComponent>();
                    ConnectionComponent cc = (ConnectionComponent)component;

                    if (sc.m_waitSyncList.Contains(cc))
                    {
                        sc.m_waitSyncList.Remove(cc);
                    }
                }
            }
        }

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(SyncComponent),
            };
        }

        public void PushStartSyncMsg(SyncSession session)
        {
            StartSyncMsg msg = new StartSyncMsg();
            msg.frame = m_world.FrameCount;
            msg.intervalTime = UpdateEngine.IntervalTime;
        }
    }
}
