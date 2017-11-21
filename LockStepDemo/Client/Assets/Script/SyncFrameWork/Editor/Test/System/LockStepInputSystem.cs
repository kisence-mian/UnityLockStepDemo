using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepInputSystem : CommandSyncSystem<TestCommandComponent>
{
    public static TestCommandComponent commandCache = new TestCommandComponent();

    public override void BuildCommand(TestCommandComponent command)
    {

        command.isFire = commandCache.isFire;

        command.moveDir = commandCache.moveDir;
        command.skillDir = commandCache.skillDir;
    }
}
