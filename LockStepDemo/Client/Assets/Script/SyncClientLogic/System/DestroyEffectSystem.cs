using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DestroyEffectSystem : SystemBase
{
    public override void Init()
    {
        AddEntityOptimizeWillBeDestroyLisnter();
    }

    public override void Dispose()
    {
        RemoveEntityOptimizeWillBeDestroyLisnter();
    }

    public override void OnEntityOptimizeWillBeDestroy(EntityBase entity)
    {
        //if(entity.GetExistComp<FlyObjectComponent>()
        //    && entity.GetExistComp<PerfabComponent>())
        //{
        //    OnFlyDestroy(entity);
        //}
    }

    void OnFlyDestroy(EntityBase entity)
    {
        FlyObjectComponent fc = entity.GetComp<FlyObjectComponent>();
        PerfabComponent pc = entity.GetComp<PerfabComponent>();

        EffectManager.ShowEffect(fc.FlyData.m_HitEffect, pc.perfab.transform.position + pc.perfab.transform.forward * 0.3f, 1);
    }
}
