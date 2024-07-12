using FrameWork;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSystem<T> : ViewSystemBase where T : PlayerCommandBase, new()
{
    public override void Init()
    {
        Debug.Log("SyncSystem init ");

        GlobalEvent.AddTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.AddTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.AddTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.AddTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.AddTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.AddTypeEvent<T>(ReceviceCommandMsg);
        //GlobalEvent.AddTypeEvent<CommandMsg>(ReceviceCmdMsg);
    }

    public override void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.RemoveTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.RemoveTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.RemoveTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.RemoveTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.RemoveTypeEvent<T>(ReceviceCommandMsg);
        //GlobalEvent.RemoveTypeEvent<CommandMsg>(ReceviceCmdMsg);
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(T),
        };
    }

    #region 消息接收
    Deserializer deserializer = new Deserializer();

    void ReceviceStartSyncMsg(StartSyncMsg msg, params object[] objs)
    {
        Debug.Log("Start Msg " + msg.frame);

        m_world.FrameCount = msg.frame ;
        m_world.IsStart = true;
        m_world.EntityIndex = msg.createEntityIndex;
        m_world.SyncRule = msg.SyncRule;

        m_world.m_isRecalc = true;

        WorldManager.IntervalTime = msg.intervalTime;

        //立即执行创建操作
        m_world.LazyExecuteEntityOperation();

        //执行未处理的命令
        GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
        for (int i = 0; i < gdcc.m_noExecuteCommandList.Count; i++)
        {
            for (int j = 0; j < gdcc.m_noExecuteCommandList[i].msg.Count; j++)
            {
                RecordCommand(gdcc.m_noExecuteCommandList[i].msg[j]);
            }
        }

        m_world.m_isRecalc = false;

        AdvanceCalc(msg.frame + msg.advanceCount); //提前计算一帧

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        csc.aheadFrame = msg.advanceCount;
    }

    void RecevicePursueMsg(PursueMsg msg, params object[] objs)
    {
        Debug.Log("RecevicePursueMsg " + msg.frame + " recalcFrame " + msg.recalcFrame);

        //立即返回确认消息
        AffirmMsg amsg = new AffirmMsg();
        amsg.frame = msg.frame;
        amsg.time = msg.serverTime;
        ProtocolAnalysisService.SendCommand(amsg);

        PlayerCommandRecordComponent pcrc = m_world.GetEntity(msg.id).GetComp<PlayerCommandRecordComponent>();

        //for (int i = 0; i < msg.m_commandList.Count; i++)
        //{
        //    pcrc.RecordCommand(deserializer.Deserialize<T>( msg.m_commandList[i]));
        //}

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        //如果确定帧数落后则重计算
        if (msg.recalcFrame > csc.ClearFrame) 
        {
            Reson = "RecevicePursueMsg";
            Recalc(msg.recalcFrame);  //从接收到命令的第一帧开始重计算
            clearReson = "RecevicePursueMsg";
            AdvanceCalc(msg.frame + msg.advanceCount); //提前计算
        }

        csc.aheadFrame = msg.advanceCount;
    }

    void ReceviceSyncEntity(SyncEntityMsg msg, params object[] objs)
    {
        //Debug.Log("ReceviceSyncEntity frame " + msg.frame);

        //SyncDebugSystem.LogAndDebug("ReceviceSyncEntity frame " + msg.frame);

        if(m_world.IsStart)
        {
            ServerCacheComponent rc = m_world.GetSingletonComp<ServerCacheComponent>();

            ServiceMessageInfo info = new ServiceMessageInfo();
            info.m_frame = msg.frame;
            info.m_type = MessageType.SyncEntity;
            info.m_msg = msg;

            //TODO 服务器缓存 未清除
            rc.m_messageList.Add(info);

            Reson = "ReceviceSyncEntity";
            Recalc(msg.frame);
            clearReson = "ReceviceSyncEntity";
        }
        else
        {
            ExecuteSyncEntity(msg);
        }
    }

    void ReceviceAffirmMsg(AffirmMsg msg,params object[] objs)
    {
        //Debug.Log("ReceviceAffirmMsg");

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        csc.rtt = ClientTime.GetTime() - msg.time;
        
        if(csc.unConfirmFrame.Contains(msg.frame))
        {
            //Debug.Log("确认消息 " + msg.frame);
            csc.unConfirmFrame.Remove(msg.frame);
        }

        //TODO 服务器确认后这里可以清除下缓存
    }

    void ReceviceChangeSingletonCompMsg(ChangeSingletonComponentMsg msg,params object[] objs)
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

            Reson = "ReceviceChangeSingletonCompMsg";
            Recalc(msg.frame);
            clearReson = "ReceviceChangeSingletonCompMsg";
        }
        else
        {
            ExecuteChangeSingletonCompMsg(msg);
        }
    }

    void ReceviceCommandMsg(T cmd, params object[] objs)
    {
        //Debug.Log("ReceviceCommandMsg id" + cmd.id + " frame " + cmd.frame);

        //立即返回确认消息
        AffirmMsg amsg = new AffirmMsg();
        amsg.frame = cmd.frame;
        amsg.time = cmd.time;
        ProtocolAnalysisService.SendCommand(amsg);

        //已确认过的消息不再处理
        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        if (csc.confirmFrame.Contains(cmd.frame))
        {
            return;
        }
        else
        {
            csc.confirmFrame.Add(cmd.frame);
        }

        if (m_world.IsStart)
        {
            PlayerCommandRecordComponent pcrc = m_world.GetEntity(cmd.id).GetComp<PlayerCommandRecordComponent>();
            pcrc.lastInputFrame = cmd.frame;

            if (cmd.frame <= m_world.FrameCount)
            {
                //过去的命令
                //如果与本地预测结果不一致，则重新演算
                //不过要等到这一帧所有命令到齐才重计算

                PlayerCommandBase record = pcrc.GetInputCahae(cmd.frame);

                if(record == null || !record.EqualsCmd(cmd))
                {
                    pcrc.m_isConflict = true;
                }
                else
                {
                    pcrc.m_isConflict = false;
                }

                pcrc.RecordCommand(cmd);

                List<EntityBase> list = GetEntityList();

                bool isAllMessage = true;
                bool isConflict = false;
                for (int i = 0; i < list.Count; i++)
                {
                    PlayerCommandRecordComponent tmp = list[i].GetComp<PlayerCommandRecordComponent>();
                    isAllMessage &= (tmp.lastInputFrame >= m_world.FrameCount);
                    isConflict |= tmp.m_isConflict;
                }

                //Debug.Log("ReceviceCommandMsg id " + cmd.id + " frame " + cmd.frame + " isAllMessage " + isAllMessage + " isConflict " + isConflict + " pcrc.m_isConflict " + pcrc.m_isConflict);

                if (isAllMessage)
                {
                    if(csc.isAllFrame != cmd.frame - 1)
                    {
                        Debug.LogError("丢失 isAll ! isAllFrame:->" + csc.isAllFrame + " cmd.frame " + cmd.frame);
                        GlobalEvent.DispatchEvent(SyncDebugSystem.c_isConflict, cmd.frame);
                    }
                    else
                    {
                        Debug.Log("执行 isAll " + cmd.frame);
                        GlobalEvent.DispatchEvent(SyncDebugSystem.c_isAllMessage, cmd.frame);
                    }

                    csc.isAllFrame = cmd.frame;

                    if (isConflict)
                    {
                        if(cmd.frame > csc.ClearFrame)
                        {
                            m_world.eventSystem.ClearCache(cmd.frame);
                            //Debug.Log("消息不同重计算 ");
                            Reson = "ReceviceCommandMsg";
                            Recalc(cmd.frame);
                            clearReson = "ReceviceCommandMsg";
                        }
                        else
                        {
                            Debug.LogError("ReceviceCommandMsg 回滚到了确定帧 clearReson " + clearReson + " clearFrame " + csc.ClearFrame + " aimFrame " + cmd.frame +" id: " + cmd.id);
                        }
                    }
                    else
                    {
                        //此时派发确定性
                        m_world.eventSystem.DispatchCertainty(cmd.frame);
                    }
                }
            }
            else
            {
                pcrc.RecordCommand(cmd);
            }
        }
        else
        {
            //存入未执行命令列表
            //Debug.Log("存入未执行命令列表");
            GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
        }
    }

    //void ReceviceCmdMsg(CommandMsg msg,params object[] objs)
    //{
    //    //Debug.Log("ReceviceCmdMsg " + msg.frame);

    //    //立即返回确认消息
    //    AffirmMsg amsg = new AffirmMsg();
    //    amsg.frame = msg.frame;
    //    amsg.time = msg.serverTime;
    //    ProtocolAnalysisService.SendCommand(amsg);

    //    if (m_world.IsStart)
    //    {
    //        //TODO 如果全部都与本地预测相同则不再重计算
    //        for (int i = 0; i < msg.msg.Count; i++)
    //        {
    //            RecordCommand(msg.msg[i]);
    //        }

    //        if (m_world.FrameCount >= msg.frame)
    //        {
    //            Recalc(msg.frame);
    //        }
    //    }
    //    else
    //    {
    //        //存入未执行命令列表
    //        Debug.Log("存入未执行命令列表");
    //        GameDataCacheComponent gdcc = m_world.GetSingletonComp<GameDataCacheComponent>();
    //        gdcc.m_noExecuteCommandList.Add(msg);
    //    }
    //}

    void RecordCommand(CommandInfo cmd)
    {
        EntityBase entity = m_world.GetEntity(cmd.id);
        AddComp(entity);

        PlayerCommandRecordComponent pcrc = entity.GetComp<PlayerCommandRecordComponent>();
        pcrc.RecordCommand(cmd.ToCommand());
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

    #endregion

    #region 消息发送

    #endregion

    #region 同步重新演算

    //int isAllFrame = 0;
    //int clearFrame = 0;
    string clearReson = "";
    string Reson = "";

    /// <summary>
    /// 从目标帧开始重新演算
    /// </summary>
    /// <param name="frameCount"></param>
    public void Recalc(int frameCount)
    {
        //Debug.Log("重计算 " + frameCount + " " + m_world.FrameCount);
        //派发重计算
        GlobalEvent.DispatchEvent(SyncDebugSystem.c_Recalc, frameCount);

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        if (SyncDebugSystem.isDebug)
        {
            string content = "重计算 Recalc " + frameCount + " current " + m_world.FrameCount + "\n";
            SyncDebugSystem.syncLog += content;
        }

        if(frameCount - 1 < csc.ClearFrame)
        {
            string content = "回滚到了确定帧！！clearFrame " + csc.ClearFrame + " aimFrame " + (frameCount - 1) + " Reson "+ Reson + " ClearReson: " + clearReson + "\n";
            SyncDebugSystem.syncLog += content;
            Debug.LogError(content);
        }

        int aimCount = m_world.FrameCount;

        m_world.m_isRecalc = true;

        //回退到目标帧的上一帧，重新计算该帧
        m_world.RevertToFrame(frameCount - 1);

        //目标帧之后的历史记录作废
        m_world.ClearAfter(frameCount - 1);

        for (int i = frameCount; i <= aimCount; i++)
        {
            if (i == frameCount)
            {
                //只有这一帧是确定的
                m_world.m_isCertainty = true;
            }

            //重新演算
            m_world.Recalc(i,WorldManager.IntervalTime);

            //服务器数据改动,服务器给的是确切数据，所以放在重计算之后
            //TODO 有可能丢失派发事件
            ExecuteServiceMessage(i);

            //重新保存历史记录
            m_world.Record(i);

            m_world.m_isCertainty = false;
        }

        //Debug.Log("重计算 Recalc " + frameCount + " current " + m_world.FrameCount + " reson " + clearReson);

        //重计算的结果认定为最终结果，清除历史记录
        m_world.ClearBefore(frameCount - 2);

        m_world.m_isRecalc = false;

        m_world.EndRecalc();

        csc.ClearFrame = frameCount - 2;

    }

    /// <summary>
    /// 提前计算到目标帧
    /// </summary>
    /// <param name="frameCount"></param>
    public void AdvanceCalc(int frameCount)
    {
        if (SyncDebugSystem.isDebug)
        {
            string content = "提前计算  " + frameCount + " current " + m_world.FrameCount + "\n";
            SyncDebugSystem.syncLog += content;
        }

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
            if(frameCount == rc.m_messageList[i].m_frame)
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
        switch(info.m_type)
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
        for (int i = 0; i < msg.infos.Count; i++)
        {
            SyncEntity(msg.frame,msg.infos[i]);
        }

        for (int i = 0; i < msg.destroyList.Count; i++)
        {
            m_world.DestroyEntity(msg.destroyList[i]);
        }
    }

    void SyncEntity(int frame,EntityInfo info)
    {
        EntityBase entity;
        if (!m_world.GetEntityIsExist(info.id))
        {
            entity = m_world.CreateEntity(info.id);
        }
        else
        {
            entity = m_world.GetEntity(info.id);
        }

        for (int i = 0; i < info.infos.Count; i++)
        {
            ComponentBase comp = (ComponentBase)deserializer.Deserialize(info.infos[i].m_compName, info.infos[i].content);

            if (entity.GetExistComp(info.infos[i].m_compName))
            {
                entity.ChangeComp(info.infos[i].m_compName, comp);
            }
            else
            {
                entity.AddComp(info.infos[i].m_compName, comp);
            }

            //if (SyncDebugSystem.isDebug && SyncDebugSystem.IsFilter(info.infos[i].m_compName))
            {
                string content = "frame: " + frame + " id: " + info.id + " comp: " + info.infos[i].m_compName + " content: " + info.infos[i].content;
                //Debug.Log(content);
                //SyncDebugSystem.syncLog += content + "\n";
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
}