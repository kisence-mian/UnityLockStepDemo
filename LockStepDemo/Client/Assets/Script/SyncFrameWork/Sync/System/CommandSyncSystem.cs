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
            typeof(WaitSyncComponent),
            typeof(SelfComponent),
            typeof(T)
        };
    }

    public override void LateUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].RemoveComp<WaitSyncComponent>();

            T comp = list[i].GetComp<T>();

            ChangeComponentMsg msg = new ChangeComponentMsg();
            msg.m_id = list[i].ID;
            msg.info = new ComponentInfo();
            msg.info.m_compName = comp.GetType().Name;
            msg.info.content = Serializer.Serialize(comp);

            ProtocolAnalysisService.SendCommand(msg);

            Debug.Log("Send 123");
        }
    }
}
