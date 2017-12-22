using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestLifeSpanSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(TestLifeSpanComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            TestLifeSpanComponent lsc = list[i].GetComp<TestLifeSpanComponent>(ComponentType.TeamAchievementDataComponent);
            lsc.lifeTime -= deltaTime;

            //Debug.Log("lsc.lifeTime  " + lsc.lifeTime + " frame " + m_world.FrameCount + " ID " + lsc.Entity.ID);

            if (lsc.lifeTime <= 0)
            {
                m_world.ClientDestroyEntity(list[i].ID);
            }
        }
    }
}