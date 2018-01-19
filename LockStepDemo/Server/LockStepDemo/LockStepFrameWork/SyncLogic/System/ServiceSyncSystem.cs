using DeJson;
using HDJ.Framework.Utils;
using LockStepDemo.GameLogic.Component;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ServiceSyncSystem : ServiceSystem
{
    public override void Init()
    {
        Debug.Log("ServiceSyncSystem init");
        m_world.eventSystem.AddListener(ServiceEventDefine.c_playerReconnect, OnPlayerReconnect);
    }

    public override void Dispose()
    {
        m_world.eventSystem.RemoveListener(ServiceEventDefine.c_playerReconnect, OnPlayerReconnect);
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
                typeof(SyncComponent)
            };
    }

    public override void EndFrame(int deltaTime)
    {
        List<EntityBase> list = m_world.GetEntityList(new string[] { "ConnectionComponent" });

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();

            if(cc.m_isWaitPushReconnect)
            {
                cc.m_isWaitPushReconnect = false;
                SendReconnectMsg(cc);
            }
        }
    }

    #region 事件接收
    void OnPlayerReconnect(EntityBase entity, params object[] objs)
    {
        ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
        comp.m_isWaitPushReconnect = true;


    }

    #endregion

    #region 推送数据


    #region 实体

    public EntityInfo CreateEntityInfo(EntityBase entity, SyncSession session)
    {
        EntityInfo Data = new EntityInfo();
        Data.id = entity.ID;
        Data.infos = new List<ComponentInfo>();

        foreach (var c in entity.comps)
        {
            if (c == null)
                continue;

            Type type = c.GetType();

            if (!type.IsSubclassOf(typeof(ServiceComponent))
                && type != typeof(SyncComponent))
            {
                try
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = type.Name;
                    info.content = Serializer.Serialize(c);

                    Data.infos.Add(info);
                }
                catch (StackOverflowException e)
                {
                    Debug.LogError("Serializer error " + type.FullName + " " + e.ToString());
                }
            }
        }

        //给有连接组件的增加Self组件
        if (entity.GetExistComp(ComponentType.ConnectionComponent))
        {
            ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
            if (comp.m_session == session)
            {
                ComponentInfo info = new ComponentInfo();
                info.m_compName = "SelfComponent";
                info.content = "{}";
                Data.infos.Add(info);
            }
            else
            {
                ComponentInfo info = new ComponentInfo();
                info.m_compName = "TheirComponent";
                info.content = "{}";
                Data.infos.Add(info);
            }
        }

        return Data;
    }

    #endregion

    #region 单例组件

    public void PushSingletonComp(SyncSession session, string compName)
    {
        Debug.Log("PushSingletonComp " + compName);

        SingletonComponent comp = m_world.GetSingletonComp(compName);
        ChangeSingletonComponentMsg msg = new ChangeSingletonComponentMsg();

        msg.info = new ComponentInfo();
        msg.info.m_compName = compName;
        msg.info.content = Serializer.Serialize(comp);
        msg.frame = m_world.FrameCount;

        ProtocolAnalysisService.SendMsg(session, msg);
    }

    #endregion

    #region 开始数据

    static string[] s_playerFilter = new string[] { "ConnectionComponent" };
    public static void SendStartMsg(WorldBase world)
    {
        //获取所有的Player组件,并派发
        List<EntityBase> list = world.GetEntityList(s_playerFilter);

        StartSyncMsg startMsg = CreateStartMsg(world);

        SyncEntityMsg playerInfo = new SyncEntityMsg();
        playerInfo.frame = world.FrameCount;
        playerInfo.infos = new List<EntityInfo>();

        for (int i = 0; i < list.Count; i++)
        {
            playerInfo.infos.Add(CreatePlayerComponentInfo(list[i]));
        }

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();
            SyncEntityMsg serviceInfo = CreateServiceMsg(world,cc.m_session);

            ProtocolAnalysisService.SendMsg(cc.m_session, playerInfo);
            ProtocolAnalysisService.SendMsg(cc.m_session, serviceInfo);
            ProtocolAnalysisService.SendMsg(cc.m_session, startMsg);
        }
    }

    #region 生成数据

    public static SyncEntityMsg CreateServiceMsg(WorldBase world,SyncSession session)
    {
        SyncEntityMsg serviceInfo = new SyncEntityMsg();
        serviceInfo.frame = world.FrameCount;
        serviceInfo.infos = new List<EntityInfo>();

        List<EntityBase> list = world.GetEntityList(s_playerFilter);
        for (int i = 0; i < list.Count; i++)
        {
            serviceInfo.infos.Add(CreateServiceComponentInfo(list[i], session));
        }

        return serviceInfo;
    }

    public static EntityInfo CreatePlayerComponentInfo(EntityBase entity)
    {
        EntityInfo Data = new EntityInfo();
        Data.id = entity.ID;
        Data.infos = new List<ComponentInfo>();

        foreach (var c in entity.comps)
        {
            if (c == null)
                continue;

            Type type = c.GetType();

            if (type == typeof(PlayerComponent)
                || type.IsSubclassOf(typeof(AIComponentBase))
                || type == typeof(RealPlayerComponent)
                )
            {
                try
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = type.Name;
                    info.content = Serializer.Serialize(c);

                    Data.infos.Add(info);
                }
                catch (StackOverflowException e)
                {
                    Debug.LogError("Serializer error " + type.FullName + " " + e.ToString());
                }
            }
        }

        return Data;
    }

    public static EntityInfo CreateServiceComponentInfo(EntityBase entity, SyncSession session)
    {
        EntityInfo Data = new EntityInfo();
        Data.id = entity.ID;
        Data.infos = new List<ComponentInfo>();

        //给有连接组件的增加Self组件
        if (entity.GetExistComp(ComponentType.ConnectionComponent))
        {
            ConnectionComponent comp = entity.GetComp<ConnectionComponent>();
            if (comp.m_session == session)
            {
                ComponentInfo info = new ComponentInfo();
                info.m_compName = "SelfComponent";
                info.content = "{}";
                Data.infos.Add(info);
            }
            else
            {
                ComponentInfo info = new ComponentInfo();
                info.m_compName = "TheirComponent";
                info.content = "{}";
                Data.infos.Add(info);
            }
        }

        return Data;
    }

    public static StartSyncMsg CreateStartMsg(WorldBase world)
    {
        Debug.Log("PushStartSyncMsg " + world.FrameCount);

        StartSyncMsg msg = new StartSyncMsg();
        msg.frame = world.FrameCount;
        msg.advanceCount = 1; //客户端提前一帧
        msg.intervalTime = UpdateEngine.IntervalTime;
        msg.createEntityIndex = 0;
        msg.randomSeed = world.m_RandomSeed;

        return msg;
    }

    #endregion

    #endregion

    #region 重连数据

    void SendReconnectMsg(ConnectionComponent comp)
    {
        float time = ServiceTime.GetServiceTime();

        //发送游戏全部数据
        PushAllEnityData(comp);

        //发送单例数据
        PushSingleComponentData(comp);

        //发送游戏开始消息
        StartSyncMsg startMsg = CreateStartMsg(m_world);
        ProtocolAnalysisService.SendMsg(comp.m_session, startMsg);

        Debug.Log("重连时间 " + (ServiceTime.GetServiceTime() - time));
    }

    void PushSingleComponentData(ConnectionComponent comp)
    {
        //同步单例组件
        foreach (var item in m_world.m_singleCompDict)
        {
            if (item.Key != "EntityRecordComponent")
            {
                PushSingletonComp(comp.m_session, item.Key);
            }
        }
    }

    public void PushAllEnityData(ConnectionComponent connect)
    {
        if (connect.m_session != null
            && !connect.m_session.Connected)
        {
            return;
        }

        SyncEntityMsg msg = new SyncEntityMsg();
        msg.frame = m_world.FrameCount;
        msg.infos = new List<EntityInfo>();

        List<EntityBase> waitSyncList = GetEntityList();

        int count = 0;

        for (int i = 0; i < waitSyncList.Count; i++)
        {
            count++;
            msg.infos.Add(CreateEntityInfo(waitSyncList[i], connect.m_session));

            if (count > 5)
            {
                count = 0;
                if (msg.infos.Count > 0)
                {
                    ProtocolAnalysisService.SendMsg(connect.m_session, msg);
                }

                msg = new SyncEntityMsg();
                msg.frame = m_world.FrameCount;

                msg.infos = new List<EntityInfo>();
            }
        }

        if (msg.infos.Count > 0 )
        {
            ProtocolAnalysisService.SendMsg(connect.m_session, msg);
        }

        connect.m_waitDestroyEntity.Clear();
        connect.m_waitSyncEntity.Clear();
    }

    #endregion

    #endregion
}

