using FrameWork;
using LockStepDemo.GameLogic.Component;
using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : CommandSyncSystem<CommandComponent>
{
    public override void BuildCommand(CommandComponent command)
    {
        command.isForward = Input.GetKey(KeyCode.W);
        command.isBack = Input.GetKey(KeyCode.S);
        command.isRight = Input.GetKey(KeyCode.D);
        command.isLeft = Input.GetKey(KeyCode.A);

        command.isFire = Input.GetMouseButton(0);
    }
}
