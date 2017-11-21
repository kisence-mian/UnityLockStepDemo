using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestInitSystem : InitSystemBase
{
    public override void OnPlayerJoin(EntityBase entity)
    {
        if (!entity.GetExistComp<TestMoveComponent>())
        {
            TestMoveComponent c = new TestMoveComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<TestCommandComponent>())
        {
            TestCommandComponent c = new TestCommandComponent();
            entity.AddComp(c);
        }
    }
}
