using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatusComponent : MomentComponentBase
{
    public SkillStatusEnum m_skillStstus = SkillStatusEnum.None;

    public int m_skillTime = 0;
    public int m_skillTriggerTimeSpace = 0;

    public bool m_isHit = false;

    public bool m_isEnter = false;      //已经进入过改状态
    public bool m_isTriggerSkill = false; //是否计算过伤害

    public SyncVector3 skillDir = new SyncVector3();

    public SkillData m_currentSkillData;
    public List<SkillData> m_skillList = new List<SkillData>();

    public override MomentComponentBase DeepCopy()
    {
        SkillStatusComponent sc = new SkillStatusComponent();
        sc.m_skillStstus = m_skillStstus;

        sc.m_skillTime = m_skillTime;
        sc.m_skillTriggerTimeSpace = m_skillTriggerTimeSpace;

        sc.m_isHit = m_isHit;
        sc.m_isEnter = m_isEnter;
        sc.m_isTriggerSkill = m_isTriggerSkill;

        if (m_currentSkillData != null)
        {
            sc.m_currentSkillData = m_currentSkillData.DeepCopy();
        }

        for (int i = 0; i < m_skillList.Count; i++)
        {
            sc.m_skillList.Add(m_skillList[i].DeepCopy());
        }

        return sc;
    }

    public SkillData GetSkillData(string ID)
    {
        for (int i = 0; i < m_skillList.Count; i++)
        {
            if (m_skillList[i].m_skillID == ID)
            {
                return m_skillList[i];
            }
        }

        throw new Exception("DONT FIND SkillInfo skillID:" + ID + " by id: " + Entity.ID);
    }
}

public enum SkillStatusEnum
{
    None,
    Before,
    Current,
    Later,
    Finish,
}
