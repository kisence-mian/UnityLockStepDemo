using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandComponent : PlayerCommandBase
{
    public bool isForward;
    public bool isBack;
    public bool isRight;
    public bool isLeft;

    public bool isFire;

    public override PlayerCommandBase DeepCopy()
    {
        CommandComponent cc = new CommandComponent();

        cc.id = id;
        cc.frame = frame;

        cc.isFire    = isFire;
        cc.isForward = isForward;
        cc.isBack    = isBack;
        cc.isRight   = isRight;
        cc.isLeft    = isLeft;

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

        if (isForward != cc.isForward)
            return false;

        if (isBack != cc.isBack)
            return false;

        if (isRight != cc.isRight)
            return false;

        if (isLeft != cc.isLeft)
            return false;

        return true;
    }
}
