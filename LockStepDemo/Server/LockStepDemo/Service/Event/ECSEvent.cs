using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ECSEventHandle(EntityBase entity);

public class ECSEvent
{
    private static Dictionary<Enum, ECSEventHandle> m_EventDict = new Dictionary<Enum, ECSEventHandle>();

    public static void AddEvent(Enum type, ECSEventHandle handle)
    {
        if (m_EventDict.ContainsKey(type))
        {
            m_EventDict[type] += handle;
        }
        else
        {
            m_EventDict.Add(type, handle);
        }
    }

    public static void RemoveEvent(Enum type, ECSEventHandle handle)
    {
        if (m_EventDict.ContainsKey(type))
        {
            m_EventDict[type] -= handle;
        }
    }

    public static void DispatchEvent(Enum type, EntityBase entity)
    {
        if (m_EventDict.ContainsKey(type))
        {
            m_EventDict[type](entity);
        }
    }
}
