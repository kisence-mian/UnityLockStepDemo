using UnityEngine;
using System.Collections;

public class StandardAttackStatus : AttackSatus
{

    public override void EnterAttackBefore()
    {
        DisplayManager.PlayCameraSkillAnim(m_currentAttackData.m_skillID, SkillStatusEnum.Before);
        DisplayManager.PlaySkillAnim(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Before);
        //DisplayManager.PlayAttackFX(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Before);
        m_character.m_effectComp.CreatSkillEffect(m_currentAttackData.m_skillID, SkillStatusEnum.Before);
        //DisplayManager.PlaySPX(m_currentAttackData.m_skillID, SkillStatusEnum.Before);

        m_character.m_audio.PlaySkillSFX(m_currentAttackData.m_skillID, SkillStatusEnum.Before);
    }

    public override void EnterAttackCurrent(float timeOffset)
    {
        string l_shiftID = m_currentAttackData.GetInfo().m_MoveID;
        m_character.m_moveComp.Shift(m_character.m_characterID, m_character.m_characterID, l_shiftID, timeOffset);
        DisplayManager.PlayCameraSkillAnim(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
        DisplayManager.PlaySkillAnim(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Current, m_isRaise);
       // DisplayManager.PlayAttackFX(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Current);
        m_character.m_effectComp.CreatSkillEffect(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
        //DisplayManager.PlaySPX(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
        m_character.m_audio.PlaySkillSFX(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
    }

    public override void InHitTime(float hitTime)
    {
        //FightLogicService.UseSkill(m_character, m_currentAttackData.m_skillID, hitTime);
    }

    public override void EnterAttackLater()
    {
        base.EnterAttackLater();
        DisplayManager.PlayCameraSkillAnim(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
        DisplayManager.PlaySkillAnim(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Later,isLater:true);
       // DisplayManager.PlayAttackFX(m_character, m_currentAttackData.m_skillID, SkillStatusEnum.Later);
        m_character.m_effectComp.CreatSkillEffect(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
        //DisplayManager.PlaySPX(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
        m_character.m_audio.PlaySkillSFX(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
    }

}
