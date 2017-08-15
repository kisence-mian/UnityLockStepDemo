using FrameWork;
using LockStepDemo.GameLogic.Component;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSyncSystem<T> : ViewSystemBase where T:PlayerCommandBase,new()
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SelfComponent),
            typeof(T)
        };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        if (list.Count > 1)
        {
            Debug.LogError("CommandSyncSystem Error exist two selfComponet!");
            return;
        }

        if (list.Count > 0)
        {
            EntityBase entity = list[0];
            T comp = new T();

            comp.frame = m_world.FrameCount;
            comp.id = entity.ID;

            BuildCommand(comp);
            entity.ChangeComp(comp);

            //缓存起来
            RecordComponent rc = m_world.GetSingletonComp<RecordComponent>();
            rc.m_inputCache = comp;

            ProtocolAnalysisService.SendCommand(comp);
        }
    }

    public virtual void BuildCommand(T command)
    {

    }
}
