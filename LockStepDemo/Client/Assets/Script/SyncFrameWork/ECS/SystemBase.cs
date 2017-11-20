using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBase
{
    public WorldBase m_world;

    #region 私有属性

   string[] m_filter;
  public  string[] Filter
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

    private string name;
    public string Name
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                name = GetType().FullName;
            }

            return name;
        }

        set
        {
            name = value;
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

    public virtual void OnGameStart()
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

    #region Update 客户端以刷新频率执行

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void BeforeUpdate(int deltaTime) { }

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void Update(int deltaTime) { }

    /// <summary>
    /// 服务器不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void LateUpdate(int deltaTime) { }

    #endregion

    #region FixUpdate 前后端以同样频率执行

    public virtual void BeforeFixedUpdate(int deltaTime) { }

    public virtual void FixedUpdate(int deltaTime) { }

    public virtual void LateFixedUpdate(int deltaTime) { }

    #endregion

    #region 特殊接口

    /// <summary>
    /// 帧的最后执行
    /// </summary>
    public virtual void EndFrame(int deltaTime) { }

    /// <summary>
    /// 在游戏暂停时执行
    /// </summary>
    public virtual void RunByPause()
    {

    }

    /// <summary>
    /// 重计算，只限给SyncSystem使用
    /// </summary>
    public virtual void Recalc() { }

    #endregion

    #endregion

    #region 事件回调

    public virtual void OnEntityCreate(EntityBase entity)
    {

    }

    public virtual void OnEntityDestroy(EntityBase entity)
    {

    }

    public virtual void OnEntityWillBeDestroy(EntityBase entity)
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

    public List<EntityBase> GetEntityList()
    {
       return m_world.group.GetEntityByGroupName(Name);
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

    #region 事件监听
    protected void AddEntityCreaterLisnter()
    {
        m_world.OnEntityCreated += OnEntityCreate;
    }

    protected void AddEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed += OnEntityDestroy;
    }

    protected void AddEntityWillBeDestroyLisnter()
    {
        m_world.OnEntityWillBeDestroyed += OnEntityWillBeDestroy;
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

    protected void RemoveEntityCreaterLisnter()
    {
        m_world.OnEntityCreated -= OnEntityCreate;
    }

    protected void RemoveEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed -= OnEntityDestroy;
    }

    protected void RemoveEntityWillBeDestroyLisnter()
    {
        m_world.OnEntityWillBeDestroyed += OnEntityWillBeDestroy;
    }

    protected void RemoveEntityCompAddLisenter()
    {
        m_world.OnEntityComponentAdded -= OnEntityCompAdd;
    }

    protected void RemoveEntityCompRemoveLisenter()
    {
        m_world.OnEntityComponentRemoved -= OnEntityCompRemove;
    }

    protected void RemoveEntityCompChangeLisenter()
    {
        m_world.OnEntityComponentChange -= OnEntityCompChange;
    }


    #endregion

    #endregion
}