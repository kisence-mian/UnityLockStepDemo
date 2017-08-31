using DeJson;
using LockStepDemo;
using LockStepDemo.ServiceLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSystem : SystemBase
{
    public override void Init()
    {
        AddEntityCreaterLisnter();
        AddEntityDestroyLisnter();

        InitMap();
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        //服务器这里要改成判断connection组件进来
        if(entity.GetExistComp<ConnectionComponent>())
        {
            PlayerJoin(entity);
        }
    }

    public void PlayerJoin(EntityBase entity)
    {
        if(!entity.GetExistComp<CommandComponent>())
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
            c.m_assetName = "famale_01";
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

            DataTable data = DataManager.GetData("SkillData");
            for (int i = 0; i < data.TableIDs.Count; i++)
            {
                c.m_skillList.Add(new SkillData(data.TableIDs[i], i));
            }
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<CDComponent>())
        {
            CDComponent c = new CDComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<MoveComponent>())
        {
            MoveComponent c = new MoveComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<CampComponent>())
        {
            CampComponent c = new CampComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<CollisionComponent>())
        {
            CollisionComponent c = new CollisionComponent();
            c.area.areaType = AreaType.Circle;
            c.area.radius = 0.5f;
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<LifeComponent>())
        {
            LifeComponent c = new LifeComponent();
            c.maxLife = 100;
            c.life = 100;
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

    Deserializer deserializer = new Deserializer();
    public void InitMap()
    {
        List<Area> list = new List<Area>();

        string content = FileTool.ReadStringByFile(Environment.CurrentDirectory + "/Map/mapData.txt");
        string[] contentArray = content.Split('\n');

        for (int i = 0; i < contentArray.Length; i++)
        {
            if (contentArray[i] != "")
            {
                list.Add(deserializer.Deserialize<Area>(contentArray[i]));
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent cc = new CollisionComponent();
            cc.area = list[i];

            SyncComponent sc = new SyncComponent();

            BlockComponent bc = new BlockComponent();

            m_world.CreateEntity(cc, sc, bc);

            Debug.Log("Create map");
        }
    }
}
