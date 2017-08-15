using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public abstract class ComponentBase
{
    private EntityBase entity;

    public EntityBase Entity
    {
        get
        {
            return entity;
        }

        set
        {
            entity = value;
        }
    }

    public virtual void Init()
    {

    }
}
