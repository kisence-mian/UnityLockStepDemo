using FrameWork;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSyncSystem<T> : ViewSystemBase where T:PlayerCommandBase,new()
{
    public override void Init()
    {
        AddEntityCompAddLisenter();
    }

    public override void Dispose()
    {
        RemoveEntityCompAddLisenter();
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(T),
        };
    }

    public void AddComp(EntityBase entity)
    {
        if (!entity.GetExistComp<PlayerCommandRecordComponent>())
        {
            //Debug.Log("OnEntityCompAdd PlayerCommandRecordComponent");

            PlayerCommandRecordComponent rc = new PlayerCommandRecordComponent();
            rc.m_defaultInput = new T();

            //自动添加记录组件
            entity.AddComp(rc);
        }
    }

    /// <summary>
    /// 重演算的时候读取输入缓存
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="deltaTime"></param>
    public override void OnlyCallByRecalc(int frame,int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            PlayerCommandRecordComponent rc = list[i].GetComp<PlayerCommandRecordComponent>();
            T cmd = (T)rc.GetInputCahae(frame);

            if(cmd == null)
            {
                Debug.LogError("重演算没有读取到输入记录！ frame:" + frame + " ID: " + list[i].ID);
                return; //TODO
            }

            list[i].ChangeComp(cmd);
        }
    }

    public override void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        int selfCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
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

        AddComp(entity);

        //缓存起来
        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>();
        rc.RecordCommand(comp);

        if(!m_world.m_isLocal)
        {
            comp.time = ClientTime.GetTime();
            ProtocolAnalysisService.SendCommand(comp);
        }
    }

    void OtherCommandLogic(EntityBase entity)
    {
        AddComp(entity);

        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>();
        //先取服务器缓存
        T cmd = (T)rc.GetInputCahae(m_world.FrameCount);

        //没有的话预测一份
        if (cmd == null)
        {
            cmd = (T)rc.GetForecastInput(m_world.FrameCount);
        }

        rc.RecordCommand(cmd);

        entity.ChangeComp(cmd);
    }

    public virtual void BuildCommand(T command)
    {

    }
}
