using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem  : SystemBase
{
    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            PerfabComponent pc = list[i].GetComp<PerfabComponent>();
            //m_world.GetSingletonComp<PlayerCameraComponent>().cam.m_target = pc.perfab;
        }
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SelfComponent),
            typeof(PerfabComponent),
        };
    }
}
