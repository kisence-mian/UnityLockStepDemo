using UnityEngine;
using System.Collections;

public class HurtStatus : CharacterBaseStatus
{
    float m_hurtTime = 0f;
    bool isCanMove = false;




    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Hurt;

    }
    public override void OnUpdate()
    {
        m_hurtTime -= Time.deltaTime;

        if (m_hurtTime < 0)
        {
            m_character.ChangeStatus(CharacterStatusEnum.Move);
            isCanMove = true;
        }
        
    }

    public override void ReceviceCmd(CommandBase cmd)
    {
        base.ReceviceCmd(cmd);

       
        //OnHurt();
    }

    public void OnHurt()
    {
        m_hurtTime = m_character.m_Property.m_hurtAnimTime;
    }

    /// <summary>
    /// 播放受击动画
    /// </summary>
    protected void PlayHurtAnim()
    {
        if (m_character.m_Property.m_hurtAnimName != "None" )
        {
            if (m_character.m_camp == Camp.Brave)
            {
                m_character.MaterialHighLight();
            }
            else
            {
                m_character.m_animComp.PlayAnim(m_character.m_Property.m_hurtAnimName);
                m_character.MaterialHighLight();
            }
            
        }  
        else
            m_character.m_animComp.StopAnim();
    }



    public override void OnEnterStatus()
    {
        
        isCanMove = false;
        base.OnEnterStatus();
        OnHurt();
    }

    public override void OnExitStatus()
    {
        isCanMove = true;
        base.OnExitStatus();
        //m_character.m_animComp.RunAnim();
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        return isCanMove ;
    }
}
