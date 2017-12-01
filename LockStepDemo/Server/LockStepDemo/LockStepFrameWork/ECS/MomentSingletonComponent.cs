using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MomentSingletonComponent : SingletonComponent
{
    private int frame;

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

    public abstract MomentSingletonComponent DeepCopy();
}
