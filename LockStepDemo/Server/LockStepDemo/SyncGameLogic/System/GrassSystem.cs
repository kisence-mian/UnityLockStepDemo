using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] 
        {
            typeof(PlayerComponent),
            typeof(CollisionComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            PlayerComponent pc = (PlayerComponent)list[i].GetComp("PlayerComponent");
            CollisionComponent cc = (CollisionComponent)list[i].GetComp("CollisionComponent");

            pc.isCloak = false;
            pc.grassID = -1;

            for (int j = 0; j < cc.CollisionList.Count; j++)
            {
                EntityBase entityTmp = cc.CollisionList[j];
                if (entityTmp.GetExistComp("GrassComponent"))
                {
                    GrassComponent gc = (GrassComponent)entityTmp.GetComp("GrassComponent");

                    pc.isCloak = true;
                    pc.grassID = gc.Entity.ID;
                }
            }
        }
    }
}
