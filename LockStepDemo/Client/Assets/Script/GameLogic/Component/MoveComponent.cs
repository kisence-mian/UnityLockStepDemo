using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MomentComponentBase
{
    public int m_posx;
    public int m_posy;
    public int m_posz;

    public int m_dirx;
    public int m_diry;
    public int m_dirz;

    public int m_velocity; //速度

    public override MomentComponentBase DeepCopy()
    {
        MoveComponent mc = new MoveComponent();

        mc.m_posx = m_posx;
        mc.m_posy = m_posy;
        mc.m_posz = m_posz;

        mc.m_dirx = m_dirx;
        mc.m_diry = m_diry;
        mc.m_dirz = m_dirz;

        mc.m_velocity = m_velocity;

        return mc;
    }
}
