using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConnectionComponent : ServiceComponent
{
    public string playerID;

    public bool m_isWaitPushStart = false;
    public SyncSession m_session;

    public List<int> unConfirmFrame = new List<int>(); //未确认的帧
    //public List<int> confirmFrame = new List<int>();   //已确认的帧

    public int rtt;                 //网络时延 单位ms
    public int lastInputFrame = -1; //玩家的最后一次输入帧

    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();
    public List<PlayerCommandBase> m_forecastList = new List<PlayerCommandBase>(); //预测操作列表
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
            m_forecastList.Add(pb);

            return pb;
        }
        else
        {
            for (int i = 0; i < m_commandList.Count; i++)
            {
                if (m_commandList[i].frame == frame)
                {
                    m_lastInputCache = m_commandList[i];

                    m_commandList.RemoveAt(i);

                    return m_lastInputCache;
                }
            }

            PlayerCommandBase pb = GetForecast(frame);
            m_forecastList.Add(pb);

            return pb;
        }
    }

    public PlayerCommandBase GetForecast(int frame)
    {
        //Debug.Log("预测操作 id:" + Entity.ID + " frame " + frame);
        PlayerCommandBase cmd = null;
        if (m_lastInputCache == null)
        {
            cmd = m_defaultInput.DeepCopy();
        }
        else
        {
            cmd = m_lastInputCache.DeepCopy();
        }

        //PlayerCommandBase cmd = new CommandComponent();
        cmd.frame = frame;
        cmd.id = Entity.ID;
        return cmd;
    }

    public PlayerCommandBase GetHistoryForecast(int frame)
    {
        for (int i = 0; i < m_forecastList.Count; i++)
        {
            if (m_forecastList[i].frame == frame)
            {

                return m_forecastList[i];
            }
        }

        m_defaultInput.id = Entity.ID;
        m_defaultInput.frame = frame;
        return m_defaultInput;
    }

    public void ClearForecast(int frame)
    {
        for (int i = 0; i < m_forecastList.Count; i++)
        {
            if (m_forecastList[i].frame <= frame)
            {
                m_forecastList.RemoveAt(i);
                i--;
            }
        }
    }
}
