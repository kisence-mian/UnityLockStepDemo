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
            DispatchEvent(life, life);

            return life;
        }

        set
        {
            int oldValue = life;

            life = value;

            if (life > maxLife)
            {
                life = maxLife;
            }

            DispatchEvent(oldValue, life);
        }
    }

    void DispatchEvent(int oldValue, int newValue)
    {
        if (newValue > oldValue)
        {
            Entity.World.eventSystem.DispatchEvent(GameUtils.GetEventKey(Entity.ID, CharacterEventType.Recover), Entity);
        }
    }
}
