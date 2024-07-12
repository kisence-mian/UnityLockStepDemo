using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatus : CharacterBaseStatus
{
    public override void OnUpdate()
    {
        string animName = GetBuffAnim();

        if (animName != null)
        {
            m_character.m_animComp.PlayAnim(animName);
        }
    }

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Buff;
    }

    public string GetBuffAnim()
    {
        for (int i = 0; i < m_character.m_buffList.Count; i++)
        {
            if (m_character.m_buffList[i].m_buffData.m_IsTakeOver && m_character.m_buffList[i].m_buffData.m_AnimName != "null")
            {
                return m_character.m_buffList[i].m_buffData.m_AnimName;
            }
        }

        return null;
    }


    public override bool CanBreakBySkill(string skillID)
    {
        return false;
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        return false;
    }
}
