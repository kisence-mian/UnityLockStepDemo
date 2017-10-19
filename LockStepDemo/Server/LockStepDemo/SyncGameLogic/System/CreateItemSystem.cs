using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreateItemSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(ItemCreatePointComponent),
            typeof(CollisionComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ItemCreatePointComponent icpc = list[i].GetComp<ItemCreatePointComponent>();
            CollisionComponent cc         = list[i].GetComp<CollisionComponent>();

            icpc.CreateTimer -= deltaTime;

            //Debug.Log("CreateTimer " + icpc.CreateTimer + " frame " + m_world.FrameCount);

            if (CanCreate(cc))
            {
                if (icpc.CreateTimer <= 0)
                {
                    CreateRandomItem(icpc);
                }
            }
            else
            {
                icpc.CreateTimer = 10 * 1000;
            }
        }
    }

    public void CreateRandomItem(ItemCreatePointComponent comp)
    {
        //创建一个可以捡的道具
        CollisionComponent colc = new CollisionComponent();
        colc.area.direction = new Vector3(1, 0, 0);
        colc.area.position = comp.pos.ToVector();
        colc.area.areaType = AreaType.Circle;
        colc.area.radius = 0.5f;

        ItemComponent ic = new ItemComponent();
        AssetComponent assert = new AssetComponent();
        TransfromComponent tc = new TransfromComponent();

        tc.pos = comp.pos;

        int r =  m_world.GetRandom() % comp.randomList.Count;

        //Debug.Log("r " + r + " comp.randomList.Count " + comp.randomList.Count);

        assert.m_assetName = comp.randomList[r];

        string identify = comp.Entity.ID + "Item" + comp.pos.ToVector(); //通过标识符保证唯一ID
        m_world.CreateEntity(identify, colc, ic, assert, tc);
    }

    bool CanCreate(CollisionComponent comp)
    {
        for (int i = 0; i < comp.CollisionList.Count; i++)
        {
            if (comp.CollisionList[i].GetExistComp<ItemComponent>())
            {
                return false;
            }
        }

        return true;
    }
}
