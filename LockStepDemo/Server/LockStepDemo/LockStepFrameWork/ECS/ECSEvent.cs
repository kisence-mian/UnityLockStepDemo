using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ECSEventHandle(EntityBase entity,params object[] objs);

public class ECSEvent
{
    WorldBase m_world;

    private Dictionary<string, ECSEventHandle> m_EventDict = new Dictionary<string, ECSEventHandle>();

    //只在重计算和本地计算调用
    private Dictionary<string, ECSEventHandle> m_certaintyEventDict = new Dictionary<string, ECSEventHandle>();

    List<EventCache> m_eventCache = new List<EventCache>();

    public ECSEvent(WorldBase world)
    {
        m_world = world;
    }

    public  void AddListener(string key, ECSEventHandle handle,bool certainty = false)
    {
        if (!certainty)
        {
            if (m_EventDict.ContainsKey(key))
            {

                m_EventDict[key] += handle;
            }
            else
            {
                m_EventDict.Add(key, handle);
            }
        }
        else
        {
            if (m_certaintyEventDict.ContainsKey(key))
            {

                m_certaintyEventDict[key] += handle;
            }
            else
            {
                m_certaintyEventDict.Add(key, handle);
            }
        }

    }

    public void RemoveListener(string key, ECSEventHandle handle, bool certainty = false)
    {
        if (!certainty)
        {
            if (m_EventDict.ContainsKey(key))
            {
                m_EventDict[key] -= handle;
            }
        }
        else
        {
            if (m_certaintyEventDict.ContainsKey(key))
            {
                m_certaintyEventDict[key] -= handle;
            }
        }
    }

    public void DispatchEvent(string key, EntityBase entity, params object[] objs)
    {
        if (m_EventDict.ContainsKey(key))
        {
            try
            {
                m_EventDict[key](entity, objs);
            }
            catch(Exception e)
            {
                Debug.LogError("DispatchECSEvent " + e.ToString());
            }
        }

        if (m_world.IsCertainty || m_world.IsLocal)
        {
            if (m_certaintyEventDict.ContainsKey(key))
            {
                try
                {
                    m_certaintyEventDict[key](entity, objs);
                }
                catch (Exception e)
                {
                    Debug.LogError("DispatchECSEvent isCertainty " + e.ToString());
                }
            }
        }
        else
        {
            EventCache e = new EventCache();
            e.frame = m_world.FrameCount;
            e.eventKey = key;
            e.entity = entity;
            e.objs = objs;

            m_eventCache.Add(e);
        }
    }

    //这一帧之前的计算认为全部有效
    public void DispatchCertainty(int frame)
    {
        for (int i = 0; i < m_eventCache.Count; i++)
        {
            EventCache e = m_eventCache[i];
            if (e.frame <= frame)
            {
                if (m_certaintyEventDict.ContainsKey(e.eventKey))
                {
                    m_certaintyEventDict[e.eventKey](e.entity, e.objs);
                }

                m_eventCache.RemoveAt(i);
                i--;
            }
        }
    }

    //推倒重新计算
    public void ClearCacheBefore(int frame)
    {
        for (int i = 0; i < m_eventCache.Count; i++)
        {
            EventCache e = m_eventCache[i];
            if (e.frame <= frame)
            {
                m_eventCache.RemoveAt(i);
                i--;
            }
        }
    }

    public void ClearCacheAfter(int frame)
    {
        for (int i = 0; i < m_eventCache.Count; i++)
        {
            EventCache e = m_eventCache[i];
            if (e.frame > frame)
            {
                m_eventCache.RemoveAt(i);
                i--;
            }
        }
    }

    public struct EventCache
    {
        public int frame;
        public string eventKey;
        public EntityBase entity;
        public object[] objs;
    }
}
