using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase
{
    private WorldBase world;

    private int id = 0;

    public string name;
    private int createFrame = 0;
    private int destroyFrame = 0;

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

    public int CreateFrame
    {
        get
        {
            return createFrame;
        }

        set
        {
            createFrame = value;
        }
    }

    public int DestroyFrame
    {
        get
        {
            return destroyFrame;
        }

        set
        {
            destroyFrame = value;
        }
    }

    public EntityBase(WorldBase world)
    {
        World = world;
        comps = new ComponentBase[world.componentType.Count()];
    }

    public event EntityComponentChangedCallBack OnComponentAdded;
    public event EntityComponentChangedCallBack OnComponentRemoved;
    public event EntityComponentReplaceCallBack OnComponentReplaced;

    #region 组件相关

    public ComponentBase[] comps = null;

    //public bool GetExistComp<T>()where T : ComponentBase, new()
    //{
    //    return GetExistComp(typeof(T).Name);
    //}

    public bool GetExistComp(string compName)
    {
      int index= world.componentType.GetComponentIndex(compName);
      return  comps[index] != null;
      //  return m_compDict.ContainsKey(compName);
    }

    public bool GetExistComp(int compIndex)
    {
        return comps[compIndex] != null;
    }

    public T AddComp<T>() where T:ComponentBase,new()
    {
        T comp = new T();
        comp.Init();
        AddComp(comp);
        return comp;
    }

    public EntityBase AddComp<T>(T comp) where T : ComponentBase, new()
    {
        string key = typeof(T).Name;
        AddComp(key, comp);
        return this;
    }

    public EntityBase AddComp(string compName, ComponentBase comp)
    {
        comp.Entity = this;
        int index = world.componentType.GetComponentIndex(compName);
        AddComp(index, comp);

        return this;
    }

    public EntityBase AddComp(int index, ComponentBase comp)
    {
        comp.Entity = this;

        if (comps[index] != null)
        {
            throw new System.Exception("AddComp exist comp !" + index);
        }
        else
        {
            comps[index] = comp;
            // m_compDict.Add(key, comp);
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, index, comp);
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
        int index = world.componentType.GetComponentIndex(compName);
        RemoveComp(index);
    }

    public void RemoveComp(int index)
    {
        if (comps[index] == null)
        {
            throw new System.Exception("RemoveComp not exist comp !" + index);
        }
        else
        {
            ComponentBase comp = comps[index];
            comps[index] = null;
            if (OnComponentRemoved != null)
            {
                OnComponentRemoved(this, index, comp);
            }

            comp.Entity = null;
            comp.World = null;
        }
    }

    public T GetComp<T>() where T : ComponentBase, new()
    {
        return (T)GetComp(typeof(T).Name);
    }
    public T GetComp<T>(int index) where T : ComponentBase, new()
    {
        T co = comps[index] as T;
        if (co == null)
            throw new System.Exception("EntityID " + ID + " GetComp not exist comp !" + typeof(T).Name);
        else
            return co;
    }

    public ComponentBase GetComp(string compName)
    {
        int index = world.componentType.GetComponentIndex(compName);
        ComponentBase co = comps[index];
        if (co == null)
            throw new System.Exception("EntityID " + ID + " GetComp not exist comp !" + compName);
        else
            return co;
    }

    public ComponentBase GetComp(int index)
    {
        ComponentBase co = comps[index];
        if (co == null)
            throw new System.Exception("EntityID " + ID + " GetComp not exist comp !" + index.ToString());
        else
            return co;
    }

    public void ChangeComp(string compName,ComponentBase comp)
    {
        int index = world.componentType.GetComponentIndex(compName);
        ChangeCompIndex(index, comp);
    }

    public void ChangeComp<T>(int index, T comp) where T : ComponentBase, new()
    {
        ChangeCompIndex(index, comp);
    }

    public void ChangeCompIndex(int index, ComponentBase comp)
    {
        //int index = world.componentType.GetComponentIndex(compName);
        ComponentBase oldComp = comps[index];
        if (oldComp == null)
            throw new System.Exception("EntityID " + ID + " GetComp not exist comp !" + comp.GetType().Name);
        else
        {
            oldComp.Entity = null;

            comps[index] = comp;
            comp.Entity = this;
            comp.World = world;

            if (OnComponentReplaced != null)
            {
                OnComponentReplaced(this, index, oldComp, comp);
            }

            world.heapComponentPool.PutObject(index, oldComp);
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

    #region Hash

    public virtual int ToHash()
    {
        int hash = id;

        foreach (var item in comps)
        {
            hash += item.ToHash();
        }

        return hash;
    }
#endregion
}

public delegate void EntityComponentChangedCallBack(EntityBase entity, int index, ComponentBase component);
public delegate void EntityComponentReplaceCallBack(EntityBase entity, int index, ComponentBase previousComponent, ComponentBase newComponent);
