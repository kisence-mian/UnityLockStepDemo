using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CollisionDamageSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(CollisionComponent),
            typeof(Camp),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            DamageLogic(list[i]);
        }
    }

    void DamageLogic(EntityBase entity)
    {
        CollisionComponent cc = entity.GetComp<CollisionComponent>();

        if (cc.CollisionList.Count > 0)
        {
            Debug.Log("collision " + cc.CollisionList[0].ID);
        }
    }
}

