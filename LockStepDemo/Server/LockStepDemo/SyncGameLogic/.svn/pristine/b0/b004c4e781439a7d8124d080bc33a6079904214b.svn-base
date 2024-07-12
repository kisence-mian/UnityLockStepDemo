using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MomentComponentBase
{
    public int maxLife;
    public int life;

    public int ResurgenceTimer = 0;

    public bool isAlive = true;

    public override MomentComponentBase DeepCopy()
    {
        LifeComponent lc = new LifeComponent();
        lc.maxLife = maxLife;
        lc.life = Life;
        lc.ResurgenceTimer = ResurgenceTimer;

        lc.Entity = Entity;
        lc.isAlive = isAlive;

        return lc;
    }

    public int Life
    {
        get
        {
            DispatchEvent(life,life);

            return life;
        }

        set
        {
            int oldValue = life;

            life = value;

            DispatchEvent(oldValue, value);
        }
    }

    void DispatchEvent(int oldValue,int newValue)
    {
        if (newValue > 0 && isAlive == false)
        {
            isAlive = true;
            Entity.World.eventSystem.DispatchEvent(GameUtils.GetEventKey(Entity.ID, CharacterEventType.Recover), Entity);
        }

        if(newValue < 0 && isAlive == true)
        {
            isAlive = false;
            Entity.World.eventSystem.DispatchEvent(GameUtils.GetEventKey(Entity.ID, CharacterEventType.Die), Entity);
            ResurgenceTimer = 0;
        }

        if(newValue > oldValue)
        {
            Entity.World.eventSystem.DispatchEvent(GameUtils.GetEventKey(Entity.ID, CharacterEventType.Recover), Entity);
        }

        if (newValue < oldValue)
        {
            Entity.World.eventSystem.DispatchEvent(GameUtils.GetEventKey(Entity.ID, CharacterEventType.Damage), Entity);
        }
    }
}
