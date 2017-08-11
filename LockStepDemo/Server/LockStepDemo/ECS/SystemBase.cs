using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBase
{
    public WorldBase m_world;

    #region 私有属性

    string[] m_filter;
    string[] Filter
    {
        get
        {
            if (m_filter == null)
            {
                Type[] types = GetFilter();
                m_filter = new string[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    m_filter[i] = types[i].Name;
                }
            }

            return m_filter;
        }
    }

    #endregion

    #region 重载方法

    #region 生命周期
    // Use this for initialization
    public virtual void Init()
    {

    }

    public virtual void Dispose()
    {

    }

    #endregion

    #region 过滤器

    public virtual Type[] GetFilter()
    {
        return new Type[0];
    }

    #endregion

    #region Update

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void BeforeUpdate(int deltaTime)
    {

    }

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void Update(int deltaTime)
    {

    }

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void LateUpdate(int deltaTime)
    {

    }

    public virtual void BeforeFixedUpdate(int deltaTime)
    {

    }

    public virtual void FixedUpdate(int deltaTime)
    {

    }

    public virtual void LateFixedUpdate(int deltaTime)
    {

    }

    public virtual void Recalculation(int deltaTime)
    {

    }

    #endregion

    #region 事件回调

    public virtual void OnEntityCreate(EntityBase entity)
    {

    }

    public virtual void OnEntityDestroy(EntityBase entity)
    {

    }

    public virtual void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
    {

    }

    public virtual void OnEntityCompRemove(EntityBase entity, string compName, ComponentBase component)
    {

    }

    public virtual void OnEntityCompChange(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
    {

    }
    #endregion

    #endregion

    #region 继承方法

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

    List<EntityBase> m_tupleList = new List<EntityBase>();
    public List<EntityBase> GetEntityList()
    {
        m_tupleList.Clear();
        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            if (GetAllExistComp(Filter, m_world.m_entityList[i]))
            {
                m_tupleList.Add(m_world.m_entityList[i]);
            }
        }

        return m_tupleList;
    }

    public List<EntityBase> GetEntityList(string[] filter)
    {
        List<EntityBase> tupleList = new List<EntityBase>();
        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            if (GetAllExistComp(filter, m_world.m_entityList[i]))
            {
                tupleList.Add(m_world.m_entityList[i]);
            }
        }

        return tupleList;
    }

    protected void AddEntityCreaterLisnter()
    {
        m_world.OnEntityCreated += ReceviceEntityCreate;
    }

    protected void AddEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed += ReceviceEntityDestroy;
    }

    protected void AddEntityCompAddLisenter()
    {
        m_world.OnEntityComponentAdded += OnEntityCompAdd;
    }

    protected void AddEntityCompRemoveLisenter()
    {
        m_world.OnEntityComponentRemoved += OnEntityCompRemove;
    }

    protected void AddEntityCompChangeLisenter()
    {
        m_world.OnEntityComponentChange += OnEntityCompChange;
    }

    void ReceviceEntityCreate(EntityBase entity)
    {
        if (GetAllExistComp(Filter, entity))
        {
            OnEntityCreate(entity);
        }
    }

    void ReceviceEntityDestroy(EntityBase entity)
    {
        if (GetAllExistComp(Filter, entity))
        {
            OnEntityDestroy(entity);
        }
    }

    #endregion
}