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
        CampComponent acc = entity.GetComp<CampComponent>();

        if (cc.CollisionList.Count > 0)
        {
            for (int i = 0; i < cc.CollisionList.Count; i++)
            {
                if (cc.CollisionList[i].GetExistComp<LifeComponent>()
                    && cc.CollisionList[i].GetExistComp<CampComponent>()
                    && acc.creater != cc.CollisionList[i].GetComp<CampComponent>().creater)
                {
                    //Debug.Log("fly DamageLogic frame-> " + m_world.FrameCount + "  id " + cc.CollisionList[i].ID + " Fly ID " + entity.ID);
                    if (!fc.damageList.Contains(cc.CollisionList[i].ID)) //第一次伤害
                    {
                        SkillUtils.FlyDamageLogic(m_world, entity, cc.CollisionList[i]);
                        fc.damageList.Add(cc.CollisionList[i].ID);
                    }
                        

                    //不能穿人销毁飞行物
                    if (!fc.FlyData.m_AcrossEnemy)
                        m_world.ClientDestroyEntity(entity.ID);

                    //Debug.Log("fc.FlyData.m_AcrossEnemy " + fc.FlyData.m_AcrossEnemy);
                }

                if (cc.CollisionList[i].GetExistComp<BlockComponent>())
                {
                    //不能穿墙摧毁飞行物
                    if (fc.FlyData.m_CollisionTrigger)
                        m_world.ClientDestroyEntity(entity.ID);
                }
            }
        }
    }
}

