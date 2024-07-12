using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using HDJ.Framework.Utils;
[Serializable]
public class EventsObject  {
    [NonSerialized]
    public bool enable = true;
    public int Count { get {return eventComponents.Count; } }

    public List<string> eventComponents = new List<string>();
    private List<EventComponentBase> eventComponentObjs = new List<EventComponentBase>();

    public void Init(LogicObject logicObject,int logicObjId)
    {
        for (int i = 0; i < eventComponents.Count; i++)
        {
            ClassValue cv = JsonUtils.JsonToClassOrStruct<ClassValue>(eventComponents[i]);
            EventComponentBase eventTriggerObj = (EventComponentBase)cv.GetValue();
            if (eventTriggerObj != null)
            {
                eventTriggerObj.Initialize(logicObject);
                eventComponentObjs.Add(eventTriggerObj);
            }
        }
    }
    public void OnPause(bool isPause)
    {
        for (int i = 0; i < eventComponentObjs.Count; i++)
        {
            eventComponentObjs[i].OnPause(isPause);
        }
       
    }
    public void Close()
    {
        for (int i = 0; i < eventComponentObjs.Count; i++)
        {
            eventComponentObjs[i].OnClose();
        }
        eventComponentObjs.Clear();
    }


}
