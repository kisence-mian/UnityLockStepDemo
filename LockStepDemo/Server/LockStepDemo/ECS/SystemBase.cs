using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBase
{
    public WorldBase m_world;
    // Use this for initialization
    public virtual void Init ()
    {
		
	}

    public virtual void BeforeUpdate(int deltaTime)
    {

    }

    public virtual void BeforeFixedUpdate(int deltaTime)
    {

    }

    // Update is called once per frame
    public virtual void Update (int deltaTime)
    {
		
	}

    public virtual void FixedUpdate(int deltaTime)
    {

    }

    public virtual void LateUpdate(int deltaTime)
    {

    }

    public virtual void LateFixedUpdate(int deltaTime)
    {

    }
}
