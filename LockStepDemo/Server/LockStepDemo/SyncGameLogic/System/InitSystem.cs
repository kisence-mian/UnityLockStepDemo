using DeJson;
using LockStepDemo;
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

        InitElementCreatePoint();
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
        ConnectionComponent connectComp = entity.GetComp<ConnectionComponent>();
        PlayerComponent playerComp = null;

        if (!entity.GetExistComp<PlayerComponent>())
        {
            playerComp = new PlayerComponent();
            entity.AddComp(playerComp);
        }
        else
        {
            playerComp = entity.GetComp<PlayerComponent>();
        }

        //将角色ID传入游戏
        playerComp.characterID = connectComp.m_session.player.characterID;
        playerComp.nickName = connectComp.m_session.player.playerID;

        ElementData e1 = new ElementData();
        e1.id = 100;
        e1.num = 10;
        playerComp.elementData.Add(e1);

        ElementData e2 = new ElementData();
        e2.id = 101;
        e2.num = 10;
        playerComp.elementData.Add(e2);

        ElementData e3 = new ElementData();
        e3.id = 102;
        e3.num = 10;
        playerComp.elementData.Add(e3);

        ElementData e4 = new ElementData();
        e4.id = 103;
        e4.num = 00;
        playerComp.elementData.Add(e4);

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
            c.m_assetName = playerComp.CharacterData.m_ModelID;
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<MoveComponent>())
        {
            MoveComponent c = new MoveComponent();
            c.pos.FromVector(new Vector3(15, 0, 0));

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

        if (!entity.GetExistComp<CampComponent>())
        {
            CampComponent c = new CampComponent();
            c.creater = entity.ID;
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<MoveComponent>())
        {
            MoveComponent c = new MoveComponent();
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
            c.maxLife = playerComp.CharacterData.m_hp;
            c.life = playerComp.CharacterData.m_hp;
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<BlowFlyComponent>())
        {
            BlowFlyComponent c = new BlowFlyComponent();
            entity.AddComp(c);
        }

        //预测一个输入
        //TODO 放在框架中
        if (entity.GetExistComp<ConnectionComponent>())
        {
            ConnectionComponent cc = entity.GetComp<ConnectionComponent>();
            cc.m_lastInputCache = new CommandComponent();
            cc.m_defaultInput   = new CommandComponent();
        }

        GameTimeComponent gtc = m_world.GetSingletonComp<GameTimeComponent>();
        gtc.GameTime = 100 * 1000;
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

            m_world.CreateEntityImmediately(cc, sc, bc);

            Debug.Log("Create map");
        }
    }

    void InitElementCreatePoint()
    {
        string content = FileTool.ReadStringByFile(Environment.CurrentDirectory + "/Map/elementCreatePointData.txt");
        string[] contentArray = content.Split('\n');

        for (int i = 0; i < contentArray.Length; i++)
        {
            if (contentArray[i] != "")
            {
                ItemCreatePointComponent tmp = deserializer.Deserialize<ItemCreatePointComponent>(contentArray[i]);
                CollisionComponent cc = new CollisionComponent();
                cc.area.position = tmp.pos.ToVector();
                cc.area.areaType = AreaType.Circle;
                cc.area.radius = 1;

                m_world.CreateEntity(tmp,cc);
            }
        }
    }
}
