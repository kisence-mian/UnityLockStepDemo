using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCommandRecordComponent : ComponentBase
{
    public PlayerCommandBase m_defaultInput;
    public Dictionary<int,PlayerCommandBase> m_inputCache = new Dictionary<int, PlayerCommandBase>();  //输入缓存
    public Dictionary<int, bool> m_conflictCache = new Dictionary<int, bool>();    //冲突缓存

    public bool m_isConflict = false;
    public int lastInputFrame = -1;

    public PlayerCommandBase GetInputCahae(int frame)
    {
        if(m_inputCache.ContainsKey(frame))
        {
            return m_inputCache[frame];
        }

        return null;
    }

    public PlayerCommandBase GetForecastInput(int frame)
    {
        //取出上一帧的缓存赋值给下一帧做预测用
        PlayerCommandBase record = GetInputCahae(lastInputFrame);

        //没有则取默认值
        if (record == null)
        {
            record = m_defaultInput;
        }

        PlayerCommandBase cmd = record.DeepCopy();

        cmd.frame = frame;
        cmd.id = Entity.ID;

        return cmd;
    }

    public void RecordCommand(PlayerCommandBase cmd)
    {
        //Debug.Log("记录操作 id:" + cmd.id + " frame " + cmd.frame);

        if(m_inputCache.ContainsKey(cmd.frame))
        {
            m_inputCache[cmd.frame] = cmd;
        }
        else
        {
            m_inputCache.Add(cmd.frame, cmd);
        }
    }

    public void ClearCache(int frame)
    {
        //for (int i = 0; i < m_inputCache.Count; i++)
        //{
        //    if (m_inputCache[i].frame < frame)
        //    {
        //        m_inputCache.RemoveAt(i);
        //        i--;
        //    }
        //}
    }

    public void SetConflict(int frame, bool isConfict)
    {
        if (m_conflictCache.ContainsKey(frame))
        {
            if(isConfict)
            {
                m_conflictCache[frame] = isConfict;
            }
        }
        else
        {
            m_conflictCache.Add(frame, isConfict);
        }
    }
    public bool GetConflict(int frame)
    {
        if (m_conflictCache.ContainsKey(frame))
        {
            return m_conflictCache[frame];
        }
        else
        {
            return false;
        }
    }

    public bool GetAllMessage(int frame)
    {
        if (m_conflictCache.ContainsKey(frame))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
