using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ComponentName(LogicComponentType.Event, "数值/数值变化事件")]
[System.Serializable]
public class Event_Value_InternalValueChange : EventComponentBase
{
    [InternalValueMenu]
    public string internalValueName = "";

    protected override void Init()
    {
        logicObject.logicManager.RegisterInternalValueChangeEvent(internalValueName, InternalValueChangeCallBack);
    }

    void InternalValueChangeCallBack(string name,object value)
    {
        EventComplete();
    }

    public override void OnClose()
    {
        logicObject.logicManager.RemoveInternalValueChangeEvent(internalValueName, InternalValueChangeCallBack);
    }
}
