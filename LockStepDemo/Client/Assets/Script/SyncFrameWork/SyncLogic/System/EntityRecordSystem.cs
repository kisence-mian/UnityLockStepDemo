using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO 暂时搁置
public class EntityRecordSystem :RecordSystemBase
{
    public override void Init()
    {
        Debug.Log("EntityRecordSystem Init");

        AddEntityCreaterLisnter();
        AddEntityDestroyLisnter();
    }

    public override void Dispose()
    {
        RemoveEntityCreaterLisnter();
        RemoveEntityDestroyLisnter();
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        //只记录预测时的操作
        if(m_world.m_isRecalc)
        {
            return;
        }

        //Debug.Log(" 记录创建 ID: " + entity.ID + " frame " + m_world.FrameCount);

        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        //如果此帧有这个ID的摧毁记录，把它抵消掉
        EntityRecordInfo record = erc.GetReord(m_world.FrameCount, entity.ID, EntityChangeType.Destroy);
        if (record != null)
        {
            //Debug.Log("抵消掉摧毁记录 " + entity.ID);
            erc.m_list.Remove(record);
        }
        else
        {
            EntityRecordInfo info = new EntityRecordInfo();
            info.changeType = EntityChangeType.Create;
            info.id = entity.ID;
            info.frame = m_world.FrameCount;
            info.SaveComp(entity);

            erc.m_list.Add(info);
        }
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        //只记录预测时的操作
        if (m_world.m_isRecalc)
        {
            return;
        }

        //Debug.Log(" 记录摧毁 ID: " + entity.ID + " frame " + m_world.FrameCount);

        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        //如果此帧有这个ID的创建记录，把它抵消掉
        EntityRecordInfo record = erc.GetReord(m_world.FrameCount, entity.ID, EntityChangeType.Create);

        if(record != null)
        {
            //Debug.Log("抵消掉创建记录 " + entity.ID);
            erc.m_list.Remove(record);
        }
        else
        {
            EntityRecordInfo info = new EntityRecordInfo();
            info.changeType = EntityChangeType.Destroy;
            info.id = entity.ID;
            info.frame = m_world.FrameCount;
            info.SaveComp(entity);

            erc.m_list.Add(info);
        }
    }

    public override void ClearAfter(int frame)
    {
        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();
    }

    public override void ClearBefore(int frame)
    {
        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();
    }

    public override MomentComponentBase GetRecord(int id, int frame)
    {
        throw new NotImplementedException();
    }

    public override void PrintRecord(int id)
    {
        throw new NotImplementedException();
    }

    public override void Record(int frame)
    {
    }

    public override void RevertToFrame(int frame)
    {
        //Debug.Log("RevertToFrame m_world.Frame " + m_world.FrameCount + " frame " + frame);

        //逐帧倒放
        for (int i = m_world.FrameCount; i >= frame + 1; i--)
        {
            RevertOneFrame(i);
        }
    }

    public void RevertOneFrame(int frame)
    {
        //Debug.Log("回退到 " + frame);

        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        for (int i = 0; i < erc.m_list.Count; i++)
        {
            if(erc.m_list[i].frame == frame)
            {
                RevertRecord(erc.m_list[i]);
            }
        }
    }

    public void RevertRecord(EntityRecordInfo data)
    {
        if(data.changeType == EntityChangeType.Create)
        {
            //Debug.Log("DestroyEntityNoDispatch " + data.id + " frame " + m_world.FrameCount);
            m_world.RollbackCreateEntity(data.id);
        }
        else
        {
            //Debug.Log("CreateEntityNoDispatch " + data.id + " frame " + m_world.FrameCount);
            m_world.RollbackDestroyEntity(data.id, data.compList.ToArray());
        }
    }

    public override void Record(int frame, EntityBase entity)
    {
        throw new NotImplementedException();
    }
}
