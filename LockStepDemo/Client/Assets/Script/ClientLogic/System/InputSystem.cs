using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : ViewSystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SelfComponent),
            typeof(CommandComponent)
        };
    }

    public override void BeforeUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent command = list[i].GetComp<CommandComponent>();

            command.isForward = Input.GetKey(KeyCode.W);
            command.isBack    = Input.GetKey(KeyCode.S);
            command.isRight   = Input.GetKey(KeyCode.D);
            command.isLeft    = Input.GetKey(KeyCode.A);

            command.isFire = Input.GetMouseButton(0);
        }
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent command = list[i].GetComp<CommandComponent>();
            ProtocolAnalysisService.SendCommand(command);
        }
    }
}
