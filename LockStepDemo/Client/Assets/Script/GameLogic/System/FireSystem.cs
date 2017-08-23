using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FireSystem : ViewSystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
                typeof(CDComponent),
                typeof(CommandComponent),
            };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent cc = list[i].GetComp<CommandComponent>();
            CDComponent cdc = list[i].GetComp<CDComponent>();

            if (cc.isFire)
            {
                ViewComponent vc = new ViewComponent();
                AssetComponent ac = new AssetComponent();
                ac.m_assetName = "Sphere";

                MoveComponent mc = new MoveComponent();

                mc.m_dirx = 1;
                mc.m_velocity = 1;

                m_world.CreateEntity(vc, ac, mc);
            }
        }
    }
}
