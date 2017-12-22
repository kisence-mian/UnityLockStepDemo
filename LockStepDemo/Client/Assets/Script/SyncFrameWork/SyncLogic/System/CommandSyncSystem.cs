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
            typeof(RealPlayerComponent),
        };
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

    public override void BeforeFixedUpdate(int deltaTime)
    {
        if(m_world.IsRecalc)
        {
            OnlyCallByRecalc(m_world.FrameCount, deltaTime);
        }
        else
        {
            NoRecalcBeforeFixedUpdate(deltaTime);
        }
    }

    /// <summary>
    /// 重演算的时候读取输入缓存
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="deltaTime"></param>
    public void OnlyCallByRecalc(int frame,int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        //Debug.Log("OnlyCallByRecalc count " + list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            AddComp(list[i]);
            PlayerCommandRecordComponent rc = list[i].GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent );
            T cmd = (T)rc.GetInputCahae(frame);

            //Debug.Log("recalc cmd " + list[i].ID + " content " + Serializer.Serialize(cmd) + " " + m_world.FrameCount);

            if(cmd == null)
            {
                Debug.LogError("重演算没有读取到输入记录！ frame:" + frame + " ID: " + list[i].ID);
            }
            else
            {
                list[i].ChangeComp(cmd);
            }
        }
    }

    public void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        int selfCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].GetExistComp(ComponentType.SelfComponent))
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

    SameCommand sameCmdCache = new SameCommand();
    void SelfCommandLogic(EntityBase entity)
    {
        //Debug.Log("SelfCommandLogic " + m_world.FrameCount);

        //先取服务器缓存
        AddComp(entity);
        PlayerCommandRecordComponent rc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);

        T cmd = (T)rc.GetInputCahae(m_world.FrameCount);

        //没有的话构建一份
        if (cmd == null)
        {
            cmd = new T();

            cmd.frame = m_world.FrameCount;
            cmd.id = entity.ID;

            BuildCommand(cmd);

            rc.RecordCommand(cmd);

            //Debug.Log("Self cmd " + entity.ID + " content " + Serializer.Serialize(cmd) + " " + m_world.FrameCount);
        }
        else
        {
            //Debug.Log("读取 服务器缓存 输入");
            cmd = (T)cmd.DeepCopy();
        }

        if (!m_world.IsLocal)
        {
            T record = (T)rc.GetInputCahae(m_world.FrameCount - 1);

            cmd.frame = m_world.FrameCount - 1;

            if (record != null && record.EqualsCmd(cmd))
            {
                sameCmdCache.frame = m_world.FrameCount;
                sameCmdCache.time = ClientTime.GetTime();
                sameCmdCache.id = entity.ID;

                if (NetworkManager.IsConnect)
                {
                    ProtocolAnalysisService.SendCommand(sameCmdCache);
                }

                //Debug.Log("send same " + m_world.FrameCount + " id " + sameCmdCache.id);
            }
            else
            {
                //Debug.Log("send cmd " + m_world.FrameCount + " id " + cmd.id);

                cmd.frame = m_world.FrameCount;
                cmd.time = ClientTime.GetTime();
                if (NetworkManager.IsConnect)
                {
                    ProtocolAnalysisService.SendCommand(cmd);
                }
            }
        }

        entity.ChangeComp(cmd);
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

    public virtual void BuildCommand(T command)
    {

    }
}
