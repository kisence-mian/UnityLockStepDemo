using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepInputSystem : CommandSyncSystem<CommandComponent>
{
    public static CommandComponent commandCache = new CommandComponent();

    public override void BuildCommand(CommandComponent command)
    {

        command.isFire = commandCache.isFire;

        command.moveDir = commandCache.moveDir;
        command.skillDir = commandCache.skillDir;
    }
}
