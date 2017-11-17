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
    //public int m_sendIndex = 0;

    //public Dictionary<int, CommandMsg> m_unConfirmFrame = new Dictionary<int, CommandMsg>(); //未确认的帧

    public int rtt;                 //网络时延 单位ms
    private int lastInputFrame = -1; //玩家的最后一次输入帧
    public float UpdateSpeed  = 1;  //世界更新速度

    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();
    //public List<PlayerCommandBase> m_forecastList = new List<PlayerCommandBase>(); //预测操作列表
    public PlayerCommandBase m_defaultInput = null;   //默认输入
    //public PlayerCommandBase m_lastInputCache = null; //玩家的最后一次输入

    public List<EntityBase> m_waitSyncEntity = new List<EntityBase>(); //等待同步的实体
    public List<int> m_waitDestroyEntity = new List<int>();            //等待同步删除的实体

    public int LastInputFrame
    {
        get => lastInputFrame;
        set
        {
            if(value > lastInputFrame)
            {
                lastInputFrame = value;
            }
        }
    }

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
                    return m_commandList[i];
                }
            }

            PlayerCommandBase pb = GetForecast(frame);
            return pb;
        }
    }

    public PlayerCommandBase GetForecast(int frame)
    {
        //Debug.Log("预测操作 id:" + Entity.ID + " frame " + frame);
        PlayerCommandBase cmd = GetLastInput();

        cmd.frame = frame;
        cmd.id = Entity.ID;
        return cmd;
    }

    public PlayerCommandBase GetLastInput()
    {
        if(m_commandList.Count != 0)
        {
            try
            {
                return m_commandList[m_commandList.Count - 1].DeepCopy();

            }catch(Exception e)
            {
                Debug.LogError("m_commandList.Count - 1 " + (m_commandList.Count - 1) + " m_commandList[m_commandList.Count - 1] ->" + m_commandList[m_commandList.Count - 1] + "<-" + e.ToString());
            }

        }

        return GetDefaultInput();
    }

    public PlayerCommandBase GetDefaultInput()
    {
        //TODO defaultInput 没了
        if (m_defaultInput == null)
        {
            Debug.LogError("m_defaultInput is null");

            m_defaultInput = new CommandComponent();
        }

        return m_defaultInput;
    }

    public void AddCommand(PlayerCommandBase cmd)
    {
        LastInputFrame = cmd.frame;

        for (int i = 0; i < m_commandList.Count; i++)
        {
            if (m_commandList[i].frame == cmd.frame)
            {
                m_commandList[i] = cmd;
                return;
            }
        }

        m_commandList.Add(cmd);
    }
}
