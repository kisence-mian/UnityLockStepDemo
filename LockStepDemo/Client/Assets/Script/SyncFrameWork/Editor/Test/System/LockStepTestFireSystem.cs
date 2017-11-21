using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestFireSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[]
        {
            typeof(TestCommandComponent),
            typeof(TestMoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            TestCommandComponent cc = list[i].GetComp<TestCommandComponent>();
            if (cc.isFire)
            {
                //Debug.Log("Fire " + m_world.FrameCount);

                TestLifeSpanComponent lsc = new TestLifeSpanComponent();
                lsc.lifeTime = 500;

                m_world.CreateEntity("FireObject" + cc.Entity.ID,lsc);
            }
        }
    }
}
