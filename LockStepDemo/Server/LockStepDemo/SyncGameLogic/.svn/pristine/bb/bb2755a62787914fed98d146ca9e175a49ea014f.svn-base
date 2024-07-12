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

            BlockComponent abc = null;

            if(list[i].GetExistComp("BlockComponent"))
            {
               abc = (BlockComponent)list[i].GetComp("BlockComponent");
            }

            if(list[i].GetExistComp("MoveComponent"))
            {
                MoveComponent amc = (MoveComponent)list[i].GetComp("MoveComponent");

                acc.area.position = amc.pos.ToVector();
                acc.area.direction = amc.dir.ToVector();
            }

            for (int j = i + 1; j < list.Count; j++)
            {
                CollisionComponent bcc = clist[j];

                //两个阻挡组件之间不计算阻挡
                if (abc != null && list[j].GetExistComp("BlockComponent"))
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
            }
        }
    }



}
