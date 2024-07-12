using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ComponentName(LogicComponentType.Action, "系统/组件控制")]
[Serializable]
public class Action_System_ComponetControl : ActionComponentBase
{
    public List<int> componetIdList = new List<int>();
    public bool enableEvent = true;
    public bool enableCondition = true;
    public bool enableAction = true;
    //是否要执行子节点
    public bool runChild = true;

    protected override void Action()
    {
       for(int i =0;i< componetIdList.Count; i++)
        {
            LogicObject to = logicObject.logicManager.data.GetLogicObject(componetIdList[i]);
            if (to != null)
            {              
                to.events.enable = enableEvent;
                to.conditions.enable = enableCondition;
                to.actions.enable = enableAction;
                to.runChild = runChild;              
            }
        }
    }
}
