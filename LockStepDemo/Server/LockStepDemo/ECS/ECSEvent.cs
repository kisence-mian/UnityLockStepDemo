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
        if(!m_world.m_isCertainty)
        {
            if (m_EventDict.ContainsKey(key))
            {
                m_EventDict[key](entity, objs);
            }
        }
        else
        {
            if (m_certaintyEventDict.ContainsKey(key))
            {
                m_certaintyEventDict[key](entity, objs);
            }
        }
    }
}
