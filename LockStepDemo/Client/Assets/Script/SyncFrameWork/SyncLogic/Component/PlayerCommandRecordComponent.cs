using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCommandRecordComponent : ComponentBase
{
    public PlayerCommandBase m_defaultInput;
    public List<PlayerCommandBase> m_inputCache = new List<PlayerCommandBase>();  //输入缓存

    public bool m_isConflict = false;
    public int lastInputFrame = -1;

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

    public PlayerCommandBase GetForecastInput(int frame)
    {
        //取出上一帧的缓存赋值给下一帧做预测用
        PlayerCommandBase record = GetInputCahae(frame - 1);

        //没有则取默认值
        if(record == null)
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

        for (int i = 0; i < m_inputCache.Count; i++)
        {
            if(m_inputCache[i].frame == cmd.frame)
            {
                m_inputCache[i] = cmd;
                return;
            }
        }

        m_inputCache.Add(cmd); //本地没有记录，直接存入记录
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
    }
}
