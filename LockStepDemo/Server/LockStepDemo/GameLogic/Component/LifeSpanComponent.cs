using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpanComponent : MomentComponentBase
{
    public int lifeTime = 0;

    public override MomentComponentBase DeepCopy()
    {
        LifeSpanComponent lsc = new LifeSpanComponent();
        lsc.lifeTime = lifeTime;

        return lsc;
    }
}
