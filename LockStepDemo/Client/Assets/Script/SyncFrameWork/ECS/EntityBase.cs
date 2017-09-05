using System.Collections;
using System.Collections.Generic;

public class EntityBase
{
    private WorldBase world;

    private int id = 0;
    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public WorldBase World
    {
        get
        {
            return world;
        }

        set
        {
            world = value;
        }
    }

    public event EntityComponentChangedCallBack OnComponentAdded;
    public event EntityComponentChangedCallBack OnComponentRemoved;
    public event EntityComponentReplaceCallBack OnComponentReplaced;
    //event EntityReleased OnEntityReleased;

    #region 组件相关

    public Dictionary<string, ComponentBase> m_compDict = new Dictionary<string, ComponentBase>();

    public bool GetExistComp<T>()where T : ComponentBase, new()
    {
        return GetExistComp(typeof(T).Name);
    }

    public bool GetExistComp(string compName)
    {
        return m_compDict.ContainsKey(compName);
    }

    public T AddComp<T>() where T:ComponentBase,new()
    {
        T comp = new T();
        comp.Init();

        comp.Entity = this;

        string key = typeof(T).Name;

        if(m_compDict.ContainsKey(key))
        {
            throw new System.Exception("AddComp exist comp !" + key);
        }
        else
        {
            m_compDict.Add(key, comp);
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, key, comp);
            }
        }

        return comp;
    }

    public EntityBase AddComp<T>(T comp) where T : ComponentBase, new()
    {
        string key = typeof(T).Name;

        comp.Entity = this;

        if (m_compDict.ContainsKey(key))
        {
            throw new System.Exception("AddComp exist comp !" + key);
        }
        else
        {
            m_compDict.Add(key, comp);
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, key, comp);
            }
        }

        return this;
    }

    public EntityBase AddComp(string compName, ComponentBase comp)
    {
        comp.Entity = this;

        if (m_compDict.ContainsKey(compName))
        {
            throw new System.Exception("AddComp exist comp !" + compName);
        }
        else
        {
            m_compDict.Add(compName, comp);
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, compName, comp);
            }
        }

        return this;
    }

    public void RemoveComp<T>() where T : ComponentBase, new()
    {
        RemoveComp(typeof(T).Name);
    }

    public void RemoveComp(string compName)
    {
        if (!m_compDict.ContainsKey(compName))
        {
            throw new System.Exception("RemoveComp not exist comp !" + compName);
        }
        else
        {
            ComponentBase comp = m_compDict[compName];
            m_compDict.Remove(compName);
            if (OnComponentRemoved != null)
            {
                OnComponentRemoved(this, compName, comp);
            }

            comp.Entity = null;
        }
    }

    public T GetComp<T>() where T : ComponentBase, new()
    {
        return (T)GetComp(typeof(T).Name);
    }

    public ComponentBase GetComp(string compName)
    {
        if(m_compDict.ContainsKey(compName))
        {
            return m_compDict[compName];
        }
        else
        {
            throw new System.Exception("GetComp not exist comp !" + compName);
        }
    }

    public void ChangeComp(string compName,ComponentBase comp)
    {
        if (m_compDict.ContainsKey(compName))
        {
            ComponentBase oldComp = m_compDict[compName];
            oldComp.Entity = null;

            m_compDict[compName] = comp;
            comp.Entity = this;

            if (OnComponentReplaced != null)
            {
                OnComponentReplaced(this, compName, oldComp, comp);
            }
        }
        else
        {
            throw new System.Exception("ChangeComp not exist comp !" + compName);
        }
    }

    public void ChangeComp<T>(T comp) where T:ComponentBase
    {
        string key = typeof(T).Name;
        ChangeComp(key, comp);
    }

    #endregion

    #region 事件派发


    #endregion
}

public delegate void EntityComponentChangedCallBack(EntityBase entity, string compName, ComponentBase component);
public delegate void EntityComponentReplaceCallBack(EntityBase entity, string compName, ComponentBase previousComponent, ComponentBase newComponent);
