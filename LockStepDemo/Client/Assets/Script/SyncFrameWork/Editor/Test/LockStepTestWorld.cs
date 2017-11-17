using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestWorld : GameWorldBase
{
    public override Type[] GameSystems()
    {
        return new Type[] {

            typeof(LockStepInputSystem),
                        //逻辑层
            typeof(CollisionSystem),
            typeof(OperationSystem),
            typeof(BlowFlySystem),
            typeof(MoveSystem),
            typeof(TranslationOverlapSystem),
            typeof(FireSystem),
            typeof(SkillStatusSystem),
            typeof(SkillSystem),
            typeof(LifeSpanSystem),
            typeof(CollisionDamageSystem),
            typeof(FlyObjectCollisionSystem),
            typeof(GameSystem),
            typeof(ResurgenceSystem),
            typeof(CreateItemSystem),
            typeof(ItemSystem),
            typeof(RankSystem),
            typeof(BuffSystem),
            typeof(GrassSystem),

            typeof(InitSystem),


        };
    }

    public override Type[] GetRecordTypes()
    {
        return new Type[] {
            typeof(LifeSpanComponent),
                typeof(MoveComponent),
                typeof(PlayerComponent),
                typeof(LifeComponent),
                typeof(SkillStatusComponent),
                typeof(BlowFlyComponent),
                typeof(FlyObjectComponent)
        };
    }
}
