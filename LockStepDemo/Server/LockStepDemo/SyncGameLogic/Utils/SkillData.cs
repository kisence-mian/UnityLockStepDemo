using UnityEngine;
using System.Collections;

public class SkillData
{
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    private int index = 0;
    public string m_skillID;
    public int cd = 0; //上次执行时间，用来作为CD判断

    private SkillDataGenerate skillInfo;
    private SkillStatusDataGenerate beforeInfo;
    private SkillStatusDataGenerate currentInfo;
    private SkillStatusDataGenerate laterInfo;

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

    public int BeforeTime
    {
        get
        {
            return BeforeInfo.m_Time;
        }
    }

    public int HitTime
    {
        get
        {
            return BeforeTime + SkillInfo.m_HitTime;
        }
    }

    public int CurrentTime
    {
        get
        {
            return BeforeTime + CurrentInfo.m_Time;
        }
    }

    public int LaterTime
    {
        get
        {
            return CurrentTime + LaterInfo.m_Time;
        }
    }

    public int CDSpace
    {
        get
        {
            return SkillInfo.m_CD;
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

    public SkillDataGenerate GetInfo()
    {
        return SkillInfo;
    }

    //开始cd
    public void BegionCD(float lastExecuteTime)
    {
        //cd = 0;
    }

    /// <summary>
    /// 重置运行时数据
    /// </summary>
    public void Reset()
    {
        cd = 0;
    }

    //判断CD结束
    public bool CDFinished()
    {
        return cd <= 0;
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
        }
    }

    public SkillData DeepCopy()
    {
        SkillData sd = new SkillData(m_skillID, Index);
        sd.cd = cd;
        return sd;
    }
}
