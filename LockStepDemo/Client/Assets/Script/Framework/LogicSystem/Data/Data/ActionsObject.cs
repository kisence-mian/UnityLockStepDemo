using HDJ.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionsObject  {
    [NonSerialized]
    public bool enable = true;
    public int Count { get { return actionComponents.Count; } }

    public List<string> actionComponents = new List<string>();

    private List<ActionComponentBase> actionComponentObjs = new List<ActionComponentBase>();

    public void Init(LogicObject logicObject)
    {
        for (int i = 0; i < actionComponents.Count; i++)
        {
            ClassValue cv = JsonUtils.JsonToClassOrStruct<ClassValue>(actionComponents[i]);
            ActionComponentBase td = (ActionComponentBase)cv.GetValue();
            if (td != null)
            {
                td.Initialize(logicObject);
                actionComponentObjs.Add(td);
            }
        }
    }

    public void OnPause(bool isPause)
    {
        for (int i = 0; i < actionComponentObjs.Count; i++)
        {
            actionComponentObjs[i].OnPause(isPause);
        }
    }

    public void RunActions()
    {
        if (!enable)
            return;
        for (int i = 0; i < actionComponentObjs.Count; i++)
        {
            actionComponentObjs[i].RunAction();
        }
    }

    public void Close()
    {
        for (int i = 0; i < actionComponentObjs.Count; i++)
        {
            actionComponentObjs[i].OnClose();
        }
        actionComponentObjs.Clear();
    }

    }
