using UnityEngine;
using System.Collections;

public class HurtModel 
{
    CharacterBase m_character;

    public virtual void Init(CharacterBase character)
    {
        m_character = character;
    }

    public virtual void Update()
    {

    }

    public virtual void BeRecover(RecoverCmd cmd)
    {
        m_character.m_Property.HP += cmd.m_recoverNumber;
    }

    public virtual void BeAttack(DamageCmd cmd)
    {
        BeSkill(cmd);
    }

    public virtual void BeSkill(DamageCmd cmd)
    {
        PlayHurtSFX(cmd);
        PlayHurFX(cmd);

        m_character.m_Property.HP -= cmd.m_damageNumber;

        //受到伤害打断Buff
        m_character.BeDamageInterruptBuff();

        if (cmd.m_skillID != null && cmd.m_attackerID == GameLogic.MyPlayerID)
        {
            //此处应该发送事件，让上层逻辑决定是否震屏
            string l_s_shokeID = DataGenerateManager<SkillDataGenerate>.GetData(cmd.m_skillID).m_HurtCameraShoke;
            DisplayManager.CameraShoke(l_s_shokeID);
        }

        //动画表现以及状态切换
        if (cmd.m_skillID != null && m_character.m_currentStatus.CanBreakBySkill(cmd.m_skillID))
        {
            //被打断了，播受伤效果、动画，进入伤害状态
            m_character.ChangeStatus(CharacterStatusEnum.Hurt);
            CharacterManager.Dispatch(cmd.m_characterID, CharacterEventType.BeBreak, m_character);
        }
        else
        {
            //没有被打断，继续之前的状态
        }
    }

    //public virtual void TrapDamage(TrapDamageCmd cmd)
    //{
    //    m_character.m_Property.HP -= cmd.m_damageNumber;

    //    //此处应该发送事件，让上层逻辑决定是否震屏
    //    //string l_s_shokeID = SkillInfoData.Instance.GetHurtCameraShoke(cmd.m_skillID);
    //    //DisplayManager.CameraShoke(l_s_shokeID);

    //    ////动画表现以及状态切换
    //    //if (BeBreak(cmd))
    //    //{
    //    //    //被打断了，播受伤效果、动画，进入伤害状态
    //    //    m_character.ChangeStatus(CharacterStatusEnum.Hurt);
    //    //}
    //    //else
    //    //{
    //    //    //没有被打断，继续之前的状态
    //    //}
    //}

    public virtual void BeFlyHurt(DamageCmd cmd)
    {

    }

    /// <summary>
    /// 播放受击音效
    /// </summary>
    protected void PlayHurtSFX(DamageCmd cmd)
    {
        if (cmd.m_buffID != null && cmd.m_buffID != "")
        {
            string hurtAudioName = DataGenerateManager<BuffDataGenerate>.GetData(cmd.m_buffID).m_BuffhitSFX;

            if (hurtAudioName != "null")
            {
                m_character.m_audio.PlayAudio(hurtAudioName);
            }
        }
        else if(cmd.m_skillID != null)
        {
            string hurtAudioName = DataGenerateManager<SkillDataGenerate>.GetData(cmd.m_skillID).m_HurtSFX;

            if (hurtAudioName != "null")
            {
                m_character.m_audio.PlayAudio(hurtAudioName);
            }
        }
    }

    protected void PlayHurFX(DamageCmd cmd)
    {
        if (cmd.m_skillID != null)
        {
            string hurtFXName = DataGenerateManager<SkillDataGenerate>.GetData(cmd.m_skillID).m_HitFXName;

            if (hurtFXName != "null")
            {
                HardPointEnum hitFXCreatPoint = DataGenerateManager<SkillDataGenerate>.GetData(cmd.m_skillID).m_HitFXCreatPoint;

                CharacterBase characterBase = CharacterManager.GetCharacter(cmd.m_characterID);

                m_character.m_materialComp.PlayHighLightFX();

                EffectManager.ShowEffect(hurtFXName, characterBase.m_hardPoint.GetHardPoint(hitFXCreatPoint).position, characterBase.transform.eulerAngles, 1);
            }
        }
    }
}
