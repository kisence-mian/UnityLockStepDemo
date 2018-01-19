using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimisticCommandSystem<T> : SystemBase where T:PlayerCommandBase,new()
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(T),
            typeof(RealPlayerComponent),
        };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        int selfCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetExistComp(ComponentType.SelfComponent))
            {
                selfCount++;
                if (selfCount > 1)
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

    void OtherCommandLogic(EntityBase entity)
    {
        //Debug.Log("OtherCommandLogic " + m_world.FrameCount);

        AddComp(entity);

        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
        //先取服务器缓存
        T cmd = (T)rc.GetInputCahae(m_world.FrameCount);

        //没有的话预测一份
        if (cmd == null)
        {
            //Debug.Log("预测本地操作 " + m_world.FrameCount + " id " + entity.ID);
            cmd = (T)rc.GetForecastInput(m_world.FrameCount);
        }

        rc.RecordCommand(cmd);

        entity.ChangeComp(cmd);

        //Debug.Log("Other cmd " + entity.ID + " content " + Serializer.Serialize(cmd) + " " + m_world.FrameCount);
    }

    public void AddComp(EntityBase entity)
    {
        if (!entity.GetExistComp(ComponentType.PlayerCommandRecordComponent))
        {
            //Debug.Log("OnEntityCompAdd PlayerCommandRecordComponent");

            PlayerCommandRecordComponent rc = new PlayerCommandRecordComponent();
            rc.m_defaultInput = new T();

            //自动添加记录组件
            entity.AddComp(rc);
        }
    }

    SameCommand sameCmdCache = new SameCommand();
    void SelfCommandLogic(EntityBase entity)
    {
        //Debug.Log("SelfCommandLogic " + m_world.FrameCount);

        if(m_world.IsLocal)
        {
            //构建一份新指令并发送
            T ncmd = new T();

            ncmd.frame = m_world.FrameCount;
            ncmd.id = entity.ID;

            BuildCommand(ncmd);

            entity.ChangeComp(ncmd);
        }
        else
        {
            //先取服务器缓存
            AddComp(entity);
            PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);

            T cmd = (T)rc.GetInputCahae(m_world.FrameCount);
            cmd = (T)cmd.DeepCopy();
            entity.ChangeComp(cmd);

            //构建一份新指令并发送
            T ncmd = new T();

            ncmd.frame = m_world.FrameCount + 1;
            ncmd.id = entity.ID;

            BuildCommand(ncmd);

            if (ncmd.EqualsCmd(cmd))
            {
                sameCmdCache.frame = m_world.FrameCount + 1;
                sameCmdCache.time = ClientTime.GetTime();
                sameCmdCache.id = entity.ID;

                if (NetworkManager.IsConnect)
                {
                    ProtocolAnalysisService.SendCommand(sameCmdCache);
                }
            }
            else
            {
                ncmd.frame = m_world.FrameCount + 1;
                ncmd.time = ClientTime.GetTime();
                if (NetworkManager.IsConnect)
                {
                    ProtocolAnalysisService.SendCommand(ncmd);
                }
            }
        }
    }

    public virtual void BuildCommand(T command)
    {

    }
}
