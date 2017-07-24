using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase
{
    List<SystemBase> m_systemList = new List<SystemBase>();
    Dictionary<int,EntityBase> m_entityDict = new Dictionary<int,EntityBase>();

    public void Init()
    {

    }

	// Update is called once per frame
	void Update (float time)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].Update(time);
        }
	}

    void LateUpdate(float time)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].LateUpdate(time);
        }
    }

    void FixedUpdate(float time)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].Update(time);
        }
    }

    void LateFixedUpdate(float time)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].LateFixedUpdate(time);
        }
    }
}
