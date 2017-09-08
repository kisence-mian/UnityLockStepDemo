using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CreateItemSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(ItemCreatePointComponent),
            typeof(CollisionComponent),
        };
    }

    public override void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ItemCreatePointComponent icpc = list[i].GetComp<ItemCreatePointComponent>();
            CollisionComponent cc         = list[i].GetComp<CollisionComponent>();

            icpc.CreateTimer -= deltaTime;

            if (icpc.CreateTimer <= 0)
            {
                if(CanCreate(cc))
                {
                    CreateRandomItem(icpc);
                }
               

                icpc.CreateTimer = 10 * 1000;
            }
        }
    }

    public void CreateRandomItem(ItemCreatePointComponent comp)
    {
        //创建一个可以捡的道具
        CollisionComponent colc = new CollisionComponent();
        colc.area.position = comp.pos.ToVector();
        colc.area.areaType = AreaType.Circle;
        colc.area.radius = 0.5f;

        ItemComponent ic = new ItemComponent();
        AssetComponent assert = new AssetComponent();
        MoveComponent mc = new MoveComponent();
        //SyncComponent sc = new SyncComponent(); 
        
        mc.pos = comp.pos;

        Random random = new Random();
        int r =  random.Next() % comp.randomList.Count;
        assert.m_assetName = comp.randomList[r];

        m_world.CreateEntity(colc, ic, assert, mc);
    }

    bool CanCreate(CollisionComponent comp)
    {
        for (int i = 0; i < comp.CollisionList.Count; i++)
        {
            if(comp.CollisionList[i].GetExistComp<ItemComponent>())
            {
                return false;
            }
        }

        return true;
    }
}
