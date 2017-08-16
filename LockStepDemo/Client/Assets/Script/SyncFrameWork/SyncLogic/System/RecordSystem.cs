using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RecordSystem : SystemBase
{
    public override void Init()
    {
        AddEntityCompChangeLisenter();
        AddEntityCompAddLisenter();
        AddEntityCompRemoveLisenter();

        AddEntityCreaterLisnter();
        AddEntityDestroyLisnter();
    }

    public override void LateFixedUpdate(int deltaTime)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        RecordInfo info = new RecordInfo();
        info.frame = m_world.FrameCount;
        info.m_changeData = rc.m_changeCache;

        rc.m_recordList.Add(info);

        //清空缓存
        rc.ClearCache();
    }

    #region 事件接收

    public override void OnEntityCompChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ChangeRecordInfo info = new ChangeRecordInfo();
        info.m_type = ChangeType.ChangeComp;
        info.m_EnityID = entity.ID;
        info.m_compName = compName;
        info.m_comp = previousComponent;

        rc.m_changeCache.Add(info);
    }

    public override void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ChangeRecordInfo info = new ChangeRecordInfo();
        info.m_type = ChangeType.AddComp;
        info.m_EnityID = entity.ID;
        info.m_compName = compName;
        info.m_comp = component;

        rc.m_changeCache.Add(info);
    }

    public override void OnEntityCompRemove(EntityBase entity, string compName, ComponentBase component)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ChangeRecordInfo info = new ChangeRecordInfo();
        info.m_type = ChangeType.RemoveComp;
        info.m_EnityID = entity.ID;
        info.m_compName = compName;
        info.m_comp = component;

        rc.m_changeCache.Add(info);
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ChangeRecordInfo info = new ChangeRecordInfo();
        info.m_type = ChangeType.CreateEntity;
        info.m_EnityID = entity.ID;

        rc.m_changeCache.Add(info);
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ChangeRecordInfo info = new ChangeRecordInfo();
        info.m_type = ChangeType.DestroyEntity;
        info.m_EnityID = entity.ID;

        rc.m_changeCache.Add(info);
    }
    #endregion
}
