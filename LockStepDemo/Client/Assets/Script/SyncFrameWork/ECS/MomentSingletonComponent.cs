using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MomentSingletonComponent : SingletonComponent
{
    private int frame;
    private bool isChange;

    public int Frame
    {
        get
        {
            return frame;
        }

        set
        {
            frame = value;
        }
    }

    public bool IsChange
    {
        get
        {
            return isChange;
        }

        set
        {
            isChange = value;
        }
    }

    public abstract MomentSingletonComponent DeepCopy();
}
