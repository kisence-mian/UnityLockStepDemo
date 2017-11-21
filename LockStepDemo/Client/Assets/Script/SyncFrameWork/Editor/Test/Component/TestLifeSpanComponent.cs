using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLifeSpanComponent : MomentComponentBase
{
    public int lifeTime = 0;

    public override MomentComponentBase DeepCopy()
    {
        TestLifeSpanComponent lsc = new TestLifeSpanComponent();
        lsc.lifeTime = lifeTime;

        return lsc;
    }
}