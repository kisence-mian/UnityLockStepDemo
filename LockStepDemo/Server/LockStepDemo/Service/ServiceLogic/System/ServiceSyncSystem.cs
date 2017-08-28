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
            AddEntityCreaterLisnter();
            AddEntityDestroyLisnter();

            AddEntityCompAddLisenter();
            AddEntityCompRemoveLisenter();
        }

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(SyncComponent)
            };
        }

        public override void BeforeFixedUpdate(int deltaTime)
        {
            //全推送
            PushAllData();

            //推送开始消息
            PushStartSyncMsg();
        }

        public override void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
        {
            Debug.Log("OnEntityCompAdd " + compName);

            //有新玩家加入
            if (entity.GetExistComp<ConnectionComponent>()
                && entity.GetExistComp<SyncComponent>())
            {
                OnPlayerJoin(entity);
            }
        }

        public override void OnEntityCompRemove(EntityBase entity, string compName, ComponentBase component)
        {
            if (entity.GetExistComp<ConnectionComponent>()
                && entity.GetExistComp<SyncComponent>())
            {
                OnPlayerExit(entity);
            }
        }

        public override void OnEntityCreate(EntityBase entity)
        {
            Debug.Log("OnEntityCreate ");

            SyncComponent sc = null;
            //自动创建Sync组件
            if (!entity.GetExistComp<SyncComponent>())
            {
                Debug.Log("自动创建Sync组件 ");
                sc = entity.AddComp<SyncComponent>();
            }
            else
            {
                sc = entity.GetComp<SyncComponent>();
            }

            if(m_world.SyncRule == SyncRule.Status)
            {
                SetAllSync(sc);
            }
        }

        public override void OnEntityDestroy(EntityBase entity)
        {
            Debug.Log("OnEntityDestroy ");

            if (entity.GetExistComp<SyncComponent>())
            {
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

        #region 玩家相关
        
        void OnPlayerJoin(EntityBase entity)
        {
            Debug.Log("ServiceSyncSystem OnPlayerJoin ");

            ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
            SyncComponent syc = entity.GetComp<SyncComponent>();

            comp.m_isWaitPushStart = true;

            List<EntityBase> list = GetEntityList();
            for (int i = 0; i < list.Count; i++)
            {
                //在所有同步对象的同步队列里加入这个新玩家 (把这个世界的数据告诉新玩家)
                SyncComponent sycTmp = list[i].GetComp<SyncComponent>();
                if(!sycTmp.m_waitSyncList.Contains(comp))
                {
                    sycTmp.m_waitSyncList.Add(comp);
                }
            }

            //把自己广播给所有人
            SetAllSync(syc);
        }

        void OnPlayerExit(EntityBase entity)
        {
            ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
            comp.m_isWaitPushStart = false;

            SyncComponent sc = entity.GetComp<SyncComponent>();
            SetAllSync(sc);

            //TODO 将来改成推送移除连接组件
            PushDestroyEntity(sc, entity);
        }

        #endregion

        #region 推送数据

        #region 游戏进程

        void PushStartSyncMsg()
        {
            List<EntityBase> list = GetEntityList(new string[] { "ConnectionComponent" });
            for (int i = 0; i < list.Count; i++)
            {
                ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
                
                if(comp.m_isWaitPushStart == true)
                {
                    comp.m_isWaitPushStart = false;
                    PushStartSyncMsg(comp.m_session);
                }
            }
        }

        void PushStartSyncMsg(SyncSession session)
        {
            Debug.Log("PushStartSyncMsg ");

            StartSyncMsg msg = new StartSyncMsg();
            msg.frame = m_world.FrameCount;
            msg.advanceCount = 1; //客户端提前一帧
            msg.intervalTime = UpdateEngine.IntervalTime;
            msg.createEntityIndex = m_world.EntityIndex;

            ProtocolAnalysisService.SendMsg(session, msg);
        }

        //推送所有数据(把所有同步队列的数据推送出去)
        public void PushAllData()
        {
            List<EntityBase> list = GetEntityList();
            for (int i = 0; i < list.Count; i++)
            {
                SyncComponent sc = list[i].GetComp<SyncComponent>();
                PushSyncEnity(sc, list[i]);
            }
        }

        #endregion

        #region 实体

        public void PushSyncEnity(SyncComponent connectionComp, EntityBase entity)
        {
            for (int i = 0; i < connectionComp.m_waitSyncList.Count; i++)
            {
                Debug.Log("Push " + connectionComp.m_waitSyncList[i].m_session.SessionID + " entity " + entity.ID);
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
            msg.frame = m_world.FrameCount;
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
            ProtocolAnalysisService.SendMsg(session, msg);
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
            DestroyEntityMsg msg = new DestroyEntityMsg();
            msg.id = entityID;
            msg.frame = m_world.FrameCount;

            ProtocolAnalysisService.SendMsg(session, msg);

            Debug.Log("PushDestroyEntity 3");
        }

        #endregion

        #region 单例组件

        public void PushSingletonComp<T>(SyncSession session) where T :SingletonComponent,new()
        {
            string key = typeof(T).Name;
            PushSingletonComp(session,key);
        }

        public void PushSingletonComp(SyncSession session, string compName)
        {
            SingletonComponent comp = m_world.GetSingletonComp(compName);
            ChangeSingletonComponentMsg msg = new ChangeSingletonComponentMsg();
            msg.info.m_compName = compName;
            msg.info.content = Serializer.Serialize(comp);
            msg.frame = m_world.FrameCount;

            ProtocolAnalysisService.SendMsg(session, msg);
        }

        #endregion

        #endregion
    }
}
