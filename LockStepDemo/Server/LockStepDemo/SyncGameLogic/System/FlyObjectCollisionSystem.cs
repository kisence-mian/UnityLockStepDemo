using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//TODO 弃用
public class FlyObjectCollisionSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(FlyObjectComponent),
            typeof(CollisionComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        //List<EntityBase> list = GetEntityList();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    CollisionComponent cc = list[i].GetComp<CollisionComponent>();

        //    for (int j = 0; j < cc.CollisionList.Count; j++)
        //    {
        //        Debug.Log("Collision ID :->" + cc.CollisionList[j].ID);
        //    }
        //}
    }
}
