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
            typeof(CampComponent),
            typeof(FlyObjectComponent),
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
        FlyObjectComponent fc = entity.GetComp<FlyObjectComponent>();

        if (cc.CollisionList.Count > 0)
        {
            Debug.Log("collision " + entity.ID);

            for (int i = 0; i < cc.CollisionList.Count; i++)
            {
                Debug.Log("碰撞 --> " + cc.CollisionList[i].ID );

                if (cc.CollisionList[i].GetExistComp<LifeComponent>()
                    && fc.createrID != cc.CollisionList[i].ID)
                {
                    SkillUtils.FlyDamageLogic(m_world,entity, cc.CollisionList[i]);

                    //不能穿人销毁飞行物
                    if(!fc.FlyData.m_AcrossEnemy)
                        m_world.ClientDestroyEntity(entity.ID);
                }

                if(cc.CollisionList[i].GetExistComp<BlockComponent>())
                {
                    //不能穿墙摧毁飞行物
                    if (fc.FlyData.m_CollisionTrigger)
                        m_world.ClientDestroyEntity(entity.ID);
                }
            }
        }
    }
}

