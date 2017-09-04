using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ConnectionComponent : ServiceComponent
{
    public bool m_isWaitPushStart = false;
    public SyncSession m_session;
    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();
    public List<PlayerCommandBase> m_forecastList = new List<PlayerCommandBase>(); //预测操作列表

    public PlayerCommandBase m_lastInputCache = null; //玩家的最后一次输入
    public List<EntityBase> m_waitSyncEntity = new List<EntityBase>(); //等待同步的实体

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
                    m_lastInputCache = m_commandList[0];

                    m_commandList.RemoveAt(0);

                    return m_lastInputCache;
                }
            }

            return GetForecast(frame);
        }
    }

    public PlayerCommandBase GetForecast(int frame)
    {
        Debug.Log("预测操作 id:" + Entity.ID + " frame " + frame);

        PlayerCommandBase cmd = m_lastInputCache.DeepCopy();
        cmd.frame = frame;
        return cmd;
    }
}
