using FrameWork;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSyncSystem<T> : SystemBase where T:PlayerCommandBase,new()
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(SelfComponent),
            typeof(T)
        };
    }

    public override void LateFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = new List<EntityBase>();

        for (int i = 0; i < list.Count; i++)
        {
            T comp = list[i].GetComp<T>();

            ChangeComponentMsg msg = new ChangeComponentMsg();
            msg.m_id = list[i].ID;
            msg.info.m_compName = comp.GetType().Name;
            msg.info.content = Serializer.Serialize(comp);

            ProtocolAnalysisService.SendCommand(msg);
        }
    }
}
