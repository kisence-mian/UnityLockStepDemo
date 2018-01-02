using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepEntityTestWorld : GameWorldBase<CommandComponent>
{
    public override Type[] GameSystems()
    {
        return new Type[] {

            typeof(LockStepInputSystem),

            typeof(LockStepTestFireSystem),

            //逻辑层
            typeof(LockStepTestMoveSystem),
            typeof(LockStepTestLifeSpanSystem),

            typeof(LockStepTestInitSystem),
        };
    }

    public override Type[] GetRecordTypes()
    {
        return new Type[]
        {
            typeof(LifeSpanComponent),
            typeof(MoveComponent),
        };

    }

    public override Type[] GetRecordSingletonTypes()
    {
        return new Type[]
        {
             //typeof(TestSingleComponent),
        };
    }
}
