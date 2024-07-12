using UnityEngine;
using System.Collections;

public class EffectStatusBase 
{
    protected CharacterBase m_character;
    private bool isCurrentStatus = false;

    public bool IsCurrentStatus
    {
        get { return isCurrentStatus; }
        set { isCurrentStatus = value; }
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
}
