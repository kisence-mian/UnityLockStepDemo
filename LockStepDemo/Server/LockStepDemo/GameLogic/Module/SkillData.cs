using UnityEngine;
using System.Collections;

public class SkillData
{
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    private float cDSpace = 0;
    private int index = 0;
    public string m_skillID;
    private float lastExecuteTime = -1; //上次执行时间，用来作为CD判断

    private SkillDataGenerate skillInfo;
    private SkillStatusDataGenerate beforeInfo;
    private SkillStatusDataGenerate currentInfo;
    private SkillStatusDataGenerate laterInfo;

    private float beforeTime;
    private float hitTime;
    private float currentTime;
    private float laterTime;

    public SkillDataGenerate SkillInfo
    {
        get
        {
            if (skillInfo == null)
            {
                skillInfo = DataGenerateManager<SkillDataGenerate>.GetData(m_skillID);
            }
            return skillInfo;
        }

        set
        {
            skillInfo = value;
        }
    }

    public SkillStatusDataGenerate BeforeInfo
    {
        get
        {
            if (beforeInfo == null)
            {
                beforeInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_BeforeStatus);
            }

            return beforeInfo;
        }

        set
        {
            beforeInfo = value;
        }
    }

    public SkillStatusDataGenerate CurrentInfo
    {
        get
        {
            if (currentInfo == null)
            {
                currentInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_CurrentStatus);
            }

            return currentInfo;
        }

        set
        {
            currentInfo = value;
        }
    }

    public SkillStatusDataGenerate LaterInfo
    {
        get
        {
            if (laterInfo == null)
            {
                laterInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_LaterStatus);
            }

            return laterInfo;
        }

        set
        {
            laterInfo = value;
        }
    }

    public float BeforeTime
    {
        get
        {
            return beforeTime;
        }

        set
        {
            beforeTime = value;
        }
    }

    public float HitTime
    {
        get
        {
            return hitTime;
        }

        set
        {
            hitTime = value;
        }
    }

    public float CurrentTime
    {
        get
        {
            return currentTime;
        }

        set
        {
            currentTime = value;
        }
    }

    public float LaterTime
    {
        get
        {
            return laterTime;
        }

        set
        {
            laterTime = value;
        }
    }

    public float CDSpace
    {
        get
        {
            return cDSpace;
        }

        set
        {
            cDSpace = value;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public float LastExecuteTime
    {
        get
        {
            return lastExecuteTime;
        }

        set
        {
            lastExecuteTime = value;
        }
    }

    public SkillData()
    {
    }

    public SkillData(string skillID, int index = 0)
    {
        m_skillID = skillID;
        Index = index;
        UpdateInfo();
        Reset();
    }

    #region 外部调用

    /// <summary>
    /// 重置运行时数据
    /// </summary>
    public void Reset()
    {
        LastExecuteTime = -1;
    }


    #endregion

    public void UpdateInfo()
    {
        if (m_skillID != "" && m_skillID != "null")
        {

            SkillInfo = DataGenerateManager<SkillDataGenerate>.GetData(m_skillID);
            BeforeInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_BeforeStatus);
            CurrentInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_CurrentStatus);
            LaterInfo = DataGenerateManager<SkillStatusDataGenerate>.GetData(SkillInfo.m_LaterStatus);

            BeforeTime = BeforeInfo.m_Time;

            HitTime = BeforeTime + SkillInfo.m_HitTime;
            CurrentTime = BeforeTime + CurrentInfo.m_Time;
            LaterTime = CurrentTime + LaterInfo.m_Time;
            CDSpace = SkillInfo.m_CD;

        }
    }

    public SkillData DeepCopy()
    {
        SkillData sd = new SkillData(m_skillID, Index);
        sd.CDSpace = CDSpace;

        return sd;
    }
}
