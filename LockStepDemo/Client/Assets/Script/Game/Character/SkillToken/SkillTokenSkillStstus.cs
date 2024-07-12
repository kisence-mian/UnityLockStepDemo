using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTokenSkillStstus : SkillStatus
{
    public override void OnSkillFinish()
    {
        CharacterManager.RemoveSkillToken((SkillToken)m_character);
    }

    public override void EnterSkillBefore()
    {
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Before);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Before);
    }

    public override void EnterSkillCurrent(float timeOffset)
    {
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Current);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Current);
    }

    public override void EnterSkillLater()
    {
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Later);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Later);
    }
}
