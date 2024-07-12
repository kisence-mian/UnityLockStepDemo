using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBehaviorSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(PlayerComponent),
            typeof(PerfabComponent),
        };
    }

    public override void Init()
    {
        m_world.eventSystem.AddListener(GameUtils.c_addBuff, ReceviceAddBuff);
        m_world.eventSystem.AddListener(GameUtils.c_removeBuff, ReceviceRemoveBuff);
        m_world.eventSystem.AddListener(GameUtils.c_HitBuff, ReceviceBuffHit);
    }

    public override void Dispose()
    {
        m_world.eventSystem.RemoveListener(GameUtils.c_addBuff, ReceviceAddBuff);
        m_world.eventSystem.RemoveListener(GameUtils.c_removeBuff, ReceviceRemoveBuff);
        m_world.eventSystem.RemoveListener(GameUtils.c_HitBuff, ReceviceBuffHit);
    }

    public override void FixedUpdate(int deltaTime)
    {
        //List<EntityBase> list = GetEntityList();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    AddComp(list[i]);

        //    BuffEffectLogic(list[i]);
        //}
    }

    void AddComp(EntityBase entity)
    {
        if (!entity.GetExistComp<BuffEffectComponent>())
        {
            entity.AddComp<BuffEffectComponent>();
        }
    }

    void BuffEffectLogic(EntityBase entity)
    {
        BuffEffectComponent bec = entity.GetComp<BuffEffectComponent>();
        PlayerComponent pc      = entity.GetComp<PlayerComponent>();
        PerfabComponent pec     = entity.GetComp<PerfabComponent>();

        for (int i = 0; i < pc.buffList.Count; i++)
        {
            BuffEffect(entity,pc.buffList[i], bec);
        }
    }

    void ReceviceAddBuff(EntityBase entity, params object[] objs)
    {
        //Debug.Log("ReceviceAddBuff");

        AddComp(entity);

        PlayerComponent pc = entity.GetComp<PlayerComponent>();
        PerfabComponent pec = entity.GetComp<PerfabComponent>();

        BuffInfo bi = (BuffInfo)objs[0];

        if (bi.BuffData.m_BuffCreateFX != "null")
        {
            EffectManager.ShowEffect(bi.BuffData.m_BuffCreateFX, pec.perfab.transform.position, 0.5f);
        }

        BuffEffectLogic(entity);
    }

    void ReceviceRemoveBuff(EntityBase entity, params object[] objs)
    {
        //Debug.Log("ReceviceRemoveBuff");

        AddComp(entity);

        BuffEffectComponent bec = entity.GetComp<BuffEffectComponent>();
        PlayerComponent pc = entity.GetComp<PlayerComponent>();
        PerfabComponent pec = entity.GetComp<PerfabComponent>();

        BuffInfo bi = (BuffInfo)objs[0];

        if (bi.BuffData.m_BuffExitFX != "null")
        {
            EffectManager.ShowEffect(bi.BuffData.m_BuffExitFX, pec.perfab.transform.position, 0.5f);
        }

        BuffEffectPackage bep = bec.GetBuffEffectPackage(bi.buffID);

        //Debug.Log("bep >" + bep + "<");

        if (bep != null && bep.buffEffectID != 0)
        {
            m_world.DestroyEntity(bep.buffEffectID);
            bep.buffEffectID = 0;
        }
    }

    void ReceviceBuffHit(EntityBase entity, params object[] objs)
    {
        AddComp(entity);

        BuffEffectComponent bec = entity.GetComp<BuffEffectComponent>();
        PlayerComponent pc = entity.GetComp<PlayerComponent>();
        PerfabComponent pec = entity.GetComp<PerfabComponent>();

        BuffInfo bi = (BuffInfo)objs[0];

        if (bi.BuffData.m_BuffCreateFX != "null")
        {
            EffectManager.ShowEffect(bi.BuffData.m_BuffCreateFX, pec.perfab.transform.position, 0.5f);
        }
    }

    void BuffEffect(EntityBase entity,BuffInfo bi,BuffEffectComponent bec)
    {
        BuffEffectPackage bep = bec.GetBuffEffectPackage(bi.buffID);

        if(bep == null)
        {
            bep = new BuffEffectPackage();
            bep.buffID = bi.buffID;

            bec.effectList.Add(bep);
        }

        if(bi.BuffData.m_BuffFX != "null")
        {
            if(bep.buffEffectID != 0 && !m_world.GetEntityIsExist(bep.buffEffectID))
            {
                bep.buffEffectID = 0;
            }

            if (bep.buffEffectID == 0)
            {
                TransfromComponent mc = new TransfromComponent();
                mc.parentID = entity.ID;

                AssetComponent ac = new AssetComponent();
                ac.m_assetName = bi.BuffData.m_BuffFX;

                string identifier = "BuffEffect" + entity.ID + bi.buffID;

                int EffectID = m_world.GetEntityID(identifier);

                m_world.CreateEntity(identifier, mc, ac);

                bep.buffEffectID = EffectID;

                //Debug.Log("创建BUFF " + EffectID);
            }
            else
            {
                //Debug.Log("已存在");
            }
        }
    }
}
