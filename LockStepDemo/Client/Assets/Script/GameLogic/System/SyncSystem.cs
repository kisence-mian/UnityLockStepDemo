using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSystem : SystemBase
{
    public override void Init()
    {
        GlobalEvent.AddTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
    }

    public override void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
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

    #region 消息接收

    void ReceviceSyncEntity(SyncEntityMsg msg, params object[] objs)
    {
        EntityBase entity;
        if(!m_world.GetEntityIsExist(msg.m_id))
        {
            entity = m_world.CreateEntity(msg.m_id);
        }
        else
        {
            entity = m_world.GetEntity(msg.m_id);
        }

        for (int i = 0; i < msg.infos.Count; i++)
        {
            ComponentBase comp = (ComponentBase)JsonUtility.FromJson(msg.infos[i].content, Type.GetType(msg.infos[i].m_compName));

            if (entity.GetExistComp(msg.infos[i].m_compName))
            {
                entity.ChangeComp(msg.infos[i].m_compName, comp);
            }
            else
            {
                entity.AddComp(msg.infos[i].m_compName, comp);
            }
        }

        Debug.Log("ReceviceSyncEntity");
    }
    #endregion
}

//    void ReceviceCompChange(ChangeComponentMsg msg, params object[] objs)
//    {
//        EntityBase entity = m_world.GetEntity(msg.m_id);

//        if(msg.m_operation != ChangeStatus.Remove)
//        {
//            ComponentBase comp = (ComponentBase)JsonUtility.FromJson(msg.info.content, Type.GetType(msg.info.m_compName));

//            if(msg.m_operation == ChangeStatus.Add)
//            {
//                entity.AddComp(msg.info.m_compName, comp);
//            }
//            else
//            {
//                entity.ChangeComp(msg.info.m_compName, comp);
//            }
//        }
//        else
//        {
//            entity.RemoveComp(msg.info.m_compName);
//        }
//    }

//}
