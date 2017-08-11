using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RecordSystem : SystemBase
{
    public override void Init()
    {
        AddEntityCompChangeLisenter();
    }

    public override void LateFixedUpdate(int deltaTime)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();
        FrameCountComponent fc = m_world.GetSingletonComp<FrameCountComponent>();

        RecordInfo info = new RecordInfo();
        info.frame = fc.count;
        info.m_inputCmd = rc.m_inputCache;
        info.m_ChangeCompData = rc.m_changeCache;

        rc.m_recordList.Add(info);
    }

    public override void OnEntityCompChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
    {
        RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();

        ComponentRecordInfo info = new ComponentRecordInfo();
        info.m_name = compName;
        info.m_comp = previousComponent;

        rc.m_changeCache.Add(info);
    }

    public override void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
    {
        base.OnEntityCompAdd(entity, compName, component);
    }

    public override void OnEntityCompRemove(EntityBase entity, string compName, ComponentBase component)
    {
        base.OnEntityCompRemove(entity, compName, component);
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        base.OnEntityCreate(entity);
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        base.OnEntityDestroy(entity);
    }
}
