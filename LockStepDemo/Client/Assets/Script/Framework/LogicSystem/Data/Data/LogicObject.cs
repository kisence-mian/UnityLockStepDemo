using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LogicComponentType
{
    Event,
    Condition,
    Action,
}
[Serializable]
public class LogicObject  {
    [NonSerialized]
    public LogicRuntimeMachine logicManager;
    public string name="";
    public int id =0;
    public bool isSupportAlwaysActive = false;
    [NonSerialized]
    //是否要执行子节点
    public bool runChild = true;


    public EventsObject events = new EventsObject();
    public ConditionsObject conditions = new ConditionsObject();
    public ActionsObject actions = new ActionsObject();
  
    public List<int> childObjects = new List<int>();
    public Vector3 editorPos = Vector3.zero;

    private bool isInit = false;
    public void Init(LogicRuntimeMachine logicManager)
    {
        this.logicManager = logicManager;
        if (isInit)
            return;
        isInit = true;    
        events.Init(this,id);
        conditions.Init(this);
        actions.Init(this);    
        if(events.Count ==0)
                 RunConditionCompare();

        logicManager.runningLogicObjects.Add(id);
    }
    public void SetLogicConponetTypeEnable(LogicComponentType type, bool enable)
    {
        if (type == LogicComponentType.Event)
            events.enable = enable;
        else if (type == LogicComponentType.Condition)
            conditions.enable = enable;
        else
            actions.enable = enable;
    }
    public void OnPause(bool isPause)
    {
        events.OnPause(isPause);
        conditions.OnPause(isPause);
        actions.OnPause(isPause);
    }
    public void RunConditionCompare(params object[] objs)
    {
        if (!conditions.ConditionCompare(objs))
            return;
        logicManager.SendRunActions(id);
    }

    public void RunActions()
    {
        actions.RunActions();
        Close();
        if (!runChild)
            return;
        for (int i = 0; i < childObjects.Count; i++)
        {
            LogicObject temp = logicManager.data.GetLogicObject(childObjects[i]);
            if (temp != null)
                temp.Init(logicManager);
        }
        if (isSupportAlwaysActive)
        {
            if (events.Count == 0)
            {
                Debug.LogWarning("逻辑状态模块无事件只执行一次 Name：" + name + "  id: " + id);
                return;
            }
            else
                Init(logicManager);
        }
    }

    public void Close()
    {
        events.Close();
        conditions.Close();
        actions.Close();
        logicManager.runningLogicObjects.Remove(id);
        isInit = false;
    }


}



  

