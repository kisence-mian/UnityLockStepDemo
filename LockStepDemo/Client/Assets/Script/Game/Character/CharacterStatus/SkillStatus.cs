using UnityEngine;
using System.Collections;
using System;

public class SkillStatus : CharacterBaseStatus 
{
    SkillStatusEnum m_skillStstus = SkillStatusEnum.None;

    public float m_skillTime = 0;
    public float m_skillTriggerTimeSpace = 0;
    
    public SkillData m_currentSkillData; //运行时的信息在Data中

    //bool m_enterBefored = false; //已经进入过before
    bool m_enterCurrented = false; //已经进入过current
    bool m_isTriggerSkill = false; //是否计算过伤害
    bool m_enterLatered = false;//已经进入过later

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Skill;
    }

    public override void OnUpdate()
    {
        if (m_skillStstus != SkillStatusEnum.Finish
            && m_skillStstus != SkillStatusEnum.None)
        {
            m_skillTime += Time.deltaTime;

            if (m_skillTime > m_currentSkillData.BeforeTime)
            {
                if (m_enterCurrented == false)
                {
                    m_enterCurrented = true;
                    m_skillStstus = SkillStatusEnum.Current;
                    EnterSkillCurrent(GetCurrentPursueTime());
                }
            }
            if (m_skillTime > m_currentSkillData.HitTime - SyncService.SyncAheadTime)
            {
                
                if (m_currentSkillData.SkillInfo.m_Moment)
                {
                    if (m_isTriggerSkill == false)
                    {
                        m_isTriggerSkill = true;
                        InHitTime(SyncService.CurrentServiceTime);
                    }
                }
                else
                {
                    m_skillTriggerTimeSpace -= Time.deltaTime;
                    if(m_skillTriggerTimeSpace <0)
                    {
                        m_skillTriggerTimeSpace = m_currentSkillData.SkillInfo.m_TriggerSpaceTime;
                        //加个伤害间隔
                        InHitTime(SyncService.CurrentServiceTime);
                    }

                }
            }

            if (m_skillTime > m_currentSkillData.CurrentTime)
            {
                if (m_enterLatered == false)
                {
                    m_enterLatered = true;
                    m_skillStstus = SkillStatusEnum.Later;
                    EnterSkillLater();
                }
            }

            if (m_skillTime > m_currentSkillData.LaterTime)
            {
                m_skillStstus = SkillStatusEnum.Finish;
                m_character.ChangeStatus(CharacterStatusEnum.Move);

                OnSkillFinish();
            }
        }
    }

    public override void ReceviceCmd(CommandBase cmd)
    {
        base.ReceviceCmd(cmd);

        if (cmd is SkillCmd)
        {
            SkillCmd skillCmd = (SkillCmd)cmd;

            UseSkill( skillCmd);
        }
    }

    public bool GetSkillCD(string skillID)
    {
        m_currentSkillData = m_character.GetSkillData(skillID);

        return m_currentSkillData.CDFinished();
    }

    public void UseSkill(SkillCmd cmd)
    {
        if (m_skillStstus == SkillStatusEnum.Finish ||
            m_skillStstus == SkillStatusEnum.None ||
            m_skillStstus == SkillStatusEnum.Later)
        {
            m_currentSkillData = m_character.GetSkillData(cmd.m_skillID);

            if ( m_currentSkillData.CDFinished())
            {
                CharacterManager.Dispatch(m_character.m_characterID, CharacterEventType.SKill, m_character, m_currentSkillData);

                //使用技能打断Buff
                m_character.BeDamageInterruptBuff();

                //开始计算CD
                m_currentSkillData.BegionCD(cmd.GetCreateTime());

                //技能时间追赶
                m_skillTime = (SyncService.CurrentServiceTime - cmd.GetCreateTime());
                if (m_skillTime < 0)
                {
                    m_skillTime = 0;
                }

                //对齐位置和角度
                //m_character.transform.forward = cmd.m_skillDir;
                m_character.transform.position = cmd.m_pos;

                //派发before事件
                m_skillStstus = SkillStatusEnum.Before;
                EnterSkillBefore();

                //重置技能状态
                m_skillTriggerTimeSpace = 0;
                m_enterCurrented = false;
                m_isTriggerSkill = false;
                m_enterLatered = false;
            }
        }
    }

    public override bool CanMove()
    {
        return true;

        if (m_currentSkillData != null)
        {
            return m_currentSkillData.GetInfo().m_allowMove;
        }
        else
        {
            return true;
        }
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        if (m_skillStstus == SkillStatusEnum.Finish
            || m_skillStstus == SkillStatusEnum.Later
            || m_skillStstus == SkillStatusEnum.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float GetCurrentPursueTime()
    {
        float result = (m_skillTime - m_currentSkillData.BeforeInfo.m_Time);

        if(result <0)
        {
            result = 0;
        }

        //Debug.Log("skill 误差时间 ：" + result);

        return result;
    }

    public override void OnExitStatus()
    {
        m_skillStstus = SkillStatusEnum.None;
        OnSkillNone();

        //退出技能状态销毁技能特效
        m_character.m_effectComp.BreakSkillEffect();
        base.OnExitStatus();
    }

    public override bool CanBreakBySkill(string skillID)
    {
        if (m_currentSkillData != null)
        {
            if (!DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_CanBreak)//受到的攻击也不能打断自己
            {
                return false;
            }

            switch (m_skillStstus)
            {
                case SkillStatusEnum.Before:
                    return DataGenerateManager<SkillDataGenerate>.GetData(m_currentSkillData.m_skillID).m_CanBeBreak;

                case SkillStatusEnum.Current:
                    return DataGenerateManager<SkillDataGenerate>.GetData(m_currentSkillData.m_skillID).m_CanBeBreakInC;
                default: return true;
            }


            //if (!DataGenerateManager<SkillDataGenerate>.GetData(m_currentSkillData.m_skillID).m_CanBeBreak)//自己当前的技能不可以被打断
            //{
            //    return false;
            //}
            //else
            //{
            //    if (!DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_CanBreak)//受到的攻击也不能打断自己
            //    {
            //        return false;
            //    }
            //}
        }

        return true;
    }

    #region 重载方法

    public virtual void EnterSkillBefore()
    {
        DisplayManager.PlaySkillAnim(m_character,m_currentSkillData.m_skillID, SkillStatusEnum.Before);
        DisplayManager.PlayCameraSkillAnim(m_currentSkillData.m_skillID, SkillStatusEnum.Before);
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Before);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Before);
    }

    public virtual void EnterSkillCurrent(float timeOffset)
    {
        string l_shiftID = m_currentSkillData.GetInfo().m_MoveID;
        m_character.m_moveComp.Shift(m_character.m_characterID, m_character.m_characterID, l_shiftID, timeOffset);
        DisplayManager.PlaySkillAnim(m_character, m_currentSkillData.m_skillID, SkillStatusEnum.Current);
        DisplayManager.PlayCameraSkillAnim(m_currentSkillData.m_skillID, SkillStatusEnum.Current);
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Current);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Current);
    }

    public virtual void InHitTime(float hitTime)
    {
        //FightLogicService.UseSkill(m_character, m_currentSkillData.m_skillID,hitTime);
    }

    public virtual void EnterSkillLater()
    {
        DisplayManager.PlaySkillAnim(m_character, m_currentSkillData.m_skillID, SkillStatusEnum.Later,isLater:true);
        DisplayManager.PlayCameraSkillAnim(m_currentSkillData.m_skillID, SkillStatusEnum.Later);
        m_character.m_effectComp.CreatSkillEffect(m_currentSkillData.m_skillID, SkillStatusEnum.Later);
        m_character.m_audio.PlaySkillSFX(m_currentSkillData.m_skillID, SkillStatusEnum.Later);
    }

    public virtual void OnSkillNone()
    {
    }

    public virtual void OnSkillFinish()
    {
    }
    #endregion
}




public enum DirectionEnum
{
    Forward, //施法者前方
    Backward,//施法者后方
    Leave,//受击者远离施法者方向
    Close,//受击者靠近施法者方向
}
