using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepInputSystem : CommandSyncSystem<CommandComponent>
{
    public override void BuildCommand(CommandComponent command)
    {
        if(m_world.FrameCount == 1)
        {
            command.moveDir.x = 1000;
        }
    }
}
