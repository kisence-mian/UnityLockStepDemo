using Assets.Script.SyncFrameWork.SyncLogic.System;
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
            typeof(MoveSystem),
            typeof(OperationSystem),

            typeof(CreatePerfabSystem),
            typeof(MovePerfabSystem),
            typeof(InputSystem),
            typeof(SyncSystem<CommandComponent>),

            typeof(SyncDebugSystem)
        };
    }

    public override Type[] GetRecordSystemTypes()
    {
        return new Type[] {

            typeof(CDComponent),
            typeof(MoveComponent),
        };
    }
}
