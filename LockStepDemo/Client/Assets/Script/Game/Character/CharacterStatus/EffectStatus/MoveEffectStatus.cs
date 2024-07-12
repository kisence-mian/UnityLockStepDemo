using UnityEngine;
using System.Collections;

public class MoveEffectStatus : EffectStatusBase
{
    //Vector3 m_direction = Vector3.zero;
    //public float m_gravity = -9;

    //public void Move(MoveCmd cmd)
    //{
    //    cmd.m_dir.y = 0;

    //    if (cmd.m_dir.magnitude > 1)
    //    {
    //        cmd.m_dir = cmd.m_dir.normalized;
    //    }

    //    m_direction = cmd.m_dir;
    //}

    //public override void OnUpdate()
    //{
    //    if (m_direction != Vector3.zero)
    //    {
    //        m_character.m_animComp.PlayAnim(m_character.m_Property.m_walkAnimName);
    //    }
    //    else
    //    {
    //        m_character.m_animComp.PlayAnim(m_character.m_Property.m_idleAnimName);
    //    }

    //    if (m_direction != Vector3.zero)
    //    {
    //        m_character.transform.forward = m_direction;
    //    }

    //    Vector3 result = m_direction;

    //    result *= m_character.m_Property.Speed;
    //    result.y = m_gravity;

    //    m_character.m_moveComp.Move(result);
    //}
}
