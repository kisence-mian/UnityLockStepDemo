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

    public override void Dispose()
    {
        if(UIManager.GetUI<OperationWindow>() != null)
        {
            UIManager.CloseUIWindow<OperationWindow>();
        }
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            if(!list[i].GetExistComp<OperationWindowComponent>())
            {
                OperationWindow ui = UIManager.OpenUIWindow<OperationWindow>();
                ui.m_entity = list[i];
                ui.ElementInit(); //初始化元素显示
                ui.InitRank();    //初始化排行榜显示

                OperationWindowComponent oc = new OperationWindowComponent();
                oc.ui = ui;

                list[i].AddComp(oc);
            }
        }
    }
}
