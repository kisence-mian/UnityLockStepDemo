using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectComponent : MomentComponentBase
{
    public List<BuffEffectPackage> effectList = new List<BuffEffectPackage>();

    public override MomentComponentBase DeepCopy()
    {
        BuffEffectComponent be = new BuffEffectComponent();

        for (int i = 0; i < effectList.Count; i++)
        {
            be.effectList.Add(effectList[i].DeepCopy());
        }

        return be;
    }

    public BuffEffectPackage GetBuffEffectPackage(string buffID)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if(effectList[i].buffID == buffID)
            {
                return effectList[i];
            }
        }

        return null;
    }
}

public class BuffEffectPackage
{
    public string buffID;

    public int buffEffectID;     //buff持续特效

    public BuffEffectPackage DeepCopy()
    {
        BuffEffectPackage bep = new BuffEffectPackage();
        bep.buffID = buffID;
        bep.buffEffectID = buffEffectID;

        return bep;
    }
}
