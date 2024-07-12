using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicRuntimeMachine  {

    public LogicObjectContainer data;
    private int id = 0;
    public int Id { get { return id; } }
    //内部变量值变化事件
    private Dictionary<string, CallBack<string, object>> inValueEventDic = new Dictionary<string, CallBack<string, object>>();

    //外部向内部发送事件
    private LocalEvent localEvent = new LocalEvent();
    //运行时传入的GameObject，以便组件使用
    private Dictionary<string, GameObject> runtimeGameObjects = new Dictionary<string, GameObject>();
    //在内部运行的子逻辑状态机LogicRuntimeMachine
    private Dictionary<string, int> internalLogicMachineDic = new Dictionary<string, int>();
    /// <summary>
    /// 正在运行的模块
    /// </summary>
    public List<int> runningLogicObjects = new List<int>();
    //当前是否暂停
    private bool isPause = false;
    public bool IsPause { get { return isPause; }  }

    public LogicRuntimeMachine(int id, LogicObjectContainer data)
    {
        this.id = id;
        this.data = data;
        for (int i = 0; i < data.internalValueList.Count; i++)
        {
            BaseValue bv = data.internalValueList[i];
            inValueEventDic.Add(bv.name, null);
        }
       
    }
    public void Start()
    {
        data.Init(this);
    }

    #region 管理运行的子逻辑状态机
    public void AddChildLogicMachine(string name,int id)
    {
        if (!internalLogicMachineDic.ContainsKey(name))
            internalLogicMachineDic.Add(name, id);
        else
            Debug.LogError("子状态机的名字重复了:" + name);
    }
    public void StopChildLogicMachine(string name)
    {
        if (internalLogicMachineDic.ContainsKey(name))
        {
            int id = internalLogicMachineDic[name];
            internalLogicMachineDic.Remove(name);
            LogicSystemManager.StopLogicRuntimeMachine(id);
        }
    }
#endregion

    #region 外部事件
    public void DispatchEvent(string eventName, params object[] args)
    {
        localEvent.DispatchEvent(eventName, args);
        List<int> list = new List<int>(internalLogicMachineDic.Values);
        for (int i = 0; i < list.Count; i++)
        {
            LogicRuntimeMachine lom = LogicSystemManager.GetLogicRuntimeMachine(list[i]);
            lom.DispatchEvent(eventName, args);
        }
    }
    /// <summary>
    /// 添加事件及回调
    /// </summary>
    /// <param name="eventName">事件枚举</param>
    /// <param name="handle">回调</param>
    /// <param name="isUseOnce"></param>
    public void AddEvent(string eventName, EventHandle handle)
    {
        localEvent.AddEvent(eventName, handle);
    }
    /// <summary>
    /// 移除某类事件的一个回调
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handle"></param>
    public void RemoveEvent(string eventName, EventHandle handle)
    {
        localEvent.RemoveEvent(eventName, handle);
    }
#endregion

    #region 内部变量变化事件
    /// <summary>
    /// 注册内部变量变化事件
    /// </summary>
    public void RegisterInternalValueChangeEvent(string name,CallBack<string,object> changeEvent)
    {
        if (inValueEventDic.ContainsKey(name))
        {
            if (inValueEventDic[name] == null)
            {
                inValueEventDic[name] = new CallBack<string, object>(changeEvent);
            }
            else
            {
                inValueEventDic[name] += changeEvent;
            }
        }
        else
        {
            Debug.LogError("不包含内部变量，不能注册事件. name: " + name);
        }
    }
    /// <summary>
    /// 移除内部变量变化事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="changeEvent"></param>
    public void RemoveInternalValueChangeEvent(string name, CallBack<string, object> changeEvent)
    {
        if (inValueEventDic.ContainsKey(name))
        {
            if (inValueEventDic[name] == null)
            {
                Debug.LogError("不包含事件，不能移除事件. name: " + name);
            }
            else
            {
                inValueEventDic[name] -= changeEvent;
            }
        }
        else
        {
            Debug.LogError("不包含内部变量，不能注册事件. name: " + name);
        }
    }
    #endregion

    #region 设置内部变量
    /// <summary>
    /// 设置内部变量的值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetInternalValue(string name, object value)
    {
        BaseValue bv =data. GetBaseValue(name);
        if (bv != null)
        {
            if (!bv.GetValue().Equals(value))
            {
                if (inValueEventDic[name] != null)
                    inValueEventDic[name](name, value);
            }
            bv.SetValue(value);

        }
    }
    /// <summary>
    /// 获取内部变量的值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetInternalValue(string name)
    {
        BaseValue bv =data. GetBaseValue(name);
        if (bv != null)
            return bv.GetValue();
        else
        {
            Debug.LogError("不存在内部变量：" + name);
            return null;
        }
    }
#endregion

    /// <summary>
    /// 运行时添加GameObject变量
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void AddRuntimeGameObjects(string name,GameObject obj)
    {
        if (runtimeGameObjects.ContainsKey(name))
            runtimeGameObjects[name] = obj;
        else
            runtimeGameObjects.Add(name, obj);
    }
    /// <summary>
    /// 运行时获取GameObject
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetRuntimeGameObjects(string name)
    {
        if (runtimeGameObjects.ContainsKey(name))
            return runtimeGameObjects[name];
        else
            return null;
    }
    public void SendRunActions(int id)
    {
        LogicSystemManager.NetworkSendActionId(Id, id);
    }
    public void RunActions(int id)
    {
        LogicObject to = data.GetLogicObject(id);
        to.RunActions();
               
    }

    public void Close()
    {
        data.Close();
        localEvent.RemoveAllEvent();
        inValueEventDic.Clear();
        runningLogicObjects.Clear();
        runtimeGameObjects.Clear();
        internalLogicMachineDic.Clear();
    }

    public void Pause(bool isPause)
    {
        if (isPause == this.isPause)
            return;
        this.isPause = isPause;
        for (int i = 0; i < runningLogicObjects.Count; i++)
        {
            LogicObject to = data.GetLogicObject(runningLogicObjects[i]);
            if (to != null)
                to.OnPause(isPause);
        }
        List<int> dis = new List<int>(internalLogicMachineDic.Values);
        for (int i = 0; i < dis.Count; i++)
        {
            LogicSystemManager.PauseLogicRuntimeMachine(i,isPause);
        }
    }
	
}
