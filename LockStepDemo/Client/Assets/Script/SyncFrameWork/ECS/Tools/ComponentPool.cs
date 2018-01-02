using FastCollections;
using UnityEngine;

public class ComponentPool
{
    private FastStack<ComponentBase>[] poolList;

    public ComponentPool(int componentCount, WorldBase world)
    {
        poolList = new FastStack<ComponentBase>[componentCount];
    }

    public  T GetObject<T>(int index) where T: ComponentBase,new()
    {
        return new T();
        //Debug.Log("GetObject " + index);

        T obj = null;
        FastStack<ComponentBase> stack = poolList[index];

       if(stack == null)
        {
            stack = new FastStack<ComponentBase>();
            poolList[index] = stack;
        }
        if (stack.Count > 0)
        {
            obj = (T)stack.Pop();
            //stack.RemoveAt(0);
        }
        else
        {
            obj = new T();
        }

        return obj;
    }

    public  void PutObject(int index, ComponentBase obj)
    {
        return;

        IRelease heapObj = obj as IRelease;

        if (heapObj != null)
        {
            heapObj.Release();
        }
        FastStack<ComponentBase> stack = poolList[index];
        if (stack == null)
        {
            stack = new FastStack<ComponentBase>();
            poolList[index] = stack;
        }
        stack.Add(obj);
    }
}

public interface IRelease
{
    void Release();
}

