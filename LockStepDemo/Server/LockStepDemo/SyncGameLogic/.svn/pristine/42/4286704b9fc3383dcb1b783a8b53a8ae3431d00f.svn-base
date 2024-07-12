using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandComponent : PlayerCommandBase
{
    public SyncVector3 moveDir = new SyncVector3();
    public SyncVector3 skillDir = new SyncVector3();

    public int element1;
    public int element2;

    public bool isFire = false;

    public override PlayerCommandBase DeepCopy()
    {
        CommandComponent cc = new CommandComponent();

        cc.id = id;
        cc.frame = frame;

        cc.isFire    = isFire;
        cc.moveDir   = moveDir.DeepCopy();
        cc.skillDir  = skillDir.DeepCopy();

        cc.element1 = element1;
        cc.element2 = element2;

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

        if (element1 != cc.element1)
            return false;

        if (element2 != cc.element2)
            return false;

        if (!moveDir.Equals(cc.moveDir))
            return false;

        if (!skillDir.Equals(cc.skillDir))
            return false;

        return true;
    }
}
