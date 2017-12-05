using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HealthBarSystem :SystemBase
{
    public override void Init()
    {
        AddEntityOptimizeDestroyLisnter();
    }

    public override void Dispose()
    {
        RemoveEntityOptimizeDestroyLisnter();
    }

    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(LifeComponent),
            typeof(PerfabComponent),
        };
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            if(!list[i].GetExistComp<HealthBarComponent>())
            {
                FightBehaveWindow ui = UIManager.OpenUIWindow<FightBehaveWindow>();
                ui.SetEntity(list[i]);

                HealthBarComponent hbc = new HealthBarComponent();
                hbc.m_ui = ui;

                list[i].AddComp(hbc);
            }
        }
    }

    public override void OnEntityOptimizeDestroy(EntityBase entity)
    {
        if(entity.GetExistComp<HealthBarComponent>())
        {
            HealthBarComponent hc = entity.GetComp<HealthBarComponent>();
            UIManager.CloseUIWindow(hc.m_ui);
        }
    }
}
