using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

[Module(3, "Command")]
public abstract class PlayerCommandBase : ComponentBase, CsharpProtocolInterface
{
    public int id;
    public int frame;

    //public virtual PlayerCommandBase DeepCopy()
    //{

    //}
}
