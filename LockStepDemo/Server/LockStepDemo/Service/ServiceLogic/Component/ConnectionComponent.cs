using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.ServiceLogic
{
    public class ConnectionComponent : ServiceComponent
    {
        public SyncSession m_session;
        public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();

        public PlayerCommandBase m_lastInputCache = null; //玩家的最后一次输入

        public PlayerCommandBase GetCommand(int frame)
        {
            //没有收到玩家输入复制玩家的最后一次输入
            if(m_commandList.Count == 0)
            {
                return m_lastInputCache;
            }
            else
            {
                m_lastInputCache = m_commandList[0];

                m_commandList.RemoveAt(0);

                return m_lastInputCache;
            }
        }
    }
}
