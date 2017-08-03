using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : SystemBase
{
    public override void LateUpdate(int deltaTime)
    {
        List<MoveTuple> list = GetMoveTuple();

        for (int i = 0; i < list.Count; i++)
        {
            UpdateMove(list[i].m_moveComp,deltaTime);
        }
    }

    void UpdateMove(MoveComponent comp,int deltaTime)
    {
        comp.m_posx = comp.m_dirx * deltaTime * comp.m_velocity;
        comp.m_posy = comp.m_diry * deltaTime * comp.m_velocity;
        comp.m_posz = comp.m_dirz * deltaTime * comp.m_velocity;
    }


    List<MoveTuple> m_moveTupleList = new List<MoveTuple>();
    List<MoveTuple> GetMoveTuple()
    {
        m_moveTupleList.Clear();

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            if(m_world.m_entityList[i].GetExistComp("MoveComponent"))
            {
                MoveTuple tuple = new MoveTuple();
                tuple.m_moveComp = (MoveComponent)m_world.m_entityList[i].GetComp("MoveComponent");

                m_moveTupleList.Add(tuple);
            }
        }

        return m_moveTupleList;
    }

    struct MoveTuple
    {
        public MoveComponent m_moveComp;
    }
}
