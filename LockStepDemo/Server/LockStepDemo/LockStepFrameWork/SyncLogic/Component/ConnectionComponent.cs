using DeJson;
using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConnectionComponent : ServiceComponent
{
    public string m_playerID;

    public bool m_isWaitPushStart = false;
    public SyncSession m_session;
    public int m_sendIndex = 0;

    public Dictionary<int, CommandMsg> m_unConfirmFrame = new Dictionary<int, CommandMsg>(); //未确认的帧

    public int rtt;                 //网络时延 单位ms
    public int lastInputFrame = -1; //玩家的最后一次输入帧
    public float UpdateSpeed  = 1;  //世界更新速度

    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();
    //public List<PlayerCommandBase> m_forecastList = new List<PlayerCommandBase>(); //预测操作列表
    public PlayerCommandBase m_defaultInput = null;   //默认输入
    public PlayerCommandBase m_lastInputCache = null; //玩家的最后一次输入

    public List<EntityBase> m_waitSyncEntity = new List<EntityBase>(); //等待同步的实体
    public List<int> m_waitDestroyEntity = new List<int>();            //等待同步删除的实体

    public PlayerCommandBase GetCommand(int frame)
    {
        //没有收到玩家输入复制玩家的最后一次输入
        if (m_commandList.Count == 0)
        {
            PlayerCommandBase pb = GetForecast(frame);
            return pb;
        }
        else
        {
            for (int i = 0; i < m_commandList.Count; i++)
            {
                if (m_commandList[i].frame == frame)
                {
                    m_lastInputCache = m_commandList[i];
                    return m_lastInputCache;
                }
            }

            PlayerCommandBase pb = GetForecast(frame);
            return pb;
        }
    }

    public PlayerCommandBase GetForecast(int frame)
    {
        //Debug.Log("预测操作 id:" + Entity.ID + " frame " + frame);
        PlayerCommandBase cmd = null;
        if (m_lastInputCache == null)
        {
            //TODO defaultInput 没了
            if (m_defaultInput == null)
            {
                Debug.LogError("m_defaultInput is null");

                m_defaultInput = new CommandComponent();
            }

            cmd = m_defaultInput.DeepCopy();
        }
        else
        {
            cmd = m_lastInputCache.DeepCopy();
        }

        cmd.frame = frame;
        cmd.id = Entity.ID;
        return cmd;
    }

    public CommandMsg GetUnConfirmFrame(int index)
    {
        return m_unConfirmFrame[index];
    }

    public CommandMsg GetCommandMsg(int frame)
    {
        CommandMsg cmsg = new CommandMsg();
        cmsg.serverTime = ServiceTime.GetServiceTime();
        cmsg.msg = new List<CommandInfo>();

        //把前三帧的数据也打包进去
        for (int i = 0; i < 1; i++)
        {
            CommandComponent cc = (CommandComponent)GetCommand(frame - i);
            if (!cmsg.GetIsExist(cc.frame, cc.id))
            {
                CommandInfo infoTmp = new CommandInfo();
                infoTmp.FromCommand(cc);
                cmsg.msg.Add(infoTmp);
            }
        }

        return cmsg;
    }

    public int GetSendIndex()
    {
        return m_sendIndex++;
    }

    //public PlayerCommandBase GetHistoryForecast(int frame)
    //{
    //    for (int i = 0; i < m_forecastList.Count; i++)
    //    {
    //        if (m_forecastList[i].frame == frame)
    //        {

    //            return m_forecastList[i];
    //        }
    //    }

    //    m_defaultInput.id = Entity.ID;
    //    m_defaultInput.frame = frame;
    //    return m_defaultInput;
    //}

    //public void ClearForecast(int frame)
    //{
    //    for (int i = 0; i < m_forecastList.Count; i++)
    //    {
    //        if (m_forecastList[i].frame <= frame)
    //        {
    //            m_forecastList.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //}
}
