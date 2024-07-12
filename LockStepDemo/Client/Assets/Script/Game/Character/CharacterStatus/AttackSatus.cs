using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSatus : CharacterBaseStatus
{
    protected int m_attackAnimIndex = 0;
    public SkillData m_currentAttackData;
    protected SkillStatusEnum m_attackStatus = SkillStatusEnum.Finish;

    public float m_attackTime = 0;
    public float m_skillTriggerTimeSpace = 0;

    bool m_enterCurrented = false; //已经进入过current
    bool m_calcDamaged    = false; //已经计算过伤害
    bool m_enterLatered   = false ;//已经进入过later

    float m_exitTime = 0; //退出时间
    protected bool m_isRaise = true; //播放起手动画
    AttackCmd m_comboCmd = null;

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Attack;
    }

    public override void OnUpdate()
    {
        if (m_attackStatus != SkillStatusEnum.Finish 
            && m_attackStatus != SkillStatusEnum.None)
        {
            m_attackTime += Time.deltaTime;

            if (m_attackTime > m_currentAttackData.BeforeTime)
            {
                if (m_enterCurrented == false)
                {
                    m_enterCurrented = true;
                    m_attackStatus = SkillStatusEnum.Current;
                    EnterAttackCurrent(GetCurrentPursueTime());
                }
            }

            float hitTime = m_currentAttackData.HitTime;


            if (m_attackTime > hitTime  - SyncService.SyncAheadTime)
            {
                if (m_currentAttackData.SkillInfo.m_Moment)
                {
                    if (m_calcDamaged == false)
                    {
                        m_calcDamaged = true;
                        //InHitTime(SyncService.CurrentServiceTime);
                    }
                }
                else
                {
                    m_skillTriggerTimeSpace -= Time.deltaTime;
                    if (m_skillTriggerTimeSpace < 0)
                    {
                        m_skillTriggerTimeSpace = m_currentAttackData.SkillInfo.m_TriggerSpaceTime;
                        //加个伤害间隔
                        //InHitTime(SyncService.CurrentServiceTime);
                    }
                }
            }

            if (m_attackTime > m_currentAttackData.CurrentTime )
            {
                if (m_enterLatered == false)
                {
                    m_enterLatered = true;
                    m_attackStatus = SkillStatusEnum.Later;
                    AttackLaterLogic();
                }
            }

            if (m_attackTime > m_currentAttackData.LaterTime)
            {
                m_attackStatus = SkillStatusEnum.Finish;
                AttackFinishLogic();
            }
        }
        else
        {
            m_character.ChangeStatus(CharacterStatusEnum.Move);
        }

        switch (m_attackStatus)
        {
            case SkillStatusEnum.Before:
                OnAttackBefore();
                break;
            case SkillStatusEnum.Current:
                OnAttackCurrent();
                break;
            case SkillStatusEnum.Later:
                OnAttackLater();
                break;
        }


	}

    public override void ReceviceCmd(CommandBase cmd)
    {
        if (cmd is AttackCmd)
        {
            Attack((AttackCmd)cmd);
        }
    }

    float GetCurrentPursueTime()
    {
        float result = (m_attackTime - m_currentAttackData.BeforeInfo.m_Time);

        if (result < 0)
        {
            result = 0;
        }

        //Debug.Log("attack 误差时间 ：" + result);

        return result;
    }

    public void Attack(AttackCmd cmd)
    {
        if (m_attackStatus == SkillStatusEnum.Finish ||
            m_attackStatus == SkillStatusEnum.None   ||
            m_attackStatus == SkillStatusEnum.Later
            )
        {
            m_currentAttackData = m_character.GetAttackData(m_attackAnimIndex);

            if (m_currentAttackData.CDFinished())
            {
                //如果在后摇阶段进入下段攻击切掉起手动画
                m_isRaise = !(m_attackStatus == SkillStatusEnum.Later);

                CharacterManager.Dispatch(m_character.m_characterID, CharacterEventType.Attack, m_character, m_currentAttackData);

                //开始计算CD
                m_currentAttackData.BegionCD(cmd.GetCreateTime());

                //使用技能打断Buff
                m_character.BeDamageInterruptBuff();

                //对齐位置和角度
                m_character.transform.forward = cmd.m_dir;
                m_character.transform.position = cmd.m_pos;

                //派发事件
                m_attackStatus = SkillStatusEnum.Before;
                EnterAttackBefore();

                //追赶时间
                m_attackTime = (SyncService.CurrentServiceTime - cmd.GetCreateTime());
                if (m_attackTime < 0)
                {
                    m_attackTime = 0;
                }

                if (!m_isRaise)
                {
                    m_attackTime += m_currentAttackData.SkillInfo.m_RaiseTime;
                }

                //Debug.Log("m_attackTime " + m_attackTime);

                //技能重置
                m_skillTriggerTimeSpace = 0;
                m_enterCurrented = false;
                m_calcDamaged = false;
                m_enterLatered = false;
            }
        }
        else
        {
            m_comboCmd = cmd;
        }
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        //m_isCombo = false;

        if (m_attackStatus == SkillStatusEnum.Finish
            || m_attackStatus == SkillStatusEnum.Later
            || otherStatus == CharacterStatusEnum.Skill
            || otherStatus == CharacterStatusEnum.Blowfly
            //|| m_attackStatus == AttackStatusEnum.Later
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CanBreakBySkill(string skillID)
    {
        if (m_currentAttackData != null)
        {
            if (!DataGenerateManager<SkillDataGenerate>.GetData(m_currentAttackData.m_skillID).m_CanBeBreak)//自己当前的技能不可以被打断
            {
                return false;
            }
            else
            {
                if (!DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_CanBreak)//受到的攻击也不能打断自己
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override void OnExitStatus()
    {
        m_attackStatus = SkillStatusEnum.None;
        EnterAttackNone();

        m_exitTime = SyncService.CurrentServiceTime;

        m_comboCmd = null;

        //退出技能状态销毁技能特效
        m_character.m_effectComp.BreakSkillEffect();

        base.OnExitStatus();
    }

    public override void OnEnterStatus()
    {
        base.OnEnterStatus();

        if (SyncService.CurrentServiceTime - m_exitTime > 0.5f)
        {
            m_attackAnimIndex = 0;
        }
    }

    public void AttackLaterLogic()
    {
        EnterAttackLater();

        AttackIndexLogic();

        if (m_comboCmd != null)
        {
            m_comboCmd.SetCreateTime(SyncService.CurrentServiceTime);

            m_comboCmd.m_pos = m_character.transform.position;
            m_comboCmd.m_dir = m_character.transform.forward;

            Attack(m_comboCmd);

            m_comboCmd = null;
        }
    }

    protected void AttackIndexLogic()
    {
        //m_attackAnimIndex++;
        int newAttackIndex = Random.Range(0, m_character.m_attackInfoList.Count);
        if (m_attackAnimIndex == newAttackIndex)
        {
            if (m_attackAnimIndex > 0)
            {
                m_attackAnimIndex -- ;
            }
            else
            {
                m_attackAnimIndex++;
            }
            
        }
        else
        {
            m_attackAnimIndex = newAttackIndex;
        }
        if (m_attackAnimIndex >= m_character.m_attackInfoList.Count)
        {
            m_attackAnimIndex = 0;
        }
        //Debug.Log(m_attackAnimIndex);
    }

    public void AttackFinishLogic()
    {
        m_character.ChangeStatus(CharacterStatusEnum.Move);

        EnterAttackFinsih();
    }

    #region 重载方法

    public virtual void EnterAttackBefore()
    {

    }
    public virtual void OnAttackBefore()
    {
        
    }

    public virtual void EnterAttackCurrent(float timeOffset)
    {

    }

    public virtual void OnAttackCurrent()
    {
      
    }

    public virtual void InHitTime(float hitTime)//命中时间
    {
 
    }

    public virtual void EnterAttackLater()
    {

    }
    public virtual void EnterAttackFinsih()
    {
    }

    public virtual void OnAttackLater()
    {
       
    }

    public virtual void EnterAttackNone()
    {
    }

    public virtual void OnAttackNone()
    {
    }




    #endregion
}