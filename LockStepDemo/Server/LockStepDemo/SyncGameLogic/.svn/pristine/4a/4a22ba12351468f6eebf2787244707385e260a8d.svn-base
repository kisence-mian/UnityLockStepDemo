using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurgenceSystem :SystemBase
{

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(LifeComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            LifeComponent lc = list[i].GetComp<LifeComponent>();

            if(lc.Life < 0)
            {
                lc.ResurgenceTimer += deltaTime;

                if(lc.ResurgenceTimer > 10 * 1000)
                {
                    lc.Life = lc.maxLife;
                    m_world.eventSystem.DispatchEvent(GameUtils.GetEventKey(list[i].ID, CharacterEventType.Recover), list[i]);
                }
            }
        }
    }
}
