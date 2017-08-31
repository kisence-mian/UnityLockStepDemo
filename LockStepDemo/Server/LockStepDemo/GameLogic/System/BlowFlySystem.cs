using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BlowFlySystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[]
        {
            typeof(BlowFlyComponent),
            typeof(MoveComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            BlowFlyLogic(list[i], deltaTime);
        }
    }

    void BlowFlyLogic(EntityBase entity, int deltaTime)
    {
        BlowFlyComponent bfc = entity.GetComp<BlowFlyComponent>();
        if (bfc.isBlow)
        {
            bfc.blowTime -= deltaTime;

            MoveComponent mc = entity.GetComp<MoveComponent>();

            float distance = bfc.BlowData.m_Distance;
            float height = bfc.BlowData.m_Height;
            float time = bfc.BlowData.m_Time;

            mc.m_velocity = (int)((distance / time) * 1000);
            mc.dir = bfc.blowDir;

            if (bfc.blowTime <= 0)
            {
                bfc.isBlow = false;
            }
        }
    }
}
