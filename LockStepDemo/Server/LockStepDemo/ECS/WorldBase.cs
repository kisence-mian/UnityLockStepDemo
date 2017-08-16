using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase
{
    bool m_isStart = false;
    public bool IsStart
    {
        get
        {
            return m_isStart;
        }

        set
        {
            m_isStart = value;
        }
    }

    int m_frameCount = 0;
    public int FrameCount
    {
        get
        {
            return m_frameCount;
        }

        set
        {
            m_frameCount = value;
        }
    }

    public List<SystemBase> m_systemList = new List<SystemBase>();                       //世界里所有的System集合
    public Dictionary<int, EntityBase> m_entityDict = new Dictionary<int, EntityBase>(); //世界里所有的entity集合
    public List<EntityBase> m_entityList = new List<EntityBase>();                       //世界里所有的entity列表

    public Dictionary<string, SingletonComponent> m_singleCompDict = new Dictionary<string, SingletonComponent>(); //所有的单例组件集合

    Stack<EntityBase> m_entitiesPool = new Stack<EntityBase>();  //TODO: 实体对象池

    public event EntityChangedCallBack OnEntityCreated;
    public event EntityChangedCallBack OnEntityWillBeDestroyed;
    public event EntityChangedCallBack OnEntityDestroyed;

    public event EntityComponentChangedCallBack OnEntityComponentAdded;
    public event EntityComponentChangedCallBack OnEntityComponentRemoved;
    public event EntityComponentReplaceCallBack OnEntityComponentChange;

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
            if (isView)
            {
                types = GetViewSystemTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    ViewSystemBase tmp = (ViewSystemBase)types[i].Assembly.CreateInstance(types[i].FullName);
                    m_systemList.Add(tmp);
                    tmp.m_world = this;
                    tmp.Init();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("WorldBase Init Exception:" + e.ToString());
        }

    }

    #endregion

    #region Update

    public int IntervalTime
    {
        get
        {
            return m_intervalTime;
        }

        set
        {
            m_intervalTime = value;
        }
    }

    int m_intervalTime = 200;

    /// <summary>
    /// 服务器不执行Loop
    /// </summary>
    public void Loop(int deltaTime)
    {
        if(IsStart)
        {
            BeforeUpdate(deltaTime);
            Update(deltaTime);
            LateUpdate(deltaTime);
        }
    }

    public void FixedLoop(int deltaTime)
    {
        if (IsStart)
        {
            FrameCount++;

            BeforeFixedUpdate(deltaTime);
            FixedUpdate(deltaTime);
            LateFixedUpdate(deltaTime);
        }
    }

    void BeforeUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].BeforeUpdate(deltaTime);
        }
    }

    void BeforeFixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].BeforeFixedUpdate(deltaTime);
        }
    }

    // Update is called once per frame
    void Update(int deltaTime)
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
        if (m_entityDict.ContainsKey(ID))
        {
            throw new Exception("CreateEntity Exception: Entity ID has exist ! ->" + ID + "<-");
        }

        EntityBase entity = new EntityBase();
        entity.ID = ID;

        entity.World = this;

        m_entityList.Add(entity);
        m_entityDict.Add(ID, entity);

        entity.OnComponentAdded += DispatchEntityComponentAdded;
        entity.OnComponentRemoved += DispatchEntityComponentRemoved;
        entity.OnComponentReplaced += DispatchEntityComponentChange;

        if (OnEntityCreated != null)
        {
            OnEntityCreated(entity);
        }

        return entity;
    }

    public bool GetEntityIsExist(int ID)
    {
        return m_entityDict.ContainsKey(ID);
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

        entity.OnComponentAdded -= DispatchEntityComponentAdded;
        entity.OnComponentRemoved -= DispatchEntityComponentRemoved;
        entity.OnComponentReplaced -= DispatchEntityComponentChange;

        if (OnEntityDestroyed != null)
        {
            OnEntityDestroyed(entity);
        }
    }

    public List<EntityBase> GetEntiyList(string[] compNames)
    {
        List<EntityBase> tupleList = new List<EntityBase>();
        for (int i = 0; i < m_entityList.Count; i++)
        {
            if (GetAllExistComp(compNames, m_entityList[i]))
            {
                tupleList.Add(m_entityList[i]);
            }
        }

        return tupleList;
    }

    public bool GetAllExistComp(string[] compNames, EntityBase entity)
    {
        for (int i = 0; i < compNames.Length; i++)
        {
            if (!entity.GetExistComp(compNames[i]))
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region 单例组件

    public T GetSingletonComp<T>()  where T : SingletonComponent, new()
    {
        string key = typeof(T).Name;

        SingletonComponent comp = null;

        if (m_singleCompDict.ContainsKey(key))
        {
            comp = m_singleCompDict[key];
        }
        else
        {
            comp = new T();
            m_singleCompDict.Add(key, comp);
        }

        return (T)comp;
    }

    public SingletonComponent GetSingletonComp(string compName)
    {
        SingletonComponent comp = null;

        if (m_singleCompDict.ContainsKey(compName))
        {
            comp = m_singleCompDict[compName];
        }
        else
        {
            Type compType = Type.GetType(compName);
            
            comp = (SingletonComponent)compType.Assembly.CreateInstance(compType.FullName);
            m_singleCompDict.Add(compName, comp);
        }

        return comp;
    }

    public void ChangeSingletonComp<T>(T comp) where T: SingletonComponent,new()
    {
        string compName = typeof(T).Name;
        ChangeSingleComp(compName, comp);
    }

    public void ChangeSingleComp(string compName,SingletonComponent comp)
    {
        if (m_singleCompDict.ContainsKey(compName))
        {
            m_singleCompDict[compName] = comp;
        }
        else
        {
            m_singleCompDict.Add(compName, comp);
        }
    }

    #endregion

    #region 事件派发

    void DispatchEntityComponentAdded(EntityBase entity, string compName, ComponentBase component)
    {
        if(OnEntityComponentAdded != null)
        {
            OnEntityComponentAdded(entity,compName,component);
        }
    }

    void DispatchEntityComponentRemoved(EntityBase entity, string compName, ComponentBase component)
    {
        if (OnEntityComponentRemoved != null)
        {
            OnEntityComponentRemoved(entity, compName, component);
        }
    }

    void DispatchEntityComponentChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
    {
        if (OnEntityComponentChange != null)
        {
            OnEntityComponentChange(entity, compName, previousComponent, newComponent);
        }
    }

    public delegate void EntityChangedCallBack(EntityBase entity);

    #endregion
}
