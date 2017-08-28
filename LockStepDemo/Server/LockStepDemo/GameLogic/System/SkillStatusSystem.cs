using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillStatusSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SkillStatusComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            SkillStatusLogic(list[i], deltaTime);
        }
    }

    public void SkillStatusLogic(EntityBase entity, int deltaTime)
    {
        SkillStatusComponent sc = entity.GetComp<SkillStatusComponent>();

        if (sc.m_skillStstus != SkillStatusEnum.Finish
            && sc.m_skillStstus != SkillStatusEnum.None)
        {
            sc.m_skillTime += deltaTime;

            if (sc.m_skillTime > sc.m_currentSkillData.m_beforeTime * 1000)
            {
                if (sc.m_skillStstus != SkillStatusEnum.Current)
                {
                    sc.m_isEnter = true;
                    sc.m_skillStstus = SkillStatusEnum.Current;
                }

            }
            if (sc.m_skillTime > sc.m_currentSkillData.m_hitTime * 1000)
            {
                if (sc.m_currentSkillData.m_skillInfo.m_Moment)
                {
                    if (sc.m_isTriggerSkill == false)
                    {
                        sc.m_isTriggerSkill = true;
                        sc.m_isHit = true;
                        Debug.Log("set true");
                    }
                    else
                    {
                        Debug.Log("set false");
                        sc.m_isHit = false;
                    }
                }
                else
                {
                    sc.m_skillTriggerTimeSpace -= deltaTime;
                    if (sc.m_skillTriggerTimeSpace < 0)
                    {
                        sc.m_skillTriggerTimeSpace = (int)(sc.m_currentSkillData.m_skillInfo.m_TriggerSpaceTime) * 1000;
                        //加个伤害间隔
                        sc.m_isHit = true;
                    }
                    else
                    {
                        sc.m_isHit = false;
                    }
                }
            }

            if (sc.m_skillTime > sc.m_currentSkillData.m_currentTime * 1000)
            {
                if (sc.m_skillStstus != SkillStatusEnum.Later)
                {
                    sc.m_isEnter = true;
                    sc.m_skillStstus = SkillStatusEnum.Later;
                }
            }

            if (sc.m_skillTime > sc.m_currentSkillData.m_laterTime * 1000)
            {
                if (sc.m_skillStstus != SkillStatusEnum.Finish)
                {
                    sc.m_isEnter = true;
                    sc.m_skillStstus = SkillStatusEnum.Finish;
                }
            }
        }
    }
}
