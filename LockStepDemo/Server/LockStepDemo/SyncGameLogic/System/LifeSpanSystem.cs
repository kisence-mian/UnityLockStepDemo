using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LifeSpanSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(LifeSpanComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            LifeSpanComponent lsc = list[i].GetComp<LifeSpanComponent>();
            lsc.lifeTime -= deltaTime;

            //Debug.Log("lsc.lifeTime  " + lsc.lifeTime + " frame " + m_world.FrameCount + " ID " + lsc.Entity.ID);

            if (lsc.lifeTime < 0)
            {
                m_world.ClientDestroyEntity(list[i].ID);
            }
        }
    }
}
