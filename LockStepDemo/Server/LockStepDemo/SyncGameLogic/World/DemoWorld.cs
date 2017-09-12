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
            typeof(CollisionDamageSystem),
            typeof(FlyObjectCollisionSystem),
            typeof(GameSystem),
            typeof(ResurgenceSystem),
            typeof(CreateItemSystem),
            typeof(ItemSystem),
            typeof(RankSystem),

            //初始化
            typeof(InitSystem),
            
            //服务器系统
            typeof(ConnectSystem),
            typeof(PlayerInputSystem),
            typeof(ServiceSyncSystem),

            //Debug
            typeof(SyncDebugSystem),
        };
    }

    public override Type[] GetRecordTypes()
    {
        return new Type[] {
        };
    }

    public override Type[] GetRecordSystemTypes()
    {
        return new Type[] {
        };
    }
}
