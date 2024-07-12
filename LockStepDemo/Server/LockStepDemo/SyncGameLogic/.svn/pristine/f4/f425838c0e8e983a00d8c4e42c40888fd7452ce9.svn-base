using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DemoWorld : WorldBase
{
    public override Type[] GetSystemTypes()
    {
        return new Type[] {
            //逻辑层
            typeof(CollisionSystem),
            typeof(OperationSystem),
            typeof(BlowFlySystem),
            typeof(MoveSystem),
            typeof(FireSystem),
            typeof(SkillStatusSystem),
            typeof(SkillSystem),
            typeof(LifeSpanSystem),
            typeof(CollisionSystem),
            typeof(CollisionDamageSystem),
            typeof(FlyObjectCollisionSystem),
            typeof(InitSystem),
            
            //表现层
            typeof(HealthBarSystem),
            typeof(CameraSystem),
            typeof(CreatePerfabSystem),
            typeof(MovePerfabSystem),
            typeof(InputSystem),
            typeof(SyncSystem<CommandComponent>),
            typeof(PlayerAnimSystem),
            typeof(SkillBehaviorSystem),
            typeof(DestroyEffectSystem),
            typeof(ClientOperationSystem),

            typeof(SyncDebugSystem)
        };
    }

    public override Type[] GetRecordTypes()
    {
        return new Type[] {

            typeof(CDComponent),
            typeof(LifeSpanComponent),
            typeof(MoveComponent),
            typeof(PlayerComponent),
            typeof(LifeComponent),
            typeof(SkillStatusComponent),
            typeof(BlowFlyComponent),
        };
    }

    public override Type[] GetRecordSystemTypes()
    {
        return new Type[] {
            typeof(EntityRecordSystem),
        };
    }
}
