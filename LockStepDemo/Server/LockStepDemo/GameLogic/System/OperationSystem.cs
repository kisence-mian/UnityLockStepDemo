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
        };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent com = list[i].GetComp<CommandComponent>();
            MoveComponent move = list[i].GetComp<MoveComponent>();

            move.dir = com.moveDir;

            if (com.moveDir.ToVector() != Vector3.zero)
            {
                move.m_velocity = 5;
            }
            else
            {
                move.m_velocity = 0;
            }
        }
    }
}
