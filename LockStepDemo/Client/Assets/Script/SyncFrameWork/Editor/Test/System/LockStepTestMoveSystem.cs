using Lockstep;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestMoveSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(MoveComponent),
            typeof(CommandComponent),
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
        MoveComponent mc = (MoveComponent)entity.GetComp("MoveComponent");
        CommandComponent cc = (CommandComponent)entity.GetComp("CommandComponent");

        mc.dir = cc.moveDir;
        mc.m_velocity = 4000;

        Vector2d newPos = mc.pos;

        newPos += mc.dir * FixedMath.Create(deltaTime).Div(FixedMath.Create(1000)).Mul(FixedMath.Create(mc.m_velocity).Div(FixedMath.Create(1000)));

        mc.pos = newPos;

        //Debug.Log("dir " + mc.dir + " ");
    }
}
