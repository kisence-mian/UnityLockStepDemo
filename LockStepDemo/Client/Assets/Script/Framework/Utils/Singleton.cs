using System;

public abstract class Singleton<T> where T: Singleton<T>, new()
{
    private static T mInstance ;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new T();
                mInstance.Initialize();
            }
            return mInstance;
        }
    }

    protected virtual void Initialize() { }
}