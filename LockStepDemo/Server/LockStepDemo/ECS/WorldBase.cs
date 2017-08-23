using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase
{
    SyncRule m_syncRule;

    public SyncRule SyncRule
    {
        get
        {
            return m_syncRule;
        }

        set
        {
            m_syncRule = value;
        }
    }

    bool m_isStart = false;
    bool m_isView = false; //是否是在客户端运行

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

    int m_entityIndex = 0;
    public int EntityIndex
    {
        get
        {
            return m_entityIndex;
        }

        set
        {
            m_entityIndex = value;
        }
    }

    public List<SystemBase> m_systemList = new List<SystemBase>();                 //世界里所有的System集合

    public Dictionary<int, EntityBase> m_entityDict = new Dictionary<int, EntityBase>(); //世界里所有的entity集合
    public List<EntityBase> m_entityList = new List<EntityBase>();                       //世界里所有的entity列表

    public List<RecordSystemBase> m_recordList = new List<RecordSystemBase>();           //世界里所有的RecordSystem列表
    public Dictionary<string, RecordSystemBase> m_recordDict = new Dictionary<string, RecordSystemBase>(); //世界里所有的RecordSystem集合

    public Dictionary<string, SingletonComponent> m_singleCompDict = new Dictionary<string, SingletonComponent>(); //所有的单例组件集合

    Stack<EntityBase> m_entitiesPool = new Stack<EntityBase>();  //TODO: 实体对象池 组件对象池

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

    public virtual Type[] GetRecordSystemTypes()
    {
        return new Type[0];
    }

    #endregion

    #region 初始化

    public void Init(bool isView)
    {
        m_isView = isView;
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

            //初始化RecordSystemBase

            types = GetRecordSystemTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = typeof(RecordSystem<>);
                type = type.MakeGenericType(types[i]);

                RecordSystemBase tmp = (RecordSystemBase)Activator.CreateInstance(type); ;
                m_recordList.Add(tmp);
                m_recordDict.Add(types[i].Name, tmp);
                tmp.m_world = this;
                tmp.Init();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("WorldBase Init Exception:" + e.ToString());
        }

    }

    #endregion

    #region Update

    /// <summary>
    /// 服务器不执行Loop
    /// </summary>
    public void Loop(int deltaTime)
    {
        if (IsStart)
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
            Record(FrameCount);

            FrameCount++;

            Debug.Log("Begin FixedLoop " + FrameCount + "------------");

            NoRecalcBeforeFixedUpdate(deltaTime);

            BeforeFixedUpdate(deltaTime);
            FixedUpdate(deltaTime);
            LateFixedUpdate(deltaTime);

            NoRecalcLateFixedUpdate(deltaTime);

            Debug.Log("End FixedLoop " + FrameCount + "------------");
        }
    }

    /// <summary>
    /// 重演算接口
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Recalc(int deltaTime)
    {
        BeforeFixedUpdate(deltaTime);
        FixedUpdate(deltaTime);
        LateFixedUpdate(deltaTime);
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

    void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].NoRecalcBeforeFixedUpdate(deltaTime);
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

    void NoRecalcLateFixedUpdate(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].NoRecalcLateFixedUpdate(deltaTime);
        }
    }
    #endregion

    #region 回滚相关 

    public void Record(int frame)
    {
        for (int i = 0; i < m_recordList.Count; i++)
        {
            m_recordList[i].Record(frame);
        }
    }

    public void RevertToFrame(int frame)
    {
        for (int i = 0; i < m_recordList.Count; i++)
        {
            m_recordList[i].RevertToFrame(frame);
        }
    }

    public void ClearBefore(int frame)
    {
        for (int i = 0; i < m_recordList.Count; i++)
        {
            m_recordList[i].ClearBefore(frame);
        }
    }

    public void ClearAfter(int frame)
    {
        for (int i = 0; i < m_recordList.Count; i++)
        {
            m_recordList[i].ClearAfter(frame);
        }
    }

    public RecordSystemBase GetRecordSystemBase(string name)
    {
        return m_recordDict[name];
    }

    #endregion

    #region 实体相关

    public void CreateEntity(params ComponentBase[] comps)
    {
        //状态同步本地不创建实体
        if (m_isView && m_syncRule == SyncRule.Status)
        {
            return;
        }

        CreateEntity(EntityIndex++, comps);
    }

    /// <summary>
    /// 使用指定的实体ID创建实体，不建议直接使用
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public EntityBase CreateEntity(int ID, params ComponentBase[] compList)
    {
        Debug.Log("CreateEntity " + ID);

        if (m_entityDict.ContainsKey(ID))
        {
            throw new Exception("CreateEntity Exception: Entity ID has exist ! ->" + ID + "<-");
        }

        EntityBase entity = new EntityBase();
        entity.ID = ID;

        entity.World = this;

        m_entityList.Add(entity);
        m_entityDict.Add(ID, entity);

        entity.OnComponentAdded += DispatchCompAdd;
        entity.OnComponentRemoved += DispatchCompRemove;
        entity.OnComponentReplaced += DispatchCompChange;

        if (compList != null)
        {
            for (int i = 0; i < compList.Length; i++)
            {
                entity.AddComp(compList[i].GetType().Name, compList[i]);
            }
        }

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

        entity.OnComponentAdded -= OnEntityComponentAdded;
        entity.OnComponentRemoved -= OnEntityComponentRemoved;
        entity.OnComponentReplaced -= OnEntityComponentChange;

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

    public T GetSingletonComp<T>() where T : SingletonComponent, new()
    {
        Type type = typeof(T);

        string key = typeof(T).Name;

        if (type.IsGenericType)
        {
            key += type.GetGenericArguments()[0].Name;
        }

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

    public void ChangeSingletonComp<T>(T comp) where T : SingletonComponent, new()
    {
        string compName = typeof(T).Name;
        ChangeSingleComp(compName, comp);
    }

    public void ChangeSingleComp(string compName, SingletonComponent comp)
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

    public delegate void EntityChangedCallBack(EntityBase entity);

    public void DispatchCompAdd(EntityBase entity, string compName, ComponentBase component)
    {
        if (OnEntityComponentAdded != null)
        {
            OnEntityComponentAdded(entity, compName, component);
        }
    }

    public void DispatchCompRemove(EntityBase entity, string compName, ComponentBase component)
    {
        if (OnEntityComponentRemoved != null)
        {
            OnEntityComponentRemoved(entity, compName, component);
        }
    }

    public void DispatchCompChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponentt)
    {
        if(OnEntityComponentChange != null)
        {
            OnEntityComponentChange(entity, compName,previousComponent,newComponentt);
        }
    }

    #endregion
}

/// <summary>
/// 同步规则
/// </summary>
public enum SyncRule
{
    /// <summary>
    /// 状态同步，所有对实体的操作都交给服务器下发,服务器可能针对每个玩家做剪枝
    /// </summary>
    Status,

    /// <summary>
    /// 帧同步，本地计算所有结果，本地了解游戏里的一切情况
    /// </summary>
    Frame,
}

