using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class ItemSystem : SystemBase
    {
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof( ItemComponent),
            typeof( CollisionComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ItemComponent ic      = list[i].GetComp<ItemComponent>();
            CollisionComponent cc = list[i].GetComp<CollisionComponent>();

            for (int j = 0; j < cc.CollisionList.Count; j++)
            {
                if(cc.CollisionList[j].GetExistComp<PlayerComponent>())
                {
                    PlayerComponent pc = cc.CollisionList[j].GetComp<PlayerComponent>();

                    pc.AddElement(ic.ItemID);
                    m_world.ClientDestroyEntity(list[i].ID);
                    break;
                }
            }
        }
    }
}
