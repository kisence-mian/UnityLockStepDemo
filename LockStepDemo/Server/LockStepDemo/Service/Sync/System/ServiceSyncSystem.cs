using DeJson;
using LockStepDemo.GameLogic.Component;
using LockStepDemo.GameLogic.System;
using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LockStepDemo.ServiceLogic.System
{
    class ServiceSyncSystem : ServiceSystem
    {
        public override void Init()
        {   
            
        }

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(SyncComponent)
            };
        }

        public override void LateFixedUpdate(int deltaTime)
        {
            List<EntityBase> list = GetEntityList();

            for (int i = 0; i < list.Count; i++)
            {
                PushSyncEnity(list[i].GetComp<SyncComponent>(), list[i]);
            }
        }

        #region 推送数据

        public void PushSyncEnity(SyncComponent connectionComp, EntityBase entity)
        {
            for (int i = 0; i < connectionComp.m_waitSyncList.Count; i++)
            {
                PushSyncEnity(connectionComp.m_waitSyncList[i].m_session, entity);
            }
            connectionComp.m_waitSyncList.Clear();
        }

        public void PushSyncEnity(SyncSession session, EntityBase entity)
        {
            if(!session.Connected)
            {
                return;
            }

            SyncEntityMsg msg = new SyncEntityMsg();
            msg.m_id = entity.ID;
            msg.infos = new List<ComponentInfo>();

            foreach (var c in entity.m_compDict)
            {
                Type type = c.Value.GetType();

                if(!type.IsSubclassOf(typeof(ServiceComponent)))
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = type.Name;
                    info.content = Serializer.Serialize(c.Value);
                    msg.infos.Add(info);
                }
            }
            session.SendMsg(msg);
        }

        #endregion
    }
}
