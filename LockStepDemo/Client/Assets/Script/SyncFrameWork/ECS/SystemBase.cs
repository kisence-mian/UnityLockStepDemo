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

    /// <summary>
    /// 只在回滚时调用
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void OnlyCallByRecalc(int frame,int deltaTime) { }
   
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

    /// <summary>
    /// 重新演算时不执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void NoRecalcBeforeFixedUpdate(int deltaTime) { }

    public virtual void BeforeFixedUpdate(int deltaTime) { }

    public virtual void FixedUpdate(int deltaTime) { }

    public virtual void LateFixedUpdate(int deltaTime) { }

    /// <summary>
    /// 重新演算时不执行
    /// </summary>
    public virtual void NoRecalcLateFixedUpdate(int deltaTime) { }

    /// <summary>
    /// 帧的最后执行
    /// </summary>
    public virtual void EndFrame(int deltaTime) { }

    /// <summary>
    /// 重计算，只在重计算时调用
    /// </summary>
    public virtual void Recalc() { }

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

    //List<EntityBase> m_tupleList = new List<EntityBase>();
    private string name;
    public List<EntityBase> GetEntityList()
    {
        //m_tupleList.Clear();
        //for (int i = 0; i < m_world.m_entityList.Count; i++)
        //{
        //    if (GetAllExistComp(Filter, m_world.m_entityList[i]))
        //    {
        //        m_tupleList.Add(m_world.m_entityList[i]);
        //    }
        //}
        if (string.IsNullOrEmpty(name))
        {
            name = GetType().FullName;
        }
       return m_world.group.GetEntityByGroupName(name);
        //return m_tupleList;
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
        m_world.OnEntityCreated += ReceviceEntityCreate;
    }

    protected void AddEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed += ReceviceEntityDestroy;
    }

    protected void AddEntityWillBeDestroyLisnter()
    {
        m_world.OnEntityWillBeDestroyed += ReceviceEntityWillBeDestroy;
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
        m_world.OnEntityCreated -= ReceviceEntityCreate;
    }

    protected void RemoveEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed -= ReceviceEntityDestroy;
    }

    protected void RemoveEntityWillBeDestroyLisnter()
    {
        m_world.OnEntityWillBeDestroyed += ReceviceEntityWillBeDestroy;
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

    void ReceviceEntityCreate(EntityBase entity)
    {
        //if (GetAllExistComp(Filter, entity))
        {
            OnEntityCreate(entity);
        }
    }

    void ReceviceEntityDestroy(EntityBase entity)
    {
        //if (GetAllExistComp(Filter, entity))
        {
            OnEntityDestroy(entity);
        }
    }

    void ReceviceEntityWillBeDestroy(EntityBase entity)
    {
        //if (GetAllExistComp(Filter, entity))
        {
            OnEntityWillBeDestroy(entity);
        }
    }

    #endregion
}