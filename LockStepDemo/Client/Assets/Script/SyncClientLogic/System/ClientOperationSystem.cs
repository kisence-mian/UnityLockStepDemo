using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClientOperationSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(SelfComponent),
            typeof(PlayerComponent),
        };
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            if(!list[i].GetExistComp<OperationWindowComponent>())
            {
                PlayerComponent pc = list[i].GetComp<PlayerComponent>();
                //OperationWindow ui = UIManager.OpenUIWindow<OperationWindow>();
                //ui.m_playerComp = pc;
                //ui.ElementInit();

                //OperationWindowComponent oc = new OperationWindowComponent();
                //oc.ui = ui;

                //list[i].AddComp(oc);
            }
        }
    }
}
