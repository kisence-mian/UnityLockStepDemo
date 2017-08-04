using LockStepDemo.GameLogic.Component;
using LockStepDemo.GameLogic.System;
using LockStepDemo.Service;
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

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(WaitSyncComponent)
            };
        }

        public override void LateFixedUpdate(int deltaTime)
        {
            List<EntityBase> list = GetEntityList();

            Debug.Log("LateFixedUpdate >" + list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].RemoveComp<WaitSyncComponent>();

                List<EntityBase> players = GetEntityList(new string[] { "ConnectionComponent" });
                for (int j = 0; j < players.Count; j++)
                {
                    PushSyncEnity(players[i].GetComp<ConnectionComponent>().m_session, list[i]);
                }
            }
        }

        #region 推送数据

        public void PushSyncEnity(SyncSession session, EntityBase entity)
        {
            Debug.Log("PUSH Data 1");

            SyncEntityMsg msg = new SyncEntityMsg();
            msg.m_id = entity.ID;
            msg.infos = new List<ComponentInfo>();

            foreach (var c in entity.m_compDict)
            {
                Debug.Log("PUSH Data 2");

                ComponentInfo info = new ComponentInfo();
                info.m_compName = c.GetType().Name;
                info.content = JsonUtility.ToJson(c);
                msg.infos.Add(info);
            }

            session.SendMsg(msg);
        }

    #endregion
    }
}
