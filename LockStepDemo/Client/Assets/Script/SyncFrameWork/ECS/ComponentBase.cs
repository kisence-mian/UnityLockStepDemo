using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ComponentBase
{
    private EntityBase entity;

    public static int hashCode;

    public EntityBase Entity
    {
        get
        {
            if(entity == null)
            {

            }

            return entity;
        }

        set
        {
            entity = value;
        }
    }

    public static int MomentComp = 0;

    public virtual void Init()
    {


    }

    public virtual int ToHash()
    {
        return 0;
    }

   
}
