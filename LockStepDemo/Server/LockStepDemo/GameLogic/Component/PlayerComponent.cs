using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerComponent : MomentComponentBase
{
    public SyncVector3 faceDir = new SyncVector3();

    public override MomentComponentBase DeepCopy()
    {
        PlayerComponent pc = new PlayerComponent();
        pc.faceDir = faceDir.DeepCopy();

        return pc;
    }
}
