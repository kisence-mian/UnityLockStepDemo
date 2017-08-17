using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerCommandRecordComponent : ComponentBase
{
    public PlayerCommandBase m_forecastInput; //当前帧预测输入
    public PlayerCommandBase m_lastInput;     //最后一次输入

    public List<PlayerCommandBase> m_inputCache = new List<PlayerCommandBase>();  //历史输入
    public List<PlayerCommandBase> m_serverCache = new List<PlayerCommandBase>(); //服务器缓存

    public PlayerCommandBase GetInputCahae(int frame)
    {
        for (int i = 0; i < m_inputCache.Count; i++)
        {
            if(m_inputCache[i].frame == frame)
            {
                return m_inputCache[i];
            }
        }

        return null;
    }

    public PlayerCommandBase GetServerCache(int frame)
    {
        for (int i = 0; i < m_serverCache.Count; i++)
        {
            if (m_serverCache[i].frame == frame)
            {
                return m_serverCache[i];
            }
        }

        return null;
    }

    public void ReplaceCommand(PlayerCommandBase cmd)
    {
        for (int i = 0; i < m_inputCache.Count; i++)
        {
            if(m_inputCache[i].frame == cmd.frame)
            {
                m_inputCache[i] = cmd;
                //TODO 这里可能产生不同步
                m_lastInput = cmd;        //最后的输入也改变了
                return;
            }
        }

        string content = "";

        for (int i = 0; i < m_inputCache.Count; i++)
        {
            content += "id " + m_inputCache[i].id + " frame " + m_inputCache[i].frame + "\n";
        }

        throw new Exception("ReplaceCommand faild ! id:->" + cmd.id + " frame:-> " + cmd.frame + " Count " + m_inputCache.Count + "\n" + content);
    }

    public void ClearCache(int frame)
    {
        for (int i = 0; i < m_inputCache.Count; i++)
        {
            if (m_inputCache[i].frame < frame)
            {
                m_inputCache.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < m_serverCache.Count; i++)
        {
            if (m_serverCache[i].frame < frame)
            {
                m_serverCache.RemoveAt(i);
                i--;
            }
        }
    }
}
