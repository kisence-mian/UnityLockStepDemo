using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(MoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            UpdateMove(list[i],deltaTime);
        }
    }

    void UpdateMove(EntityBase entity,int deltaTime)
    {
        MoveComponent mc = entity.GetComp<MoveComponent>();

        SyncVector3 newPos = mc.pos.DeepCopy();

        newPos.x += (mc.dir.x * deltaTime /1000) * mc.m_velocity / 1000;
        newPos.y += (mc.dir.y * deltaTime / 1000) * mc.m_velocity / 1000;
        newPos.z += (mc.dir.z * deltaTime / 1000) * mc.m_velocity / 1000;

        if (!entity.GetExistComp<FlyObjectComponent>() 
            && entity.GetExistComp<CollisionComponent>())
        {
            CollisionComponent cc = entity.GetComp<CollisionComponent>();
            cc.area.position = newPos.ToVector();

            if (!IsCollisionBlock(cc.area))
            {
                mc.pos = newPos;
            }
        }
        else
        {
            mc.pos = newPos;
        }

        if (SyncDebugSystem.isDebug && SyncDebugSystem.IsFilter("MoveSystem"))
        {
            string content = "id: " + mc.Entity.ID + " m_pos " + mc.pos.ToVector() + " deltaTime " + deltaTime + " m_velocity " + mc.m_velocity + " m_dir " + mc.dir.ToVector();
            //Debug.Log(content);

            SyncDebugSystem.syncLog += content + "\n";
        }
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

    public bool IsCollisionBlock(Area area)
    {
        List<EntityBase> list = GetEntityList(new string[] { "CollisionComponent", "BlockComponent" });
        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent cc = list[i].GetComp<CollisionComponent>();
            if (cc.area.AreaCollideSucceed(area))
            {
                return true;
            }
        }

        return false;
    }
}
