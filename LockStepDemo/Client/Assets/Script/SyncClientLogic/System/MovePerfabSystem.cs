using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePerfabSystem : ViewSystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(PerfabComponent),
            typeof(MoveComponent)
        };
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            MoveComponent move = list[i].GetComp<MoveComponent>();
            GameObject perfab  = list[i].GetComp<PerfabComponent>().perfab;

            if(perfab != null)
            {
                float distance = Vector3.Distance(perfab.transform.position, move.pos.ToVector());
                float moveSpeed = (float)move.m_velocity / 1000;

                float moveOffset = moveSpeed * ((float)deltaTime / 1100);

                if (moveOffset > distance)
                {
                    moveOffset = distance;
                }

                Vector3 dir = (move.pos.ToVector() - perfab.transform.position).normalized;
                Vector3 pos = perfab.transform.position;

                pos += dir * moveOffset;

                perfab.transform.position = pos;

                //if (move.dir.ToVector() != Vector3.zero)
                //{
                //    if (list[i].GetExistComp<SelfComponent>())
                //    {
                //        if (InputSystem.moveDirCache != Vector3.zero)
                //        {
                //            perfab.transform.forward = InputSystem.moveDirCache;
                //        }
                //    }
                //    else
                //    {
                //        perfab.transform.forward = move.dir.ToVector();
                //    }

                //}
            }
        }
    }
}
