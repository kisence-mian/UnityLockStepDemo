using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase
{
    public List<SystemBase> m_systemList = new List<SystemBase>();                     //世界里所有的System集合
    public List<ViewSystemBase> m_viewSystemList = new List<ViewSystemBase>();         //世界里所有的View System集合
    public Dictionary<int,EntityBase> m_entityDict = new Dictionary<int,EntityBase>(); //世界里所有的entity集合
    public List<EntityBase> m_entityList = new List<EntityBase>();                     //世界里所有的entity列表

    Stack<EntityBase> m_entitiesPool = new Stack<EntityBase>();  //TODO: 实体对象池

    public event EntityChangedCallBack OnEntityCreated;
    public event EntityChangedCallBack OnEntityWillBeDestroyed;
    public event EntityChangedCallBack OnEntityDestroyed;

    public event EntityComponentChangedCallBack OnEntityComponentAdded;
    public event EntityComponentChangedCallBack OnEntityComponentRemoved;
    public event EntityComponentReplaceCallBack OnEntityComponentReplaced;

    #region 重载方法
    public virtual Type[] GetSystemTypes()
    {
        return new Type[0];
    }

    public virtual Type[] GetViewSystemTypes()
    {
        return new Type[0];
    }

    #endregion

    #region 初始化

    public void Init(bool isView)
    {
        try
        {
            Type[] types = GetSystemTypes();

            for (int i = 0; i < types.Length; i++)
            {
                SystemBase tmp = (SystemBase)types[i].Assembly.CreateInstance(types[i].FullName);
                m_systemList.Add(tmp);
                tmp.m_world = this;
                tmp.Init();
            }

            //初始化ViweSystem
            if(isView)
            {
                types = GetViewSystemTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    ViewSystemBase tmp = (ViewSystemBase)types[i].Assembly.CreateInstance(types[i].FullName);
                    m_viewSystemList.Add(tmp);
                    tmp.m_world = this;
                    tmp.Init();
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("WorldBase Init Exception:" + e.ToString());
        }

    }

    #endregion

    #region Update

    void BeforeUpdate(int deltaTime)
    {
        for (int i = 0; i < m_viewSystemList.Count; i++)
        {
            m_viewSystemList[i].BeforeUpdate(deltaTime);
        }
    }

    void BeforeFixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].BeforeFixedUpdate(deltaTime);
        }
    }

    /// <summary>
    /// 服务器不执行Loop
    /// </summary>
    public void Loop(int deltaTime)
    {
        BeforeUpdate(deltaTime);
        Update(deltaTime);
        LateUpdate(deltaTime);
    }

    public void FixedLoop(int deltaTime)
    {
        BeforeFixedUpdate(deltaTime);
        FixedUpdate(deltaTime);
        LateFixedUpdate(deltaTime);
    }

    // Update is called once per frame
    void Update (int deltaTime)
    {
        for (int i = 0; i < m_viewSystemList.Count; i++)
        {
            m_viewSystemList[i].Update(deltaTime);
        }
	}

    void LateUpdate(int deltaTime)
    {
        for (int i = 0; i < m_viewSystemList.Count; i++)
        {
            m_viewSystemList[i].LateUpdate(deltaTime);
        }
    }

    void FixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].FixedUpdate(deltaTime);
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

    #region 实体相关

    public EntityBase CreateEntity(int ID)
    {
        if(m_entityDict.ContainsKey(ID))
        {
            throw new Exception("CreateEntity Exception: Entity ID has exist ! ->" + ID + "<-");
        }

        EntityBase entity = new EntityBase();
        entity.ID = ID;

        m_entityList.Add(entity);
        m_entityDict.Add(ID, entity);

        entity.OnComponentAdded += OnEntityComponentAdded;
        entity.OnComponentRemoved += OnEntityComponentRemoved;
        entity.OnComponentReplaced += OnEntityComponentReplaced;

        if (OnEntityCreated != null)
        {
            OnEntityCreated(entity);
        }

        return entity;
    }

    public EntityBase GetEntity(int ID)
    {
        if (!m_entityDict.ContainsKey(ID))
        {
            throw new Exception("GetEntity Exception: Entity ID has not exist ! ->" + ID + "<-");
        }

        return m_entityDict[ID];
    }

    public void DestroyEntity(int ID)
    {
        if (!m_entityDict.ContainsKey(ID))
        {
            throw new Exception("DestroyEntity Exception: Entity ID has not exist ! ->" + ID + "<-");
        }

        EntityBase entity = m_entityDict[ID];

        if (OnEntityWillBeDestroyed != null)
        {
            OnEntityWillBeDestroyed(entity);
        }

        m_entityList.Remove(entity);
        m_entityDict.Remove(ID);

        entity.OnComponentAdded -= OnEntityComponentAdded;
        entity.OnComponentRemoved -= OnEntityComponentRemoved;
        entity.OnComponentReplaced -= OnEntityComponentReplaced;

        if (OnEntityDestroyed != null)
        {
            OnEntityDestroyed( entity);
        }
    }

    #endregion

    #region 事件派发

    public delegate void EntityChangedCallBack(EntityBase entity);

    #endregion
}
