using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CollisionSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(CollisionComponent),
            typeof(MoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent acc = list[i].GetComp<CollisionComponent>();
            acc.CollisionList.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent acc = list[i].GetComp<CollisionComponent>();
            MoveComponent amc = list[i].GetComp<MoveComponent>();

            acc.area.position = amc.pos.ToVector();
            acc.area.direction = amc.dir.ToVector();

            for (int j = i + 1; j < list.Count; j++)
            {
                CollisionComponent bcc = list[j].GetComp<CollisionComponent>();
                MoveComponent bmc = list[j].GetComp<MoveComponent>();

                bcc.area.position = bmc.pos.ToVector();
                bcc.area.direction = bmc.dir.ToVector();

                if (acc.area.AreaCollideSucceed(bcc.area))
                {
                    acc.CollisionList.Add(bcc.Entity);
                    bcc.CollisionList.Add(acc.Entity);
                }
            }
        }
    }

}
