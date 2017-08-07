using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DemoWorld : WorldBase
{
    public override Type[] GetSystemTypes()
    {
        return new Type[] {
            typeof(InitSystem),
            typeof(SyncSystem),
            typeof(CountSystem),
            typeof(MoveSystem),
            typeof(OperationSystem)
        };
    }

    public override Type[] GetViewSystemTypes()
    {
        return new Type[] {
            typeof(CreatePerfabSystem),
            typeof(MovePerfabSystem),
            typeof(InputSystem)
        };
    }
}
