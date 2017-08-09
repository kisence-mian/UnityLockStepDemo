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
    // Use this for initialization
    public virtual void Init ()
    {
		
	}

    public virtual void Dispose()
    {

    }

    public virtual Type[] GetFilter()
    {
        return new Type[0];
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

    public virtual void OnEntityCompReplace(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent)
    {

    }

    #endregion

    #region 继承方法

    public bool GetAllExistComp(string[] compNames,EntityBase entity)
    {
        for (int i = 0; i < compNames.Length; i++)
        {
            if(!entity.GetExistComp(compNames[i]))
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

    protected void AddEntityCreaterLisnter()
    {
        m_world.OnEntityCreated += ReceviceEntityCreate;
    }

    protected void AddEntityDestroyLisnter()
    {
        m_world.OnEntityDestroyed += ReceviceEntityDestroy;
    }

    void ReceviceEntityCreate(EntityBase entity)
    {
        if(GetAllExistComp(Filter, entity))
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