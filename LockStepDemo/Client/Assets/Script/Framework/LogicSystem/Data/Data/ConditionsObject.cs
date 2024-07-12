using HDJ.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionsObject
{
    [NonSerialized]
    public bool enable = true;
    public int Count { get { return conditionComponents.Count; } }

    public List<string> conditionComponents = new List<string>();

    private List<ConditionComponentBase> conditionComponentObjs = new List<ConditionComponentBase>();
    public void Init(LogicObject logicObject)
    {
        for (int i = 0; i < conditionComponents.Count; i++)
        {
            ClassValue cv = JsonUtils.JsonToClassOrStruct<ClassValue>(conditionComponents[i]);
            ConditionComponentBase td = (ConditionComponentBase)cv.GetValue();
            if (td != null)
            {
                td.Initialize(logicObject);
                conditionComponentObjs.Add(td);
            }
        }
    }
    public void OnPause(bool isPause)
    {
        for (int i = 0; i < conditionComponentObjs.Count; i++)
        {
            conditionComponentObjs[i].OnPause(isPause);
        }

    }
    public bool ConditionCompare(params object[] objs)
    {
        if (!enable)
            return false;
        for (int i = 0; i < conditionComponentObjs.Count; i++)
        {
            ConditionComponentBase td = conditionComponentObjs[i];
            if (!td.CompareCondition(objs)) return false;

        }
        return true;
    }
    public void Close()
    {     
        for (int i = 0; i < conditionComponentObjs.Count; i++)
        {
            conditionComponentObjs[i].OnClose();
        }
     
        conditionComponentObjs.Clear();       
    }
}
