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
    public SyncVector3 skillPos = new SyncVector3();

    public SyncVector3 skillDirCache = new SyncVector3();

    public SkillData m_currentSkillData;
    public List<SkillData> m_skillList = new List<SkillData>();
    public List<int> m_CDList = new List<int>();

    //表现用
    public int FXTimer = 0;
    public bool isTriggerFX;

    public override MomentComponentBase DeepCopy()
    {
        SkillStatusComponent sc = new SkillStatusComponent();
        sc.m_skillStstus = m_skillStstus;

        sc.m_skillTime = m_skillTime;
        sc.m_skillTriggerTimeSpace = m_skillTriggerTimeSpace;

        sc.m_isHit = m_isHit;
        sc.m_isEnter = m_isEnter;
        sc.m_isTriggerSkill = m_isTriggerSkill;
        sc.skillPos = skillPos.DeepCopy();

        sc.skillDir = skillDir.DeepCopy();
        sc.skillDirCache = skillDirCache.DeepCopy();

        sc.m_currentSkillData = m_currentSkillData.DeepCopy();

        for (int i = 0; i < m_CDList.Count; i++)
        {
            sc.m_CDList.Add(m_CDList[i]);
        }

        sc.m_skillList = m_skillList;

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

        throw new Exception("DONT FIND SkillInfo skillID:->" + ID + "<- by id: " + Entity.ID);
    }

    public bool GetSkillCDFinsih(string skillID)
    {
        for (int i = 0; i < m_skillList.Count; i++)
        {
            if (m_skillList[i].m_skillID == skillID)
            {
                return m_CDList[i] <= 0;
            }
        }

        throw new Exception("DONT FIND CD skillID:->" + ID + "<- by id: " + Entity.ID);
    }

    public int GetSkillCD(string skillID)
    {
        for (int i = 0; i < m_skillList.Count; i++)
        {
            if (m_skillList[i].m_skillID == skillID)
            {
                return m_CDList[i];
            }
        }

        throw new Exception("DONT FIND CD skillID:->" + ID + "<- by id: " + Entity.ID);
    }

    public void SetSkillCD(string skillID, int CD)
    {
        for (int i = 0; i < m_skillList.Count; i++)
        {
            if (m_skillList[i].m_skillID == skillID)
            {
                m_CDList[i] = CD;
                return;
            }
        }

        throw new Exception("DONT FIND CD skillID:->" + skillID + "<- by id: " + Entity.ID);
    }

    //public override int ToHash()
    //{
    //    //return Serializer.Serialize(this).ToHash();
    //}
}

public enum SkillStatusEnum
{
    None,
    Before,
    Current,
    Later,
    Finish,
}
