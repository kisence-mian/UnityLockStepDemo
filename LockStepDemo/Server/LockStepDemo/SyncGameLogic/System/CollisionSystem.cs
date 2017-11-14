using DeJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CollisionSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(CollisionComponent),
        };
    }

    List<CollisionComponent> clist = new List<CollisionComponent>();

    public override void FixedUpdate(int deltaTime)
    {
        //Debug.Log(" ---------------CollisionComponent---------------");
        clist.Clear();

        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent acc = (CollisionComponent)list[i].GetComp("CollisionComponent");
            acc.CollisionList.Clear();

            clist.Add(acc);
        }

        //string content = "";

        for (int i = 0; i < clist.Count; i++)
        {
            CollisionComponent acc = clist[i];

            if(list[i].GetExistComp("MoveComponent"))
            {
                MoveComponent amc = (MoveComponent)list[i].GetComp("MoveComponent");

                acc.area.position = amc.pos.ToVector();
                acc.area.direction = amc.dir.ToVector();
            }

            for (int j = i + 1; j < list.Count; j++)
            {
                CollisionComponent bcc = clist[j];

                //两个静态对象之间不计算阻挡
                if (acc.isStatic && bcc.isStatic)
                {
                    continue;
                }

                if (list[j].GetExistComp("MoveComponent"))
                {
                    MoveComponent bmc = (MoveComponent)list[j].GetComp("MoveComponent");

                    bcc.area.position = bmc.pos.ToVector();
                    bcc.area.direction = bmc.dir.ToVector();
                }

                if (acc.area.AreaCollideSucceed(bcc.area))
                {
                    acc.CollisionList.Add(bcc.Entity);
                    bcc.CollisionList.Add(acc.Entity);
                }

                //分开碰撞的对象
            }
        }
    }

    void DebugLogic(CollisionComponent a , CollisionComponent b)
    {
        if(Filter(a,b))
        {
            Debug.Log("Collision print a "+a.Entity.ID+" -> " + Serializer.Serialize(a.area) 
                + "\n b " + b.Entity.ID + " ->" + Serializer.Serialize(b.area) 
                + "\n AreaCollideSucceed " + a.area.AreaCollideSucceed(b.area) + " frame " + m_world.FrameCount);
        }
    }

    bool Filter(CollisionComponent a, CollisionComponent b)
    {
        //if (!m_world.m_isCertainty)
        //    return false;

        if(a.Entity.GetExistComp<CollisionComponent>() &&
           a.Entity.GetExistComp<CampComponent>() &&
           a.Entity.GetExistComp<FlyObjectComponent>() &&

           b.Entity.GetExistComp<LifeComponent>()
            )
        {
            return true;
        }

        if (b.Entity.GetExistComp<CollisionComponent>() &&
            b.Entity.GetExistComp<CampComponent>() &&
            b.Entity.GetExistComp<FlyObjectComponent>() &&

            a.Entity.GetExistComp<LifeComponent>()
            )
        {
            return true;
        }

        return false;
    }
}
