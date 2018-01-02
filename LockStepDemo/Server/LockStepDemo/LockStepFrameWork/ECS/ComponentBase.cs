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

    public WorldBase World
    {
        get
        {
            if(world==null && entity != null)
            {
                world = entity.World;
            }
            return world;
        }

        set
        {
            world = value;
        }
    }

    private WorldBase world;
  

    public virtual void Init()
    {


    }

    public virtual int ToHash()
    {
        return 0;
    }

   
}
