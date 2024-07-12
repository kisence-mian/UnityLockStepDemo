using System;
using UnityEngine;

public class MonoSingleton<T> :MonoBehaviour where T : MonoSingleton<T>
{
    private static T mInstance ;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject obj = new GameObject("["+typeof(T).Name+"]");
                mInstance = obj.AddComponent<T>();
                mInstance. Initialize();
            }

            return mInstance;
        }
    }

    protected virtual void Initialize() { }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        mInstance = null;
    }
}