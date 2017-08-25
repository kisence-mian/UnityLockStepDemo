using UnityEngine;
using System.Collections;

public class SkillData
{
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float m_CDSpace = 0;
    public int m_index = 0;
    public string m_skillID;
    public float m_lastExecuteTime = -1; //上次执行时间，用来作为CD判断
    //public SkillDataGenerate m_skillInfo;
    //public SkillStatusDataGenerate m_beforeInfo;
    //public SkillStatusDataGenerate m_currentInfo;
    //public SkillStatusDataGenerate m_laterInfo;

    public float m_beforeTime;
    public float m_hitTime;
    public float m_currentTime;
    public float m_laterTime;

    public SkillData(string skillID, int index = 0)
    {
        m_skillID = skillID;
        m_index = index;
        UpdateInfo();
        Reset();
    }

    #region 外部调用
     
    //public SkillDataGenerate GetInfo()
    //{
    //    return m_skillInfo;
    //}

    //开始cd
    public void BegionCD(float lastExecuteTime)
    {
        m_lastExecuteTime = lastExecuteTime + 0.4f;
    }

    /// <summary>
    /// 重置运行时数据
    /// </summary>
    public void Reset()
    {
        m_lastExecuteTime = -1;
    }

    ////判断CD结束
    //public bool CDFinished()
    //{
    //    if (GetCDResidueTime() <= 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
            
    //        return false;
    //    }
    //}

    ///// <summary>
    ///// 获取CD剩余时间
    ///// </summary>
    //public float GetCDResidueTime()
    //{
    //    if (m_lastExecuteTime < 0)
    //    {
    //        return 0;
    //    }

    //    float result = m_CDSpace - (SyncService.CurrentServiceTime - m_lastExecuteTime);

    //    if (result < 0)
    //    {
    //        result = 0;
    //    }

    //    return result;
    //}

    #endregion

    void UpdateInfo()
    {
        if (m_skillID != "" && m_skillID != "null")
        {
  
            //m_skillInfo = DataGenerateManager<SkillDataGenerate>.GetData(m_skillID);
            //m_beforeInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(m_skillInfo.m_BeforeStatus);
            //m_currentInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(m_skillInfo.m_CurrentStatus);
            //m_laterInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(m_skillInfo.m_LaterStatus);

            //m_beforeTime = m_beforeInfo.m_Time;

            //m_hitTime = m_beforeTime + m_skillInfo.m_HitTime;
            //m_currentTime = m_beforeTime + m_currentInfo.m_Time;
            //m_laterTime = m_currentTime + m_laterInfo.m_Time;
            //m_CDSpace = m_skillInfo.m_CD;

        }
    }

    public SkillData DeepCopy()
    {
        SkillData sd = new SkillData(m_skillID, m_index);
        sd.m_CDSpace = m_CDSpace;

        return sd;
    }
}
