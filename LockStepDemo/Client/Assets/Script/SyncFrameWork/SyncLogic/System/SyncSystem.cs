using FrameWork;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSystem : ViewSystemBase
{
    public override void Init()
    {
        GlobalEvent.AddTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.AddTypeEvent<DestroyEntityMsg>(ReceviceDestroyEntityMsg);
        GlobalEvent.AddTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
    }

    public override void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.RemoveTypeEvent<DestroyEntityMsg>(ReceviceDestroyEntityMsg);
        GlobalEvent.RemoveTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();
        fc.count++;
    }

    #region 消息接收
    Deserializer deserializer = new Deserializer();

    void ReceviceSyncEntity(SyncEntityMsg msg, params object[] objs)
    {
        EntityBase entity;
        if(!m_world.GetEntityIsExist(msg.id))
        {
            entity = m_world.CreateEntity(msg.id);
        }
        else
        {
            entity = m_world.GetEntity(msg.id);
        }

        for (int i = 0; i < msg.infos.Count; i++)
        {
            ComponentBase comp = (ComponentBase)deserializer.Deserialize(msg.infos[i].m_compName, msg.infos[i].content);

            if (entity.GetExistComp(msg.infos[i].m_compName))
            {
                entity.ChangeComp(msg.infos[i].m_compName, comp);
            }
            else
            {
                entity.AddComp(msg.infos[i].m_compName, comp);
            }
        }
    }

    void ReceviceDestroyEntityMsg(DestroyEntityMsg msg, params object[] objs)
    {
        Debug.Log("ReceviceDestroyEntityMsg");

        m_world.DestroyEntity(msg.id);
    }

    void ReceviceChangeSingletonCompMsg(ChangeSingletonComponentMsg msg,params object[] objs)
    {
        Debug.Log("ChangeSingletonCompMsg");

        SingletonComponent comp = (SingletonComponent)deserializer.Deserialize(msg.info.m_compName, msg.info.content);
        m_world.ChangeSingleComp(msg.info.m_compName, comp);
    }
    #endregion

    #region 消息发送

    #endregion

    #region 同步重新演算

    public void Recalc(int frameCount)
    {
        FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();

        //回退到目标帧
        RevertToFrame(frameCount);

        for (int i = frameCount; i < fc.count; i++)
        {
            //重新读取操作
            LoadPlayerInput(i);

            //服务器输入其他人操作
            ExecuteServiceMessage(i);

            //重新演算
            m_world.FixedLoop(1000);
        }

        ClearRecordInfo(fc.count - 1);
    }

    #region 状态回滚

    public List<RecordInfo> GetRecordInfo(int frameCount)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        List<RecordInfo> list = new List<RecordInfo>();

        for (int i = 0; i < rc.m_recordList.Count; i++)
        {
            if (rc.m_recordList[i].frame >= frameCount)
            {
                list.Add(rc.m_recordList[i]);
            }
        }

        return list;
    }

    public void ClearRecordInfo(int frameCount)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();
        for (int i = 0; i < rc.m_recordList.Count; i++)
        {
            if (rc.m_recordList[i].frame >= frameCount)
            {
                rc.m_recordList.RemoveAt(i);
                i--;
            }
        }
    }

    public void RevertToFrame(int frameCount)
    {
        List<RecordInfo> list = GetRecordInfo(frameCount);

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].m_changeData.Count; j++)
            {
                RevertByChangeRecordInfo(list[i].m_changeData[j]);
            }
        }
    }

    public void RevertByChangeRecordInfo(ChangeRecordInfo info)
    {
        switch (info.m_type)
        {
            case ChangeType.AddComp: RevertAddComp(info); break;
            case ChangeType.ChangeComp: RevertChangeComp(info); break;
            case ChangeType.RemoveComp: RevertRemoveComp(info); break;
            case ChangeType.CreateEntity: RevertCreateEntity(info); break;
            case ChangeType.DestroyEntity: RevertDestroyEntity(info); break;
        }
    }

    public void RevertAddComp(ChangeRecordInfo info)
    {
        EntityBase entity = m_world.GetEntity(info.m_EnityID);

        entity.RemoveComp(info.m_compName);
    }

    public void RevertChangeComp(ChangeRecordInfo info)
    {
        EntityBase entity = m_world.GetEntity(info.m_EnityID);

        entity.ChangeComp(info.m_compName, info.m_comp);
    }

    public void RevertRemoveComp(ChangeRecordInfo info)
    {
        EntityBase entity = m_world.GetEntity(info.m_EnityID);

        entity.AddComp(info.m_compName, info.m_comp);
    }

    public void RevertCreateEntity(ChangeRecordInfo info)
    {
        m_world.DestroyEntity(info.m_EnityID);
    }

    public void RevertDestroyEntity(ChangeRecordInfo info)
    {
        m_world.CreateEntity(info.m_EnityID);
    }

    #endregion

    #region 读取历史输入数据

    void LoadPlayerInput(int frameCount)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        for (int i = 0; i < rc.m_recordList.Count; i++)
        {
            if (rc.m_recordList[i].frame == frameCount)
            {
                //todo something
            }
        }
    }

    #endregion

    #region 读取并执行服务器数据

    void ExecuteServiceMessage(int frameCount)
    {

    }

    #endregion

    #endregion
}