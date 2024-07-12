using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MomentComponentBase
{
    public int maxLife;
    public int life;

    public override MomentComponentBase DeepCopy()
    {
        LifeComponent lc = new LifeComponent();
        lc.maxLife = maxLife;
        lc.life = life;

        return lc;
    }
}
