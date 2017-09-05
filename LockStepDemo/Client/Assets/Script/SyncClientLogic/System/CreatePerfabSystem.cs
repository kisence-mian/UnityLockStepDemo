using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePerfabSystem : ViewSystemBase
{

    public override void Init()
    {
        AddEntityDestroyLisnter();
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(AssetComponent),
            typeof(MoveComponent),
        };
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            if(!list[i].GetExistComp<PerfabComponent>())
            {
                MoveComponent mc = list[i].GetComp<MoveComponent>();

                PerfabComponent comp = list[i].AddComp<PerfabComponent>();
                comp.perfab = GameObjectManager.CreateGameObject(list[i].GetComp<AssetComponent>().m_assetName);
                //comp.hardPoint = comp.perfab.GetComponent<HardPointComponent>();

                comp.perfab.transform.position = mc.pos.ToVector();

                //创建动画组件
                if (comp.perfab.GetComponent<Animator>() != null)
                {
                    AnimComponent ac = list[i].AddComp<AnimComponent>();
                    ac.anim = comp.perfab.GetComponent<Animator>();
                    ac.perfab = comp.perfab;
                    //ac.waistNode = comp.hardPoint.waistNode;
                }
            }
        }
    }

    public override void OnEntityDestroy(EntityBase entity)
    {
        if (entity.GetExistComp<PerfabComponent>())
        {
            PerfabComponent pc = entity.GetComp<PerfabComponent>();
            GameObjectManager.DestroyGameObjectByPool(pc.perfab);
        }
    }
}
