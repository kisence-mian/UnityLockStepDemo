using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandComponent : PlayerCommandBase
{
    public SyncVector3 moveDir = new SyncVector3();
    public SyncVector3 skillDir = new SyncVector3();

    public bool isFire = false;

    public override PlayerCommandBase DeepCopy()
    {
        CommandComponent cc = new CommandComponent();

        cc.id = id;
        cc.frame = frame;

        cc.isFire    = isFire;
        cc.moveDir   = moveDir.DeepCopy();
        cc.skillDir  = skillDir.DeepCopy();

        return cc;
    }

    public override bool EqualsCmd(PlayerCommandBase cmd)
    {
        if(!(cmd is CommandComponent))
        {
            return false;
        }

        CommandComponent cc = cmd as CommandComponent;

        if (id != cc.id)
            return false;

        if (frame != cc.frame)
            return false;

        if (isFire != cc.isFire)
            return false;

        if (!moveDir.Equals(cc.moveDir))
            return false;

        if (!skillDir.Equals(cc.skillDir))
            return false;
        return true;
    }
}
