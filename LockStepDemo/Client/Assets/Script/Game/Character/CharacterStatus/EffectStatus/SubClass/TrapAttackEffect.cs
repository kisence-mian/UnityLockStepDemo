using UnityEngine;
using System.Collections;

public class TrapAttackEffect : StandardAttackStatus {

    CreatMesh RangeTip;

    public override void EnterAttackBefore()
    {
        base.EnterAttackBefore();

        if (m_currentAttackData.GetInfo().m_IsAreaTip)
        {
            if (RangeTip == null)
            {
                GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
                RangeTip = tip.GetComponent<CreatMesh>();
            }
            RangeTip.gameObject.SetActive(true);
            RangeTip.SetMesh(m_character, m_currentAttackData.GetInfo().m_EffectArea, true, m_currentAttackData.GetInfo().m_AreaTexture);
        }
    }

    public override void EnterAttackCurrent(float timeOffset)
    {
        m_character.m_animComp.m_animator.SetTrigger("triggerAnim");

        m_character.m_effectComp.CreatSkillEffect(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
        m_character.m_audio.PlaySkillSFX(m_currentAttackData.m_skillID, SkillStatusEnum.Current);
    }

    public override void EnterAttackLater()
    {
        m_character.m_effectComp.CreatSkillEffect(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
        m_character.m_audio.PlaySkillSFX(m_currentAttackData.m_skillID, SkillStatusEnum.Later);
    }

    public override void InHitTime(float hitTime)
    {
        if (RangeTip != null)
        {
            RangeTip.gameObject.SetActive(false);
        }
        base.InHitTime(hitTime);
    }

    public override void EnterAttackNone()
    {
        if (RangeTip != null)
        {
            GameObjectManager.DestroyGameObjectByPool(RangeTip.gameObject);
            RangeTip = null;
        }

        base.EnterAttackNone();
    }

}
