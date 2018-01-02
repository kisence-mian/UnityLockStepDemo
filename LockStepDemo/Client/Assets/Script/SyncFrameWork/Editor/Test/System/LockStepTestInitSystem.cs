using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepTestInitSystem : InitSystemBase
{
    public override void OnPlayerJoin(EntityBase entity)
    {
        if (!entity.GetExistComp(ComponentType.MoveComponent))
        {
            MoveComponent c = new MoveComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp(ComponentType.CommandComponent))
        {
            CommandComponent c = new CommandComponent();
            entity.AddComp(c);
        }
    }
}
