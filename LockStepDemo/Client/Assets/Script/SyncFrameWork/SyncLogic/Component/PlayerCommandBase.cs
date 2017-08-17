using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

[Module(3, "Command")]
public abstract class PlayerCommandBase : ComponentBase, CsharpProtocolInterface
{
    public int id;
    public int frame;

    public abstract PlayerCommandBase DeepCopy();

    public abstract bool EqualsCmd(PlayerCommandBase cmd);
}
