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
    public bool m_isCertainty = false;
    public bool m_isRecalc = false;
    public bool m_isLocal = false;

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

    /// <summary>
    /// 客户端实体ID都为负数
    /// </summary>
    int m_clientEntityIndex = -1;

    public int ClientEntityIndex
    {
        get
        {
            return m_clientEntityIndex;
        }

        set
        {
            m_clientEntityIndex = value;
        }
    }

    public List<SystemBase> m_systemList = new List<SystemBase>();                 //世界里所有的System列表

    public Dictionary<int, EntityBase> m_entityDict = new Dictionary<int, EntityBase>(); //世界里所有的entity集合
    public List<EntityBase> m_entityList = new List<EntityBase>();                       //世界里所有的entity列表

    public List<RecordSystemBase> m_recordList = new List<RecordSystemBase>();           //世界里所有的RecordSystem列表
    public Dictionary<string, RecordSystemBase> m_recordDict = new Dictionary<string, RecordSystemBase>(); //世界里所有的RecordSystem集合

    public Dictionary<string, SingletonComponent> m_singleCompDict = new Dictionary<string, SingletonComponent>(); //所有的单例组件集合

    Stack<EntityBase> m_entitiesPool = new Stack<EntityBase>();  //TODO: 实体对象池

    public event EntityChangedCallBack OnEntityCreated;
    public event EntityChangedCallBack OnEntityWillBeDestroyed;
    public event EntityChangedCallBack OnEntityDestroyed;

    public event EntityComponentChangedCallBack OnEntityComponentAdded;
    public event EntityComponentChangedCallBack OnEntityComponentRemoved;
    public event EntityComponentReplaceCallBack OnEntityComponentChange;

    public ECSEvent eventSystem = null;

    //游戏结束
    public bool isFinish = false;

    #region 重载方法
    public virtual Type[] GetSystemTypes()
    {
        return new Type[0];
    }

    public virtual Type[] GetRecordTypes()
    {
        return new Type[0];
    }

    public virtual Type[] GetRecordSystemTypes()
    {
        return new Type[0];
    }

    #endregion

    #region 生命周期

    public void Init(bool isView)
    {
        eventSystem = new ECSEvent(this);

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

            //初始化RecordComponent
            types = GetRecordTypes();
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

            //初始化RecordSystem
            types = GetRecordSystemTypes();
            for (int i = 0; i < types.Length; i++)
            {
                RecordSystemBase tmp = (RecordSystemBase)types[i].Assembly.CreateInstance(types[i].FullName);
                m_recordList.Add(tmp);
                tmp.m_world = this;
                tmp.Init();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("WorldBase Init Exception:" + e.ToString());
        }

    }

    public void Dispose()
    {
        for (int i = 0; i < m_entityList.Count; i++)
        {
            if (OnEntityDestroyed != null)
            {
                OnEntityDestroyed(m_entityList[i]);
            }
        }

        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].Dispose();
        }

        m_entityList.Clear();
        m_entityDict.Clear();

        m_systemList.Clear();

        m_recordList.Clear();
        m_recordDict.Clear();
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
            //LateUpdate(deltaTime);
        }
    }

    public void FixedLoop(int deltaTime)
    {
        if (IsStart)
        {
            Record(FrameCount);

            FrameCount++;

            if (SyncDebugSystem.isDebug)
            {
                string content = "Begin FixedLoop " + FrameCount + "------------\n";
                SyncDebugSystem.syncLog += content;
            }

            NoRecalcBeforeFixedUpdate(deltaTime);

            BeforeFixedUpdate(deltaTime);
            FixedUpdate(deltaTime);
            LateFixedUpdate(deltaTime);

            NoRecalcLateFixedUpdate(deltaTime);

            LazyExecuteEntityOperation();

            EndFrame(deltaTime);

            if (SyncDebugSystem.isDebug)
            {
                string content = "End FixedLoop " + FrameCount + "------------\n";
                SyncDebugSystem.syncLog += content;
            }
        }
    }

    /// <summary>
    /// 重演算接口
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Recalc(int frame,int deltaTime)
    {
        FrameCount++;

        OnlyCallByReCalc(frame,deltaTime);

        BeforeFixedUpdate(deltaTime);
        FixedUpdate(deltaTime);
        LateFixedUpdate(deltaTime);

        LazyExecuteEntityOperation();
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

    public void LateUpdate(int deltaTime)
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

    void EndFrame(int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].EndFrame(deltaTime);
        }
    }

    void OnlyCallByReCalc(int frame,int deltaTime)
    {
        for (int i = 0; i < m_systemList.Count; i++)
        {
            m_systemList[i].OnlyCallByRecalc(frame,deltaTime);
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

        FrameCount = frame;
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
        if (!m_recordDict.ContainsKey(name))
        {
            throw new Exception("GetRecordSystemBase error not find " + name);
        }

        return m_recordDict[name];
    }

    #endregion

    #region 实体相关

    List<EntityBase> createCache = new List<EntityBase>();
    List<EntityBase> destroyCache = new List<EntityBase>();

    //集中执行实体的创建删除操作
    public void LazyExecuteEntityOperation()
    {
        for (int i = 0; i < createCache.Count; i++)
        {
            AddEntity(createCache[i]);
        }
        createCache.Clear();

        for (int i = 0; i < destroyCache.Count; i++)
        {
            RemoveEntity(destroyCache[i]);
        }
        destroyCache.Clear();
    }

    public bool GetExistByCreateCache(int id)
    {
        for (int i = 0; i < createCache.Count; i++)
        {
            if(createCache[i].ID == id)
            {
                return true;
            }
        }

        return false;
    }

    public bool GetExistByDestroyCache(int id)
    {
        for (int i = 0; i < destroyCache.Count; i++)
        {
            if (destroyCache[i].ID == id)
            {
                return true;
            }
        }

        return false;
    }

    #region 创建

    public void CreateEntity(string identifier, params ComponentBase[] comps)
    {
        //状态同步本地不创建实体
        //if (m_isView && m_syncRule == SyncRule.Status)
        //{
        //    return;
        //}

        identifier = FrameCount + identifier;

        Debug.Log("identifier " + identifier);

        CreateEntity(identifier.ToHash(), comps);
    }

    /// <summary>
    /// 客户端创建的实体,不影响同步
    /// </summary>
    /// <param name="comps"></param>
    public void CreateEntityImmediately(string conetnt,params ComponentBase[] comps)
    {
        //状态同步本地不创建实体
        EntityBase entity = NewEntity(conetnt.ToHash(), comps);

        CreateEntityAndDispatch(entity);
    }

    /// <summary>
    /// 使用指定的实体ID创建实体，不建议直接使用
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public EntityBase CreateEntity(int ID, params ComponentBase[] compList)
    {
        if (m_entityDict.ContainsKey(ID))
        {
            throw new Exception("CreateEntity Exception: Entity ID has exist ! ->" + ID + "<-");
        }

        EntityBase entity = NewEntity(ID, compList);

        createCache.Add(entity);

        return entity;
    }

    EntityBase NewEntity(int ID, params ComponentBase[] compList)
    {
        EntityBase entity = new EntityBase();
        entity.ID = ID;

        entity.World = this;

        if (compList != null)
        {
            for (int i = 0; i < compList.Length; i++)
            {
                entity.AddComp(compList[i].GetType().Name, compList[i]);
            }
        }

        return entity;
    }

    void AddEntity(EntityBase entity)
    {
        if(m_isRecalc)
        {
            RecalcCreateEntity(entity);
        }
        else
        {
            CreateEntityAndDispatch(entity);
        }
    }

    void CreateEntityAndDispatch(EntityBase entity)
    {
        if (m_entityDict.ContainsKey(entity.ID))
        {
            Debug.LogError("创建实体 id 冲突！ " + entity.ID);
        }

        m_entityList.Add(entity);
        m_entityDict.Add(entity.ID, entity);

        entity.OnComponentAdded += DispatchEntityComponentAdded;
        entity.OnComponentRemoved += DispatchEntityComponentRemoved;
        entity.OnComponentReplaced += DispatchEntityComponentChange;

        if (OnEntityCreated != null)
        {
            OnEntityCreated(entity);
        }
    }

    void CreateEntityNoDispatch(EntityBase entity)
    {
        if (m_entityDict.ContainsKey(entity.ID))
        {
            Debug.LogError("CreateEntityNoDispatch 创建实体 id 冲突！ " + entity.ID);
        }

        m_entityList.Add(entity);
        m_entityDict.Add(entity.ID, entity);

        entity.OnComponentAdded += DispatchEntityComponentAdded;
        entity.OnComponentRemoved += DispatchEntityComponentRemoved;
        entity.OnComponentReplaced += DispatchEntityComponentChange;
    }

    #endregion

    #region 摧毁

    public void ClientDestroyEntity(int ID)
    {
        //if (m_isView && m_syncRule == SyncRule.Status)
        //{
        //    //状态同步下本地创建的对象才立即销毁
        //    if (ID > 0)
        //    {
        //        return;
        //    }
        //}

        DestroyEntity(ID);
    }

    public void DestroyEntity(int ID)
    {
        if (!m_entityDict.ContainsKey(ID))
        {
            throw new Exception("DestroyEntity Exception: Entity ID has not exist ! ->" + ID + "<-");
        }

        EntityBase entity = m_entityDict[ID];

        if(!destroyCache.Contains(entity))
            destroyCache.Add(entity);
    }

    void RemoveEntity(EntityBase entity)
    {
        if(m_isRecalc)
        {
            RecalcDestroyEntity(entity);
        }
        else
        {
            DestroyEntityAndDispatch(entity);
        }
    }

    void DestroyEntityAndDispatch(EntityBase entity)
    {
        if (OnEntityWillBeDestroyed != null)
        {
            OnEntityWillBeDestroyed(entity);
        }

        m_entityList.Remove(entity);
        m_entityDict.Remove(entity.ID);

        entity.OnComponentAdded -= DispatchEntityComponentAdded;
        entity.OnComponentRemoved -= DispatchEntityComponentRemoved;
        entity.OnComponentReplaced -= DispatchEntityComponentChange;

        if (OnEntityDestroyed != null)
        {
            OnEntityDestroyed(entity);
        }
    }

    void DestroyEntityNoDispatch(EntityBase entity)
    {
        if (OnEntityWillBeDestroyed != null)
        {
            OnEntityWillBeDestroyed(entity);
        }

        m_entityList.Remove(entity);
        m_entityDict.Remove(entity.ID);

        entity.OnComponentAdded -= DispatchEntityComponentAdded;
        entity.OnComponentRemoved -= DispatchEntityComponentRemoved;
        entity.OnComponentReplaced -= DispatchEntityComponentChange;
    }

    #endregion

    #region 获取对象

    public int GetEntityID(string identifier)
    {
        identifier = FrameCount + identifier;

        return identifier.ToHash();
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

    #region 回滚相关

    List<EntityBase> rollbackCreateCache = new List<EntityBase>();
    List<EntityBase> rollbackDestroyCache = new List<EntityBase>();

    /// <summary>
    /// 回滚摧毁一个实体，不派发事件，并将回滚对象存入缓存
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="compList"></param>
    /// <returns></returns>
    public EntityBase RollbackDestroyEntity(int ID, params ComponentBase[] compList)
    {
        EntityBase entity = NewEntity(ID, compList);

        CreateEntityNoDispatch(entity);

        rollbackDestroyCache.Add(entity);

        return entity;
    }

    public void RollbackCreateEntity(int ID)
    {
        EntityBase entity = GetEntity(ID);

        DestroyEntityNoDispatch(entity);

        rollbackCreateCache.Add(entity);
    }

    //重计算结束，延迟派发事件
    public void EndRecalc()
    {
        for (int i = 0; i < rollbackCreateCache.Count; i++)
        {
            DestroyEntityAndDispatch(rollbackCreateCache[i]);
        }
        rollbackCreateCache.Clear();

        for (int i = 0; i < rollbackDestroyCache.Count; i++)
        {
            CreateEntityAndDispatch(rollbackDestroyCache[i]);
        }
        rollbackDestroyCache.Clear();
    }

    void RecalcCreateEntity(EntityBase entity)
    {
        Debug.Log("重计算 创建 " + entity.ID);
        if (GetIsExistCreateRollbackCache(entity.ID))
        {
            EntityBase cache = GetCreateRollbackCache(entity.ID);
            CopyValue(entity, cache);
            CreateEntityNoDispatch(cache);
            rollbackCreateCache.Remove(cache);
        }
        else
        {
            Debug.Log("创建并派发 " + entity.ID);
            //否则，创建并派发，因为这是一个新对象
            CreateEntityAndDispatch(entity);
        }
    }

    void RecalcDestroyEntity(EntityBase entity)
    {
        Debug.Log("重计算 摧毁 " + entity.ID);
        if(GetIsExistDestroyRollbackCache(entity.ID))
        {
            EntityBase cache = GetDestroyRollbackCache(entity.ID);
            //CopyValue(entity, cache);
            DestroyEntityNoDispatch(cache);
            rollbackDestroyCache.Remove(cache);
        }
        else
        {
            Debug.Log("摧毁并派发 " + entity.ID);
            //否则，摧毁并派发，因为这是在重计算中需要被摧毁的新对象
            DestroyEntityAndDispatch(entity);
        }
    }

    void CopyValue(EntityBase from,EntityBase to)
    {
        foreach (var item in from.m_compDict)
        {
            if(item.Value is MomentComponentBase)
            {
                MomentComponentBase mc = (MomentComponentBase)item.Value;

                MomentComponentBase copy = mc.DeepCopy();
                to.ChangeComp(item.Key, copy);
            }
        }
    }

    public EntityBase GetCreateRollbackCache(int ID)
    {
        for (int i = 0; i < rollbackCreateCache.Count; i++)
        {
            if (rollbackCreateCache[i].ID == ID)
            {
                return rollbackCreateCache[i];
            }
        }

        throw new Exception("GetCreateRollbackCache not find " + ID);
    }

    public bool GetIsExistCreateRollbackCache(int ID)
    {
        for (int i = 0; i < rollbackCreateCache.Count; i++)
        {
            if (rollbackCreateCache[i].ID == ID)
            {
                return true;
            }
        }

        return false;
    }

    public EntityBase GetDestroyRollbackCache(int ID)
    {
        for (int i = 0; i < rollbackDestroyCache.Count; i++)
        {
            if (rollbackDestroyCache[i].ID == ID)
            {
                return rollbackDestroyCache[i];
            }
        }

        throw new Exception("GetDestroyRollbackCache not find " + ID);
    }

    public bool GetIsExistDestroyRollbackCache(int ID)
    {
        for (int i = 0; i < rollbackDestroyCache.Count; i++)
        {
            if (rollbackDestroyCache[i].ID == ID)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

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
            comp.Init();
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

    //void DispatchEntityCreate(EntityBase entity)
    //{
    //    if (OnEntityCreated != null)
    //    {
    //        OnEntityCreated(entity);
    //    }
    //}

    //void DispatchEntityDestroy(EntityBase entity)
    //{
    //    if (OnEntityDestroyed != null)
    //    {
    //        OnEntityDestroyed(entity);
    //    }
    //}

    //void DispatchEntityWillBeDestroyed(EntityBase entity)
    //{
    //    if (OnEntityWillBeDestroyed != null)
    //    {
    //        OnEntityWillBeDestroyed(entity);
    //    }
    //}

    void DispatchEntityComponentAdded(EntityBase entity, string compName, ComponentBase component)
    {
        if (OnEntityComponentAdded != null)
        {
            OnEntityComponentAdded(entity, compName, component);
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

