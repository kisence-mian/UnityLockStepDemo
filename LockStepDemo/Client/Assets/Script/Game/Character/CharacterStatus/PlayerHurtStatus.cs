using UnityEngine;
using System.Collections;

public class PlayerHurtStatus : HurtStatus
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
