using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncSystem : SystemBase
{
    public override void Init()
    {

    }

    public override void LateUpdate(int deltaTime)
    {
        SyncComponent.currentFixedFrame++;
        SyncComponent.currentTime = deltaTime / 1000f;
    }
}
