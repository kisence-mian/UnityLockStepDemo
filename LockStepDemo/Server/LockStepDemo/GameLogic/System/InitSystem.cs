using LockStepDemo.ServiceLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSystem : SystemBase
{
    public override void Init()
    {
        AddEntityCreaterLisnter();
        AddEntityDestroyLisnter();
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        //服务器这里要改成判断connection组件进来
        if (entity.GetExistComp<ConnectionComponent>())
        {
            PlayerJoin(entity);
        }
    }

    public void PlayerJoin(EntityBase entity)
    {
        Debug.Log("InitSystem PlayerJoin");

        if (!entity.GetExistComp<CommandComponent>())
        {
            CommandComponent c = new CommandComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<ViewComponent>())
        {
            ViewComponent c = new ViewComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<AssetComponent>())
        {
            AssetComponent c = new AssetComponent();
            c.m_assetName = "male_01";
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<MoveComponent>())
        {
            MoveComponent c = new MoveComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<PlayerComponent>())
        {
            PlayerComponent c = new PlayerComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<SkillStatusComponent>())
        {
            SkillStatusComponent c = new SkillStatusComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<CDComponent>())
        {
            CDComponent c = new CDComponent();
            entity.AddComp(c);
        }

        //预测一个输入
        //TODO 放在框架中
        if (entity.GetExistComp<ConnectionComponent>())
        {
            ConnectionComponent cc = entity.GetComp<ConnectionComponent>();
            cc.m_lastInputCache = new CommandComponent(); 
        }
    }
}
