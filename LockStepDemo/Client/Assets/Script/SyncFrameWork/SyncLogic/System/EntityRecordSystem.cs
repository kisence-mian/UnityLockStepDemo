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
        AddEntityCreaterLisnter();
        AddEntityDestroyLisnter();
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        EntityRecordInfo info = new EntityRecordInfo();
        info.changeType = EntityChangeType.Create;
        info.id = entity.ID;
        info.SaveComp(entity);

        erc.m_list.Add(info);
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        EntityRecordComponent erc = m_world.GetSingletonComp<EntityRecordComponent>();

        EntityRecordInfo info = new EntityRecordInfo();
        info.changeType = EntityChangeType.Destroy;
        info.id = entity.ID;
        info.SaveComp(entity);

        erc.m_list.Add(info);
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
        //Debug.Log("逐帧倒放 " + frame);

        //逐帧倒放
        for (int i = m_world.FrameCount; i == frame; i--)
        {
            RevertOneFrame(i);
        }
    }

    public void RevertOneFrame(int frame)
    {
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
            Debug.Log("DestroyEntityNoDispatch " + data.id);
            m_world.DestroyEntityNoDispatch(data.id);
        }
        else
        {
            Debug.Log("CreateEntityNoDispatch " + data.id);
            m_world.CreateEntityNoDispatch(data.id, data.compList.ToArray());
        }
    }

    public override void Record(int frame, EntityBase entity)
    {
        throw new NotImplementedException();
    }
}
