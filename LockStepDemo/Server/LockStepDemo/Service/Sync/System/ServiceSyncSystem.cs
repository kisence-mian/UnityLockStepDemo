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
            AddEntityCompChangeLisenter();
            AddEntityDestroyLisnter();
        }

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(SyncComponent)
            };
        }

        public override void BeforeFixedUpdate(int deltaTime)
        {
            FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();

            fc.count++;

            Debug.Log("-------------------- Frame:" + fc.count + "------------------------------");
        }

        public override void LateFixedUpdate(int deltaTime)
        {
            List<EntityBase> list = GetEntityList();

            for (int i = 0; i < list.Count; i++)
            {
                PushSyncEnity(list[i].GetComp<SyncComponent>(), list[i]);
            }
        }


        public override void OnEntityCompChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
        {
            if (entity.GetExistComp<SyncComponent>())
            {
                List<EntityBase> list = GetEntityList(new string[] { "ConnectionComponent" });
                for (int i = 0; i < list.Count; i++)
                {
                    SyncComponent comp = entity.GetComp<SyncComponent>();
                    ConnectionComponent connComp = list[i].GetComp<ConnectionComponent>();

                    if (!comp.m_waitSyncList.Contains(connComp))
                    {
                        comp.m_waitSyncList.Add(connComp);
                    }
                }
            }
        }

        public override void OnEntityDestroy(EntityBase entity)
        {
            Debug.Log("OnEntityDestroy");

            if (entity.GetExistComp<SyncComponent>())
            {
                Debug.Log("PushDestroyEntity 1");
                SyncComponent sc = entity.GetComp<SyncComponent>();
                SetAllSync(sc);
                PushDestroyEntity(sc, entity);
            }
        }

        public void SetAllSync(SyncComponent connectionComp)
        {
            List<EntityBase> list = GetEntityList(new string[] { "ConnectionComponent"});

            for (int i = 0; i < list.Count; i++)
            {
                ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
                if (!connectionComp.m_waitSyncList.Contains(comp))
                {
                    connectionComp.m_waitSyncList.Add(comp);
                }
            }

        }

        #region 推送数据

        #region 实体

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

            FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();

            SyncEntityMsg msg = new SyncEntityMsg();
            msg.frame = fc.count;
            msg.id = entity.ID;
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

            //给有连接组件的增加Self组件
            if (entity.GetExistComp<ConnectionComponent>())
            {
                ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
                if(comp.m_session == session)
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = "SelfComponent";
                    info.content = "{}";
                    msg.infos.Add(info);
                }
                else
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = "TheirComponent";
                    info.content = "{}";
                    msg.infos.Add(info);
                }
            }

            session.SendMsg(msg);
        }

        public void PushDestroyEntity(SyncComponent connectionComp, EntityBase entity)
        {
            for (int i = 0; i < connectionComp.m_waitSyncList.Count; i++)
            {
                PushDestroyEntity(connectionComp.m_waitSyncList[i].m_session, entity.ID);
            }
            connectionComp.m_waitSyncList.Clear();
        }

        void PushDestroyEntity(SyncSession session, int entityID)
        {
            FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();
            DestroyEntityMsg msg = new DestroyEntityMsg();
            msg.id = entityID;
            msg.frame = fc.count;

            session.SendMsg(msg);

            Debug.Log("PushDestroyEntity 3");
        }

        #endregion

        #region

        public void PushSingletonComp<T>(SyncSession session) where T :SingletonComponent,new()
        {
            string key = typeof(T).Name;
            PushSingletonComp(session,key);
        }

        public void PushSingletonComp(SyncSession session, string compName)
        {
            FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();

            SingletonComponent comp = m_world.GetSingletonComp(compName);
            ChangeSingletonComponentMsg msg = new ChangeSingletonComponentMsg();
            msg.info.m_compName = compName;
            msg.info.content = Serializer.Serialize(comp);
            msg.frame = fc.count;

            session.SendMsg(msg);
        }

        #endregion

        #endregion
    }
}
