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

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent com = list[i].GetComp<CommandComponent>();
            MoveComponent move = list[i].GetComp<MoveComponent>();

            if (com.isForward)
            {
                move.m_velocity = 1;
            }

            if (com.isBack)
            {
                move.m_velocity = 0;

                if (move.m_velocity < 0)
                {
                    move.m_velocity = 0;
                }
            }

            if (com.isLeft)
            {
                move.m_dirx = -1;
            }

            if (com.isRight)
            {
                move.m_dirx = 1;
            }

            //Debug.Log("com.isBack " + com.isBack + " com.isForward " + com.isForward + " com.isRight " + com.isRight + "comp.isLeft " + com.isLeft);
        }
    }
}
