using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : SystemBase
{
    public override void FixedUpdate(int deltaTime)
    {
        List<MoveTuple> list = GetMoveTuple();

        for (int i = 0; i < list.Count; i++)
        {
            UpdateMove(list[i].m_moveComp,deltaTime);
        }
    }

    void UpdateMove(MoveComponent comp,int deltaTime)
    {
        comp.pos.x += comp.dir.x * deltaTime * comp.m_velocity / 1000;
        comp.pos.y += comp.dir.y * deltaTime * comp.m_velocity / 1000;
        comp.pos.z += comp.dir.z * deltaTime * comp.m_velocity / 1000;

        Debug.Log("id: " + comp.Entity.ID + " m_pos " + comp.pos.ToVector() + " deltaTime " + deltaTime + " m_velocity " + comp.m_velocity + " m_dir " + comp.dir.ToVector());
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
