using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        cc.isFire = isFire;
        cc.isForward = isForward;
        cc.isBack = isBack;
        cc.isRight = isRight;
        cc.isLeft = isLeft;

        return cc;
    }
}