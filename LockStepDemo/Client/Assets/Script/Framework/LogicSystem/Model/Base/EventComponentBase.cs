using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventComponentBase :LogicComponentBase
{
    protected void EventComplete(params object[] objs)
    {
        UpdateInternalValue();
        if (logicObject != null && logicObject.events.enable)
            logicObject.RunConditionCompare(objs);
    }
 
}
