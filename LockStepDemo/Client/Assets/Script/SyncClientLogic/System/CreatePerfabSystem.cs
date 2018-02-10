using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePerfabSystem : ViewSystemBase
{

    public override void Init()
    {
        AddEntityOptimizeDestroyLisnter();
        AddEntityOptimizeCreaterLisnter();
    }

    public override void Dispose()
    {
        RemoveEntityOptimizeDestroyLisnter();
        RemoveEntityOptimizeDestroyLisnter();
    }

    //public override Type[] GetFilter()
    //{
    //    return new Type[] {
    //        typeof(AssetComponent),
    //        typeof(TransfromComponent),
    //    };
    //}

    public override void OnEntityOptimizeDestroy(EntityBase entity)
    {
        //if (entity.GetExistComp<PerfabComponent>())
        //{
        //    Debug.Log("接收到销毁 ");

        //    PerfabComponent pc = entity.GetComp<PerfabComponent>();

        //    if(pc.perfab != null)
        //    {
        //        GameObjectManager.DestroyGameObjectByPool(pc.perfab);
        //        pc.perfab = null;
        //    }

        //}
    }

    public override void OnEntityOptimizeCreate(EntityBase entity)
    {
        if (GetAllExistComp(new string[] { "AssetComponent", "TransfromComponent" },entity))
        {
            Debug.Log("接收到创建 ");

            AddComp(entity);

            AssetComponent ac = entity.GetComp<AssetComponent>();
            TransfromComponent tc = entity.GetComp<TransfromComponent>();

            PerfabComponent comp = entity.GetComp<PerfabComponent>();
            comp.perfab = GameObjectManager.CreateGameObject(ac.m_assetName);
            comp.hardPoint = comp.perfab.GetComponent<HardPointComponent>();

            if (tc.parentID == 0)
            {
                comp.perfab.transform.position = tc.pos.ToVector();
            }
            else
            {
                EntityBase parent = m_world.GetEntity(tc.parentID);
                //if (parent.GetExistComp<PerfabComponent>())
                //{
                //    PerfabComponent pc = parent.GetComp<PerfabComponent>();

                //    comp.perfab.transform.SetParent(pc.perfab.transform);

                //}
                comp.perfab.transform.localPosition = tc.pos.ToVector();
            }

            //创建动画组件
            if (comp.perfab.GetComponent<Animator>() != null)
            {
                AnimComponent anc = entity.AddComp<AnimComponent>();
                anc.anim = comp.perfab.GetComponent<Animator>();
                anc.perfab = comp.perfab;
                anc.waistNode = comp.hardPoint.waistNode;
            }
        }
    }

    void AddComp(EntityBase entity)
    {
        //if (!entity.GetExistComp<PerfabComponent>())
        //{
        //    entity.AddComp<PerfabComponent>();
        //}
    }
}
