using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSystem : SystemBase
{
    public override void Init()
    {

    }

    public override void LateFixedUpdate(int deltaTime)
    {
        SyncComponent.currentFixedFrame++;
        SyncComponent.currentTime = deltaTime / 1000f;
    }

    List<SyncTuple> m_TupleList = new List<SyncTuple>();
    string[] compList = { "MoveComponent" };

    List<SyncTuple> GetTupleList()
    {
        m_TupleList.Clear();

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            if(GetAllExistComp(compList, m_world.m_entityList[i]))
            {
                SyncTuple tuple = new SyncTuple();
                tuple.m_moveComp =  m_world.m_entityList[i].GetComp<MoveComponent>();

                m_TupleList.Add(tuple);
            }
        }

        return m_TupleList;
    }

    struct SyncTuple
    {
        public MoveComponent m_moveComp;
    }
}
