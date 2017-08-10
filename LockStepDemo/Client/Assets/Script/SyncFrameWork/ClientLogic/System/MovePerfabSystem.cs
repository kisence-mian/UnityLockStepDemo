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

            Vector3 pos = new Vector3(move.m_posx * 0.001f, move.m_posy * 0.001f, move.m_posz * 0.001f);

            perfab.transform.position = Vector3.Lerp(perfab.transform.position,pos,Time.deltaTime /10);

            //Debug.Log("MovePerfabSystem Update ->" + pos);
        }
    }
}
