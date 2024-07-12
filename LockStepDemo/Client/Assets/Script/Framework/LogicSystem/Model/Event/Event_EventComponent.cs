using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ComponentName(LogicComponentType.Event, "事件/外部事件")]
[System.Serializable]
public  class Event_EventComponent : EventComponentBase
{
    public string eventName = "";
    protected override void Init()
    {
        if(!string.IsNullOrEmpty(eventName))
        logicObject.logicManager.AddEvent(eventName, EnumEventCallBack);  
    }
    private void EnumEventCallBack(params object[] objs)
    {
         EventComplete(objs);
    }
    public override void OnClose()
    {
        if (!string.IsNullOrEmpty(eventName))
            logicObject.logicManager.RemoveEvent(eventName, EnumEventCallBack);
       
    }

}
