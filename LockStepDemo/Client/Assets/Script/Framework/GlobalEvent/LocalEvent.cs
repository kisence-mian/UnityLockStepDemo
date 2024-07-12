using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEvent  {

    private  Dictionary<string, List<EventHandle>> mEventDic = new Dictionary<string, List<EventHandle>>();
    private  Dictionary<string, List<EventHandle>> mUseOnceEventDic = new Dictionary<string, List<EventHandle>>();

    /// <summary>
    /// 添加事件及回调
    /// </summary>
    /// <param name="eventName">事件枚举</param>
    /// <param name="handle">回调</param>
    /// <param name="isUseOnce"></param>
    public  void AddEvent(string eventName, EventHandle handle, bool isUseOnce = false)
    {
        if (isUseOnce)
        {
            if (mUseOnceEventDic.ContainsKey(eventName))
            {
                if (!mUseOnceEventDic[eventName].Contains(handle))
                    mUseOnceEventDic[eventName].Add(handle);
                else
                    Debug.LogWarning("already existing EventType: " + eventName + " handle: " + handle);
            }
            else
            {
                List<EventHandle> temp = new List<EventHandle>();
                temp.Add(handle);
                mUseOnceEventDic.Add(eventName, temp);
            }
        }
        else
        {
            if (mEventDic.ContainsKey(eventName))
            {
                if (!mEventDic[eventName].Contains(handle))
                    mEventDic[eventName].Add(handle);
                else
                    Debug.LogWarning("already existing EventType: " + eventName + " handle: " + handle);
            }
            else
            {
                List<EventHandle> temp = new List<EventHandle>();
                temp.Add(handle);
                mEventDic.Add(eventName, temp);
            }
        }
    }

    /// <summary>
    /// 移除某类事件的一个回调
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handle"></param>
    public  void RemoveEvent(string eventName, EventHandle handle)
    {
        if (mUseOnceEventDic.ContainsKey(eventName))
        {
            if (mUseOnceEventDic[eventName].Contains(handle))
            {
                mUseOnceEventDic[eventName].Remove(handle);
                if (mUseOnceEventDic[eventName].Count == 0)
                {
                    mUseOnceEventDic.Remove(eventName);
                }
            }
        }

        if (mEventDic.ContainsKey(eventName))
        {
            if (mEventDic[eventName].Contains(handle))
            {
                mEventDic[eventName].Remove(handle);
                if (mEventDic[eventName].Count == 0)
                {
                    mEventDic.Remove(eventName);
                }
            }
        }
    }

    /// <summary>
    /// 移除某类事件
    /// </summary>
    /// <param name="eventName"></param>
    public  void RemoveEvent(string eventName)
    {
        if (mUseOnceEventDic.ContainsKey(eventName))
        {

            mUseOnceEventDic.Remove(eventName);
        }

        if (mEventDic.ContainsKey(eventName))
        {

            mEventDic.Remove(eventName);
        }
    }

    /// <summary>
    /// 移除所有事件
    /// </summary>
    public  void RemoveAllEvent()
    {
        mUseOnceEventDic.Clear();

        mEventDic.Clear();
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="args"></param>
    public  void DispatchEvent(string eventName, params object[] args)
    {
        if (mEventDic.ContainsKey(eventName))
        {
            for (int i = 0; i < mEventDic[eventName].Count; i++)
            {
                try
                {
                    mEventDic[eventName][i](args);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        if (mUseOnceEventDic.ContainsKey(eventName))
        {
            for (int i = 0; i < mUseOnceEventDic[eventName].Count; i++)
            {
                try
                {
                    mUseOnceEventDic[eventName][i](args);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            RemoveEvent(eventName);
        }
    }
}
