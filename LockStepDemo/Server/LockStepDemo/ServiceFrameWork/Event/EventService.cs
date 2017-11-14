using LockStepDemo.Service;
using Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public delegate void EventHandle(params object[] args);
public delegate void MessageHandle<T>(SyncSession session, T e);
class EventService
{
    #region 以Enum为Key的事件派发

    private static ConcurrentDictionary<Enum, List<EventHandle>> mEventDic = new ConcurrentDictionary<Enum, List<EventHandle>>();
    private static ConcurrentDictionary<Enum, List<EventHandle>> mUseOnceEventDic = new ConcurrentDictionary<Enum, List<EventHandle>>();
    static List<EventHandle> s_removeCache = new List<EventHandle>();
    /// <summary>
    /// 添加事件及回调
    /// </summary>
    /// <param name="type">事件枚举</param>
    /// <param name="handle">回调</param>
    /// <param name="isUseOnce"></param>
    public static void AddEvent(Enum type, EventHandle handle, bool isUseOnce = false)
    {
        if (isUseOnce)
        {
            if (mUseOnceEventDic.ContainsKey(type))
            {
                if (!mUseOnceEventDic[type].Contains(handle))
                    mUseOnceEventDic[type].Add(handle);
                else
                    Debug.LogWarning("already existing EventType: " + type + " handle: " + handle);
            }
            else
            {
                List<EventHandle> temp = new List<EventHandle>();
                temp.Add(handle);
                mUseOnceEventDic.TryAdd(type, temp);
            }
        }
        else
        {
            if (mEventDic.ContainsKey(type))
            {
                if (!mEventDic[type].Contains(handle))
                    mEventDic[type].Add(handle);
                else
                    Debug.LogWarning("already existing EventType: " + type + " handle: " + handle);
            }
            else
            {
                List<EventHandle> temp = new List<EventHandle>();
                temp.Add(handle);
                mEventDic.TryAdd(type, temp);
            }
        }
    }

    /// <summary>
    /// 移除某类事件的一个回调
    /// </summary>
    /// <param name="type"></param>
    /// <param name="handle"></param>
    public static void RemoveEvent(Enum type, EventHandle handle)
    {
        if (mUseOnceEventDic.ContainsKey(type))
        {
            if (mUseOnceEventDic[type].Contains(handle))
            {
                mUseOnceEventDic[type].Remove(handle);
                if (mUseOnceEventDic[type].Count == 0)
                {
                    mUseOnceEventDic.TryRemove(type,out s_removeCache);
                }
            }
        }

        if (mEventDic.ContainsKey(type))
        {
            if (mEventDic[type].Contains(handle))
            {
                mEventDic[type].Remove(handle);
                if (mEventDic[type].Count == 0)
                {
                    mEventDic.TryRemove(type,out s_removeCache);
                }
            }
        }
    }

    /// <summary>
    /// 移除某类事件
    /// </summary>
    /// <param name="type"></param>
    public static void RemoveEvent(Enum type)
    {
        if (mUseOnceEventDic.ContainsKey(type))
        {
            mUseOnceEventDic.TryRemove(type,out s_removeCache);
        }

        if (mEventDic.ContainsKey(type))
        {
            mEventDic.TryRemove(type,out s_removeCache);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    public static void DispatchEvent(Enum type, params object[] args)
    {
        if (mEventDic.ContainsKey(type))
        {
            for (int i = 0; i < mEventDic[type].Count; i++)
            {
                //遍历委托链表
                foreach (EventHandle callBack in mEventDic[type][i].GetInvocationList())
                {
                    try
                    {
                        callBack(args);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
        }

        if (mUseOnceEventDic.ContainsKey(type))
        {
            for (int i = 0; i < mUseOnceEventDic[type].Count; i++)
            {
                //遍历委托链表
                foreach (EventHandle callBack in mUseOnceEventDic[type][i].GetInvocationList())
                {
                    try
                    {
                        callBack(args);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
            RemoveEvent(type);
        }
    }

    #endregion

    #region 消息派发

    private static Dictionary<Type, EventDispatcher> mTypeEventDic = new Dictionary<Type, EventDispatcher>();
    private static Dictionary<Type, EventDispatcher> mTypeUseOnceEventDic = new Dictionary<Type, EventDispatcher>();

    /// <summary>
    /// 添加事件及回调
    /// </summary>
    /// <param name="type">事件枚举</param>
    /// <param name="handle">回调</param>
    /// <param name="isUseOnce"></param>
    public static void AddTypeEvent<T>(MessageHandle<T> handle, bool isUseOnce = false)
    {
        GetEventDispatcher<T>(isUseOnce).m_CallBack += handle;
    }

    /// <summary>
    /// 移除某类事件的一个回调
    /// </summary>
    /// <param name="type"></param>
    /// <param name="handle"></param>
    public static void RemoveTypeEvent<T>(MessageHandle<T> handle, bool isUseOnce = false)
    {
        GetEventDispatcher<T>(isUseOnce).m_CallBack -= handle;
    }

    /// <summary>
    /// 移除某类事件
    /// </summary>
    /// <param name="type"></param>
    public static void RemoveTypeEvent<T>(bool isUseOnce = false)
    {
        GetEventDispatcher<T>(isUseOnce).m_CallBack = null;
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    public static void DispatchTypeEvent<T>(SyncSession session, T e)
    {
        GetEventDispatcher<T>(false).Call(session, e);

        //只调用一次的调用后就清除
        GetEventDispatcher<T>(true).Call(session, e);
        GetEventDispatcher<T>(true).m_CallBack = null;
    }

    static EventDispatcher<T> GetEventDispatcher<T>(bool isOnce)
    {
        Type type = typeof(T);

        if (isOnce)
        {
            if (mTypeUseOnceEventDic.ContainsKey(type))
            {
                return (EventDispatcher<T>)mTypeUseOnceEventDic[type];
            }
            else
            {
                EventDispatcher<T> temp = new EventDispatcher<T>();
                mTypeUseOnceEventDic.Add(type, temp);
                return temp;
            }
        }
        else
        {
            if (mTypeEventDic.ContainsKey(type))
            {
                return (EventDispatcher<T>)mTypeEventDic[type];
            }
            else
            {
                EventDispatcher<T> temp = new EventDispatcher<T>();
                mTypeEventDic.Add(type, temp);
                return temp;
            }
        }
    }

    abstract class EventDispatcher { }

    class EventDispatcher<T> : EventDispatcher
    {
        public MessageHandle<T> m_CallBack;

        public void Call(SyncSession session, T e)
        {
            if (m_CallBack != null)
            {
                try
                {
                    m_CallBack(session, e);
                }
                catch (Exception exc)
                {
                    Debug.LogError(exc.ToString());
                }
            }
        }
    }

    #endregion
}
