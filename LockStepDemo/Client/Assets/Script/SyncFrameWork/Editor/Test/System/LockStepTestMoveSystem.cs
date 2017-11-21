using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestMoveSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(TestMoveComponent),
            typeof(TestCommandComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            UpdateMove(list[i], deltaTime);
        }
    }

    void UpdateMove(EntityBase entity, int deltaTime)
    {
        TestMoveComponent mc = (TestMoveComponent)entity.GetComp("TestMoveComponent");
        TestCommandComponent cc = (TestCommandComponent)entity.GetComp("TestCommandComponent");

        mc.dir = cc.moveDir;
        mc.m_velocity = 4000;

        Debug.Log("LockStepTestMoveSystem " + mc.dir);

        SyncVector3 newPos = mc.pos.DeepCopy();

        newPos.x += (mc.dir.x * deltaTime / 1000) * mc.m_velocity / 1000;
        newPos.y += (mc.dir.y * deltaTime / 1000) * mc.m_velocity / 1000;
        newPos.z += (mc.dir.z * deltaTime / 1000) * mc.m_velocity / 1000;

        Debug.Log("mc.pos "+ mc.pos  + " newPos " + newPos + " mc.dir.x " + mc.dir.x + " mc.m_velocity " + mc.m_velocity);

        mc.pos = newPos;

        //Debug.DrawRay(newPos.ToVector(), Vector3.up, Color.yellow, 10);
    }
}
