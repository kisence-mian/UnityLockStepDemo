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
            typeof(CommandComponent),
            typeof(MoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent cc = list[i].GetComp<CommandComponent>();
            if (cc.isFire)
            {
                Debug.Log("Fire " + m_world.FrameCount);

                TestSingleComponent tc = m_world.GetSingletonComp<TestSingleComponent>();
                tc.testValue++;

                LifeSpanComponent lsc = new LifeSpanComponent();
                lsc.lifeTime = 500;

                m_world.CreateEntity("FireObject" + cc.Entity.ID,lsc);
            }
        }
    }
}
