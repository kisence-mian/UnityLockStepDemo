using UnityEngine;
using System.Collections;

public class MonsterHurtStatus : HurtStatus
{

    public override void OnEnterStatus()
    {
        base.OnEnterStatus();
        if (m_character.m_Property.m_isOnlyDamageValue)
        {
            return;
        }
        PlayHurtAnim();
        
    }


}
