using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase
{
    List<SystemBase> m_systemList = new List<SystemBase>();
    Dictionary<int,EntityBase> m_entityDict = new Dictionary<int,EntityBase>();

    public void Init()
    {
        //ApplicationManager.s_OnApplicationUpdate += ClientLoop;
    }

    #region ClientLoop

    void ClientLoop()
    {
        MainLoop(Time.deltaTime);
    }

    #endregion

    #region Update

    bool m_start;
    int m_logicFrameDelta;//逻辑帧更新时间
    int m_logicFrameAdd;  //累积时间

    void MainLoop(float deltaTime)
    {
        if (!m_start)
            return;

        int deltaTimeTmp = (int)(deltaTime * 1000);

        Loop(deltaTimeTmp);

        if (m_logicFrameAdd < m_logicFrameDelta)
        {
            m_logicFrameAdd += deltaTimeTmp;
        }
        else
        {
            while (m_logicFrameAdd > m_logicFrameDelta)
            {
                m_logicFrameAdd -= m_logicFrameDelta;

                FixedLoop(m_logicFrameDelta);//主循环
            }
        }
    }

    void Loop(int deltaTime)
    {
        Update(deltaTime);
        LateUpdate(deltaTime);
    }

    void FixedLoop(int deltaTime)
    {
        FixedUpdate(deltaTime);
        LateFixedUpdate(deltaTime);
    }

    // Update is called once per frame
    void Update (int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].Update(deltaTime);
        }
	}

    void LateUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].LateUpdate(deltaTime);
        }
    }

    void FixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].Update(deltaTime);
        }
    }

    void LateFixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].LateFixedUpdate(deltaTime);
        }
    }
    #endregion
}
