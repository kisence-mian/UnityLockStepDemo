using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionComponentBase : LogicComponentBase
{
    public void RunAction()
    {
        UpdateInternalValue();
        Action();
    }
    protected virtual void Action() { }
       
    
}
