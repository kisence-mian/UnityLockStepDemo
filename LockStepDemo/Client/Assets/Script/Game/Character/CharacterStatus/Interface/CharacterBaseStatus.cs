using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public abstract class CharacterBaseStatus 
{
    protected CharacterBase m_character;
    private bool m_isCurrentStatus = false;

    protected CharacterStatusEnum m_Status;

    public CharacterStatusEnum StatusEnum
    {
        get { return m_Status; }
    }
    public CharacterStatusEnum m_lastCharacterStatus;
    public bool IsCurrentStatus
    {
        get { return m_isCurrentStatus; }
        set { m_isCurrentStatus = value; }
    }

    public virtual void Init(CharacterBase character)
    {
        m_character = character;
    }

    public virtual void OnEnterStatus()
    {
    }

    public virtual void OnExitStatus()
    {
    }

    public virtual void OnUpdate()
    {

    }

    public virtual bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        return true;
    }

    public virtual bool CanBreakBySkill(string skillID)
    {
        return false;
    }

    public virtual bool CanMove()
    {
        return false;
    }

    public virtual void ReceviceCmd(CommandBase cmd)
    {

    }
}
