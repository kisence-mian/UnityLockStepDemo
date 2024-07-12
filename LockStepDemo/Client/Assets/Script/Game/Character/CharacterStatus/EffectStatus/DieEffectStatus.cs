using UnityEngine;
using System.Collections;

public class DieEffectStatus : EffectStatusBase
{
    public override void OnUpdate()
    {
        m_character.m_animComp.PlayAnim(m_character.m_Property.m_dieAnimName);
    }

	public void Die(DieCmd cmd)
    {

    }
}
