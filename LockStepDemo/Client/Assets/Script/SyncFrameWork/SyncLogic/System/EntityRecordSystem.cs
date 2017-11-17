using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EntityRecordSystem :RecordSystemBase
{
    public override void Init()
    {
        //Debug.Log("EntityRecordSystem Init");

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
        //Debug.Log("EntityRecordSystem OnEntityCreate！ " + entity.ID + " m_isCertainty " + m_world.m_isCertainty);

        //只记录预测时的操作
        if (m_world.m_isCertainty)
        {
            return;
        }

        //Debug.Log(" 记录创建 ID: " + entity.ID + " frame " + entity.m_CreateFrame);

        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        //如果此帧有这个ID的摧毁记录，把它抵消掉
        EntityRecordInfo record = erc.GetReord(entity.m_CreateFrame, entity.ID, EntityChangeType.Destroy);
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
            info.frame = entity.m_CreateFrame;
            info.SaveComp(entity);

            erc.m_list.Add(info);
        }
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        //Debug.Log("EntityRecordSystem OnEntityDestroy！ " + entity.ID + " m_isCertainty " + m_world.m_isCertainty);

        //只记录预测时的操作
        if (m_world.m_isCertainty)
        {
            return;
        }

        //Debug.Log(" 记录摧毁 ID: " + entity.ID + " frame " + entity.m_DestroyFrame);

        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        //如果此帧有这个ID的创建记录，把它抵消掉
        EntityRecordInfo record = erc.GetReord(entity.m_DestroyFrame, entity.ID, EntityChangeType.Create);

        if(record != null)
        {
            ////Debug.Log("抵消掉创建记录 " + entity.ID);
            erc.m_list.Remove(record);
        }
        else
        {
            EntityRecordInfo info = new EntityRecordInfo();
            info.changeType = EntityChangeType.Destroy;
            info.id = entity.ID;
            info.frame = entity.m_DestroyFrame;
            info.SaveComp(entity);

            erc.m_list.Add(info);
        }
    }

    public override void ClearAfter(int frame)
    {
        //EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();
    }

    public override void ClearBefore(int frame)
    {
        //EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();
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
        ////Debug.Log("RevertToFrame m_world.Frame " + m_world.FrameCount + " frame " + frame);

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

                //暂时先不移除记录
                //erc.m_list.RemoveAt(i);
                //i--;
            }
        }

        //Debug.Log("回退结束");
    }

    public void RevertRecord(EntityRecordInfo data)
    {
        //if(data.changeType == EntityChangeType.Create)
        //{
        //    //Debug.Log("RevertRecord DestroyEntityNoDispatch " + data.id + " frame " + data.frame + " worldFrame " + m_world.FrameCount);
        //    m_world.RollbackCreateEntity(data.id, data.frame);
        //}
        //else
        //{
        //    //Debug.Log("RevertRecord CreateEntityNoDispatch " + data.id + " frame " + data.frame + " worldFrame" + m_world.FrameCount);
        //    m_world.RollbackDestroyEntity(data.id, data.frame, data.compList.ToArray());
        //}
    }

    public override void Record(int frame, EntityBase entity)
    {
        throw new NotImplementedException();
    }

    public override void ClearAll()
    {
        throw new NotImplementedException();
    }
}
