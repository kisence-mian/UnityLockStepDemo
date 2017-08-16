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
            typeof(T),
        };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        int selfCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            //自动添加记录组件
            if(!list[i].GetExistComp<PlayerCommandRecordComponent>())
            {
                list[i].AddComp<PlayerCommandRecordComponent>();
            }

            if(list[i].GetExistComp<SelfComponent>())
            {
                selfCount++;
                if(selfCount > 1)
                {
                    //不止1个Self组件则报错，只取第一个组件
                    Debug.LogError("CommandSyncSystem Error exist move than one SelfComponet!");
                }
                else
                {
                    SelfCommandLogic(list[i]);
                }
            }
            else
            {
                OtherCommandLogic(list[i]);
            }
        }
    }

    void SelfCommandLogic(EntityBase entity)
    {
        T comp = new T();

        comp.frame = m_world.FrameCount;
        comp.id = entity.ID;

        BuildCommand(comp);
        entity.ChangeComp(comp);

        //缓存起来
        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>();
        rc.m_lastInput = comp;
        rc.m_inputCache.Add(comp);

        ProtocolAnalysisService.SendCommand(comp);
    }

    void OtherCommandLogic(EntityBase entity)
    {
        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>();
        //先取服务器缓存
        T cmd = (T)rc.GetServerCache(m_world.FrameCount);

        //没有的话预测一份
        if (cmd == null)
        {
            Debug.Log("预测输入 ");

            //取最后一次输入缓存，没有的话New一个新的
            cmd = (T)rc.m_lastInput;

            if(cmd == null)
            {
                cmd = new T();
                cmd.frame = m_world.FrameCount;
                cmd.id = entity.ID;
            }
            rc.m_forecastInput = cmd;
        }
        else
        {
            Debug.Log("读取缓存 ");
        }

        rc.m_lastInput = cmd;
        rc.m_inputCache.Add(cmd);

        entity.ChangeComp(cmd);
    }

    public virtual void BuildCommand(T command)
    {

    }
}
