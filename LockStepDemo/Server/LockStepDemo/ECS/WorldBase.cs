using System;
using System.Collections;
using System.Collections.Generic;

public class WorldBase
{
    List<SystemBase> m_systemList = new List<SystemBase>();                     //世界里所有的System集合
    Dictionary<int,EntityBase> m_entityDict = new Dictionary<int,EntityBase>(); //世界里所有的entity集合

    #region 重载方法

    protected Type[] systemArray = new Type[0];
    public virtual Type[] GetSystemTypes()
    {
        return systemArray;
    }

    #endregion

    #region 初始化

    public void Init()
    {
        try
        {
            Type[] types = GetSystemTypes();

            for (int i = 0; i < types.Length; i++)
            {
                SystemBase tmp = (SystemBase)types[i].Assembly.CreateInstance(types[i].FullName);
                m_systemList.Add(tmp);
                tmp.Init();
            }
        }
        catch(Exception e)
        {
            Debug.Log("WorldBase Init Exception:" + e.ToString());
        }

    }

    #endregion

    #region Update

    bool m_start;
    int m_currentFrame;   //当前帧数
    int m_logicFrameDelta;//逻辑帧更新时间(ms)
    int m_logicFrameAdd;  //累积时间(ms)

    public void MainLoop(float deltaTime)
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
