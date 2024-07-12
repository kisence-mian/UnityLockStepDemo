using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[]
        {
            typeof(CommandComponent),
            typeof(MoveComponent),
            typeof(PlayerComponent),
            typeof(LifeComponent),
            typeof(BlowFlyComponent),
        };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent com = list[i].GetComp<CommandComponent>();
            MoveComponent move = list[i].GetComp<MoveComponent>();
            PlayerComponent pc = list[i].GetComp<PlayerComponent>();
            LifeComponent lc = list[i].GetComp<LifeComponent>();
            BlowFlyComponent blc = list[i].GetComp<BlowFlyComponent>();

            if(lc.Life > 0 
                && !blc.isBlow)
            {
                pc.faceDir = com.skillDir.DeepCopy();
                move.dir = com.moveDir.DeepCopy();

                if (com.moveDir.ToVector() != Vector3.zero)
                {
                    move.m_velocity = 5 * 1000;
                }
                else
                {
                    move.m_velocity = 5 * 1000;
                }
            }

            if(lc.Life < 0)
            {
                move.dir.FromVector(Vector3.zero);
            }
        }
    }
}
