using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ECSEventHandle(EntityBase entity);

public class ECSEvent
{
    private Dictionary<string, ECSEventHandle> m_EventDict = new Dictionary<string, ECSEventHandle>();

    public  void AddListener(string key, ECSEventHandle handle)
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

    public void RemoveListener(string key, ECSEventHandle handle)
    {
        if (m_EventDict.ContainsKey(key))
        {
            m_EventDict[key] -= handle;
        }
    }

    public void DispatchEvent(string key, EntityBase entity)
    {
        if (m_EventDict.ContainsKey(key))
        {
            m_EventDict[key](entity);
        }
    }
}
