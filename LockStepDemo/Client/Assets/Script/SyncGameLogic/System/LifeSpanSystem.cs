using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LifeSpanSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(LifeSpanComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            LifeSpanComponent lsc = list[i].GetComp<LifeSpanComponent>();
            lsc.lifeTime -= deltaTime;

            if(lsc.lifeTime < 0)
            {
                m_world.ClientDestroyEntity(list[i].ID);
            }
        }
    }
}
