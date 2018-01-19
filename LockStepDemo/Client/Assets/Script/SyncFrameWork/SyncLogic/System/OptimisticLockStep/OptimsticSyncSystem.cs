using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimsticSyncSystem<T> : SystemBase where T : PlayerCommandBase, new()
{
    public override void Init()
    {
        GlobalEvent.AddTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.AddTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.AddTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.AddTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.AddTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.AddTypeEvent<T>(ReceviceCommandMsg);
        GlobalEvent.AddTypeEvent<SameCommand>(ReceviceSameCmdMsg);
    }

    public override void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.RemoveTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.RemoveTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.RemoveTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.RemoveTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.RemoveTypeEvent<T>(ReceviceCommandMsg);
        GlobalEvent.RemoveTypeEvent<SameCommand>(ReceviceSameCmdMsg);
        //GlobalEvent.RemoveTypeEvent<CommandMsg>(ReceviceCmdMsg);
    }

    #region 消息接收
    Deserializer deserializer = new Deserializer();

    void ReceviceStartSyncMsg(StartSyncMsg msg, params object[] objs)
    {
        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        csc.confirmFrame = msg.frame; //从目标帧之后开始计算

        //Debug.Log("ReceviceStartSyncMsg " + csc.confirmFrame);

        m_world.FrameCount = msg.frame;
        m_world.IsStart = true;
        //m_world.EntityIndex = msg.createEntityIndex;
        m_world.SyncRule = msg.SyncRule;

        WorldManager.IntervalTime = msg.intervalTime;
        m_world.IsCertainty = true;
        //立即执行创建操作
        m_world.LazyExecuteEntityOperation();
        m_world.IsCertainty = false;

        m_world.m_RandomSeed = msg.randomSeed;

        m_world.ClearAll();

        //执行未处理的命令
        GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
        for (int i = 0; i < gdcc.m_noExecuteCommandList.Count; i++)
        {
            ReceviceCommandMsg((T)gdcc.m_noExecuteCommandList[i]);
        }

        m_world.Record(m_world.FrameCount);

        Recalc();
        AdvanceCalc(msg.frame + msg.advanceCount); //提前计算一帧

        csc.aheadFrame = msg.advanceCount;
    }

    void RecevicePursueMsg(PursueMsg msg, params object[] objs)
    {
        //Debug.Log("RecevicePursueMsg " + msg.frame + " UpdateSpped " + msg.updateSpeed);

        //调整游戏速度
        WorldManager.UpdateSpped = msg.updateSpeed;

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        csc.aheadFrame = msg.advanceCount;

        return;
    }

    void ReceviceSyncEntity(SyncEntityMsg msg, params object[] objs)
    {
        //Debug.Log("ReceviceSyncEntity frame " + msg.frame);
        //SyncDebugSystem.LogAndDebug("ReceviceSyncEntity frame " + msg.frame);

        if (m_world.IsStart)
        {
            ServerCacheComponent rc = m_world.GetSingletonComp<ServerCacheComponent>();

            ServiceMessageInfo info = new ServiceMessageInfo();
            info.m_frame = msg.frame;
            info.m_type = MessageType.SyncEntity;
            info.m_msg = msg;

            //TODO 服务器缓存 未清除,并且这里可能有问题，导致主动复活出错，原因应该是实体同步指令未执行，因为帧数已错过
            rc.m_messageList.Add(info);

            Recalc();
        }
        else
        {
            //List<int> idList = new List<int>();

            ////删除多余的实体
            //for (int i = 0; i < msg.infos.Count; i++)
            //{
            //    idList.Add(msg.infos[i].id);
            //}

            //for (int i = 0; i < m_world.m_entityList.Count; i++)
            //{
            //    if(!idList.Contains( m_world.m_entityList[i].ID))
            //    {
            //        m_world.DestroyEntityImmediately(m_world.m_entityList[i].ID);
            //    }
            //}

            ExecuteSyncEntity(msg);
        }
    }

    void ReceviceAffirmMsg(AffirmMsg msg, params object[] objs)
    {
        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        csc.rtt = ClientTime.GetTime() - msg.time;

        //TODO 服务器确认后这里可以清除下缓存
    }

    void ReceviceChangeSingletonCompMsg(ChangeSingletonComponentMsg msg, params object[] objs)
    {
        //SyncDebugSystem.LogAndDebug("ChangeSingletonCompMsg");

        if (m_world.IsStart)
        {
            ServerCacheComponent rc = m_world.GetSingletonComp<ServerCacheComponent>();

            ServiceMessageInfo info = new ServiceMessageInfo();
            info.m_frame = msg.frame;
            info.m_type = MessageType.ChangeSingletonComponent;
            info.m_msg = msg;

            rc.m_messageList.Add(info);

            Recalc();
        }
        else
        {
            ExecuteChangeSingletonCompMsg(msg);
        }
    }

    void ReceviceCommandMsg(T cmd, params object[] objs)
    {
        //Debug.Log("ReceviceCommandMsg frame " + cmd.frame + " frame " + Serializer.Serialize(cmd));

        if (SyncDebugSystem.isDebug)
            SyncDebugSystem.RecordMsg("cmd_commandComponent", cmd.frame, Serializer.Serialize(cmd));

        //立即返回确认消息
        AffirmMsg amsg = new AffirmMsg();
        amsg.index = cmd.frame;
        amsg.time = cmd.time;
        ProtocolAnalysisService.SendCommand(amsg);

        if (m_world.IsStart)
        {
            EntityBase entity = m_world.GetEntity(cmd.id);
            AddComp(entity); //自动添加记录组件

            PlayerCommandRecordComponent pcrc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
            PlayerCommandBase record = pcrc.GetInputCahae(cmd.frame);

            //判断和本地的预测有没有冲突
            if (record == null || !record.EqualsCmd(cmd))
            {
                pcrc.SetConflict(cmd.frame, true);
            }
            else
            {
                pcrc.SetConflict(cmd.frame, false);
            }

            if (pcrc.lastInputFrame < cmd.frame)
            {
                pcrc.lastInputFrame = cmd.frame;
            }

            pcrc.RecordCommand(cmd);

            //数据完整校验
            if (cmd.frame != 0 && pcrc.GetAllMessage(cmd.frame) && !pcrc.GetAllMessage(cmd.frame - 1))
            {
                ReSendMessage(cmd.frame - 1, cmd.id);
            }

            //Recalc();
        }
        else
        {
            GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
            gdcc.m_noExecuteCommandList.Add(cmd);
        }
    }

    void ReceviceSameCmdMsg(SameCommand cmd, params object[] objs)
    {
        if (m_world.IsStart)
        {
            EntityBase entity = m_world.GetEntity(cmd.id);
            AddComp(entity); //自动添加记录组件

            PlayerCommandRecordComponent pcrc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
            PlayerCommandBase record = pcrc.GetInputCahae(cmd.frame - 1);

            if (record != null)
            {
                PlayerCommandBase sameCmd = record.DeepCopy();
                sameCmd.frame = cmd.frame;
                sameCmd.time = cmd.time;
                ReceviceCommandMsg((T)sameCmd);
            }
            //缓存中没有数据，重新请求
            else
            {
                ReSendMessage(cmd.frame, cmd.id);
            }
        }
    }

    //void ReceviceCmdMsg(CommandMsg msg, params object[] objs)
    //{
    //    //Debug.Log("ReceviceCmdMsg " + msg.index + " currentframe " + m_world.FrameCount);

    //    //立即返回确认消息
    //    AffirmMsg amsg = new AffirmMsg();
    //    amsg.index = msg.index;
    //    amsg.time = msg.serverTime;
    //    ProtocolAnalysisService.SendCommand(amsg);

    //    if (m_world.IsStart)
    //    {
    //        //TODO 如果全部都与本地预测相同则不再重计算
    //        for (int i = 0; i < msg.msg.Count; i++)
    //        {
    //            //Debug.Log("RecordCommand " + Serializer.Serialize(msg.msg[i]));
    //            RecordCommand(msg.msg[i]);
    //        }

    //        Recalc();
    //    }
    //    else
    //    {
    //        //存入未执行命令列表
    //        //Debug.Log("存入未执行命令列表");
    //        //GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
    //        //gdcc.m_noExecuteCommandList.Add(msg);
    //    }
    //}

    //void RecordCommand(CommandInfo cmd)
    //{
    //    //EntityBase entity = m_world.GetEntity(cmd.id);
    //    //AddComp(entity); //自动添加记录组件

    //    //PlayerCommandRecordComponent pcrc = entity.GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);

    //    //PlayerCommandBase remote = cmd.ToCommand();
    //    //PlayerCommandBase record = pcrc.GetInputCahae(cmd.frame);


    //    //if (entity.GetExistComp<SelfComponent>())
    //    //{
    //    //    Debug.LogWarning("set is all");
    //    //}

    //    ////判断和本地的预测有没有冲突
    //    //if (record == null || !record.EqualsCmd(remote))
    //    //{
    //    //    if (entity.GetExistComp<SelfComponent>())
    //    //    {
    //    //        Debug.LogWarning("覆盖本地指令！ ");
    //    //    }

    //    //    pcrc.SetConflict(cmd.frame, true);
    //    //}
    //    //else
    //    //{
    //    //    pcrc.SetConflict(cmd.frame, false);
    //    //}

    //    //pcrc.RecordCommand(remote);
    //}

    public void AddComp(EntityBase entity)
    {
        if (!entity.GetExistComp(ComponentType.PlayerCommandRecordComponent))
        {
            PlayerCommandRecordComponent rc = new PlayerCommandRecordComponent();
            rc.m_defaultInput = new T();

            //自动添加记录组件
            entity.AddComp(rc);
        }
    }

    #endregion

    #region 消息发送

    #endregion

    #region 同步重新演算

    int pcFrame = 0;
    //int clearFrame = 0;
    //string clearReson = "";
    //string Reson = "";

    /// <summary>
    /// 从目标帧开始重新演算
    /// </summary>
    /// <param name="frameCount"></param>
    public override void Recalc()
    {
        //Debug.Log("Recalc " + m_world.FrameCount);

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        // 先判断回退的帧预测的数据和本地数据是否一致，一致则不重计算
        // 判断哪些帧是确定性数据，派发确定性
        // 其余帧继续做预测

        int frameCount = csc.confirmFrame;
        int aimCount = m_world.FrameCount;
        bool isAllMessage = true;
        bool isConflict = false;
        int allMessageFrame = frameCount;

        List<EntityBase> list = GetEntityList();

        //增加目标帧
        for (int i = aimCount + 1; ; i++)
        {
            if (list.Count == 0)
            {
                break;
            }

            for (int j = 0; j < list.Count; j++)
            {
                AddComp(list[j]);
                PlayerCommandRecordComponent tmp = list[j].GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
                isAllMessage &= tmp.GetAllMessage(i);
            }

            if (isAllMessage)
            {
                aimCount = i;
            }
            else
            {
                break;
            }
        }

        //先判断哪些帧不需要重计算
        for (int i = frameCount + 1; i <= aimCount; i++)
        {
            isAllMessage = true;
            isConflict = false;
            for (int j = 0; j < list.Count; j++)
            {
                AddComp(list[j]);
                PlayerCommandRecordComponent tmp = list[j].GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
                isAllMessage &= tmp.GetAllMessage(i);
                isConflict |= tmp.GetConflict(i);

                //Debug.Log("GetAllMessage " + i + " -> " + tmp.GetAllMessage(i) + " isAllMessage " + isAllMessage);
            }

            if (isAllMessage)
            {
                if (!isConflict)
                {
                    frameCount = i;
                    allMessageFrame = i;
                    m_world.IsCertainty = true;
                    //派发确定性
                    m_world.eventSystem.DispatchCertainty(i);
                    m_world.eventSystem.ClearCache(); //之后计算的事件作废
                    csc.confirmFrame = i;

                    m_world.IsCertainty = false;

                    //CheckCertaintyFrame(i);

                    //Debug.Log("不用重计算 frame " + i);
                }
                else
                {
                    //Debug.Log("需要重计算 frame " + i);
                    m_world.eventSystem.ClearCache(); //之前计算的事件作废
                    allMessageFrame = i;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //Debug.Log(" 重计算 Recalc frameCount -> " + frameCount + " aimCount　-> " + aimCount);

        //如果没有新的确定帧出现，则不再重计算
        if (frameCount == allMessageFrame)
        {
            //Debug.Log("没有新的确定帧出现，不再重计算");
            return;
        }

        //回退到最后一个确定帧
        m_world.RevertToFrame(frameCount);

        //目标帧之后的历史记录作废
        m_world.ClearAfter(frameCount);

        m_world.IsRecalc = true;
        isAllMessage = true;

        for (int i = frameCount + 1; i <= aimCount; i++)
        {
            //确定性不能中断
            if (isAllMessage)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    PlayerCommandRecordComponent tmp = list[j].GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
                    isAllMessage &= tmp.GetAllMessage(i);
                }

                if (isAllMessage)
                {
                    csc.confirmFrame = i;
                    m_world.IsCertainty = true;
                }
                else
                {
                    m_world.IsCertainty = false;
                }
            }

            if (m_world.IsCertainty)
            {
                //Debug.Log("确定帧 " + i);\
                m_world.eventSystem.ClearCacheAt(i);
            }
            else
            {
                //Debug.Log("预测帧 " + i);
            }

            //重新演算
            m_world.Recalc(i, WorldManager.IntervalTime);

            //服务器数据改动,服务器给的是确切数据，所以放在重计算之后
            ExecuteServiceMessage(i);

            m_world.ClearRecordAt(i);

            //重新保存历史记录
            m_world.Record(i);


            if (m_world.IsCertainty)
            {
                //Debug.Log("确定帧结束 " + i);
            }
            else
            {
                //Debug.Log("预测帧结束 " + i);
            }
        }

        m_world.EndRecalc();

        //重计算的结果认定为最终结果，清除历史记录
        m_world.ClearBefore(frameCount - 1);

        csc.ClearFrame = frameCount - 1;
        m_world.IsCertainty = false;
        m_world.IsRecalc = false;
    }

    public void CheckCertaintyFrame(int frame)
    {
        if (pcFrame != frame - 1)
        {
            //Debug.LogError("派发确定性中断 pcframe " + pcFrame + " frame " + frame);
        }

        pcFrame = frame;
    }

    /// <summary>
    /// 提前计算到目标帧
    /// </summary>
    /// <param name="frameCount"></param>
    public void AdvanceCalc(int frameCount)
    {

        for (int i = m_world.FrameCount + 1; i <= frameCount; i++)
        {
            //Debug.Log("提前计算 BEGIN " + m_world.FrameCount);

            //提前计算
            m_world.FixedLoop(WorldManager.IntervalTime);

            //重新保存历史记录
            m_world.Record(i);

            //Debug.Log("提前计算 END " + m_world.FrameCount);
        }

        //m_world.FrameCount = frameCount;
    }


    #region 读取并执行服务器数据

    void ExecuteServiceMessage(int frameCount)
    {
        List<ServiceMessageInfo> list = LoadMessage(frameCount);

        //Debug.Log("ExecuteServiceMessage count " + list.Count + " frame " + frameCount);

        for (int i = 0; i < list.Count; i++)
        {
            ExecuteMessgae(list[i]);
        }
    }

    #region 读取服务器消息

    List<ServiceMessageInfo> LoadMessage(int frameCount)
    {
        List<ServiceMessageInfo> list = new List<ServiceMessageInfo>();
        ServerCacheComponent rc = m_world.GetSingletonComp<ServerCacheComponent>();

        for (int i = 0; i < rc.m_messageList.Count; i++)
        {
            if (frameCount == rc.m_messageList[i].m_frame)
            {
                list.Add(rc.m_messageList[i]);
            }
        }

        return list;
    }

    #endregion

    #region 执行服务器消息

    void ExecuteMessgae(ServiceMessageInfo info)
    {
        switch (info.m_type)
        {
            case MessageType.ChangeSingletonComponent:
                ExecuteChangeSingletonCompMsg((ChangeSingletonComponentMsg)info.m_msg);
                break;
            case MessageType.SyncEntity:
                ExecuteSyncEntity((SyncEntityMsg)info.m_msg);
                break;
        }
    }

    void ExecuteSyncEntity(SyncEntityMsg msg)
    {
        m_world.IsCertainty = true;
        for (int i = 0; i < msg.infos.Count; i++)
        {
            SyncEntity(msg.frame, msg.infos[i]);
        }

        //for (int i = 0; i < msg.destroyList.Count; i++)
        //{
        //    m_world.DestroyEntity(msg.destroyList[i]);
        //}
        //TODO 有可能出问题
        m_world.IsCertainty = false;
    }

    void SyncEntity(int frame, EntityInfo info)
    {
        EntityBase entity;
        if (!m_world.GetEntityIsExist(info.id))
        {
            ComponentBase[] comps = new ComponentBase[info.infos.Count];

            for (int i = 0; i < info.infos.Count; i++)
            {
                comps[i] = (ComponentBase)deserializer.Deserialize(info.infos[i].m_compName, info.infos[i].content);
            }

            entity = m_world.CreateEntity("SyncEntity", info.id, comps);
        }
        else
        {
            entity = m_world.GetEntity(info.id);

            for (int i = 0; i < info.infos.Count; i++)
            {
                ComponentBase comp = (ComponentBase)deserializer.Deserialize(info.infos[i].m_compName, info.infos[i].content);

                //Debug.Log("comp name " + info.infos[i].m_compName);

                if (entity.GetExistComp(info.infos[i].m_compName))
                {
                    entity.ChangeComp(info.infos[i].m_compName, comp);
                }
                else
                {
                    entity.AddComp(info.infos[i].m_compName, comp);
                }
            }
        }
    }

    void ExecuteDestroyEntityMsg(DestroyEntityMsg msg)
    {
        m_world.DestroyEntity(msg.id);
    }

    void ExecuteChangeSingletonCompMsg(ChangeSingletonComponentMsg msg)
    {
        //Debug.Log("msg.info.m_compName " + msg.info.m_compName + " " + Type.GetType(msg.info.m_compName));

        SingletonComponent comp = (SingletonComponent)deserializer.Deserialize(msg.info.m_compName, msg.info.content);
        m_world.ChangeSingleComp(msg.info.m_compName, comp);
    }
    #endregion

    #endregion

    #endregion

    #region 消息补齐

    void ReSendMessage(int frame, int id)
    {
        QueryCommand qc = new QueryCommand();
        qc.frame = frame;
        qc.id = id;

        ProtocolAnalysisService.SendCommand(qc);
    }

    #endregion
}
