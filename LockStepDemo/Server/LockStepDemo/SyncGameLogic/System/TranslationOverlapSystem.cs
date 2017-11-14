using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TranslationOverlapSystem : SystemBase
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
            OverlapLogic(list[i]);
        }
    }

    public void OverlapLogic(EntityBase entity)
    {
        CollisionComponent cc = (CollisionComponent)entity.GetComp("CollisionComponent");
        MoveComponent mc = (MoveComponent)entity.GetComp("MoveComponent");

        cc.area.position = mc.pos.ToVector();

        SyncVector3 offset = new SyncVector3();

        for (int i = 0; i < cc.CollisionList.Count; i++)
        {
            if(cc.CollisionList[i].GetExistComp("BlockComponent"))
            {
                CollisionComponent cctmp = (CollisionComponent)cc.CollisionList[i].GetComp("CollisionComponent");

                offset += cctmp.area.GetOffsetPos(cc.area);
            }
        }
        mc.pos = mc.pos + offset;
    }
}
