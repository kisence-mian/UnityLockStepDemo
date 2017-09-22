using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurgenceUISystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SelfComponent),
            typeof(LifeComponent),
        };
    }

    public override void Dispose()
    {
        if (UIManager.GetUI<ResurgenceWindow>() != null)
        {
            UIManager.CloseUIWindow<ResurgenceWindow>();
        }
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            LifeComponent lc = list[i].GetComp<LifeComponent>();
            if(lc.Life < 0)
            {
                if(UIManager.GetUI<ResurgenceWindow>() == null)
                {
                    UIManager.OpenUIWindow<ResurgenceWindow>();
                }
            }
            else
            {
                if (UIManager.GetUI<ResurgenceWindow>() != null)
                {
                    UIManager.CloseUIWindow<ResurgenceWindow>();
                }
            }
        }
    }
}
