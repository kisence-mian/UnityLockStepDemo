//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//public class BuffSystem : SystemBase
//{
//    public override Type[] GetFilter()
//    {
//        return new Type[] {
//            typeof(PlayerComponent),
//        };
//    }

//    public override void FixedUpdate(int deltaTime)
//    {
//        List<EntityBase> list = GetEntityList();

//        for (int i = 0; i < list.Count; i++)
//        {
//            PlayerComponent pc = list[i].GetComp<PlayerComponent>();
//            BuffLogic(deltaTime,pc);
//        }
//    }

//    void BuffLogic(int deltaTime, PlayerComponent playerComp)
//    {
//        for (int i = 0; i < playerComp.buffList.Count; i++)
//        {
//            BuffInfo bi = playerComp.buffList[i];
//            bi.buffTime += deltaTime;

//            if(bi.BuffData.m_DamageNumber > 0)
//            {
//                bi.hitTime -= deltaTime;

//                if(bi.hitTime < 0)
//                {
//                    bi.hitTime = (int)(bi.BuffData.m_BuffEffectSpace * 1000);

//                    //伤害
//                    if (playerComp.Entity.GetExistComp<LifeComponent>())
//                    {
//                        LifeComponent lc = playerComp.Entity.GetComp<LifeComponent>();
//                        SkillUtils.Damage(m_world, m_world.GetEntity(bi.creater), playerComp.Entity, bi.BuffData.m_DamageNumber);

//                        m_world.eventSystem.DispatchEvent(GameUtils.c_HitBuff, playerComp.Entity, bi);
//                    }
//                }
//            }
            
//            if(bi.buffTime > bi.BuffData.m_BuffTime * 1000)
//            {
//                m_world.eventSystem.DispatchEvent(GameUtils.c_removeBuff, playerComp.Entity, bi);

//                playerComp.buffList.RemoveAt(i);
//                i--;
//            }
//        }
//    }
//}