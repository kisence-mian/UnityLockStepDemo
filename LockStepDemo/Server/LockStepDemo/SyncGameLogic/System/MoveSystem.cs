using DeJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(MoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            UpdateMove(list[i],deltaTime);
        }
    }

    void UpdateMove(EntityBase entity,int deltaTime)
    {
        MoveComponent mc = (MoveComponent)entity.GetComp("MoveComponent");

        SyncVector3 newPos = mc.pos.DeepCopy();

        newPos.x += (mc.dir.x * deltaTime /1000) * mc.m_velocity / 1000;
        newPos.y += (mc.dir.y * deltaTime / 1000) * mc.m_velocity / 1000;
        newPos.z += (mc.dir.z * deltaTime / 1000) * mc.m_velocity / 1000;

        mc.pos = newPos;
    }
}
