using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSystem : SystemBase
{
    public override void Init()
    {
        AddEntityOptimizeCreaterLisnter();
        AddEntityOptimizeDestroyLisnter();

        //InitMap();

        //InitElementCreatePoint();
    }

    public override void Dispose()
    {
        RemoveEntityOptimizeCreaterLisnter();
        RemoveEntityOptimizeDestroyLisnter();
    }

    public override void OnEntityOptimizeCreate(EntityBase entity)
    {
        //服务器这里要改成判断connection组件进来
        if(entity.GetExistComp<SelfComponent>() || entity.GetExistComp<TheirComponent>())
        {
            PlayerJoin(entity);
        }
    }

    public void PlayerJoin(EntityBase entity)
    {
        PlayerComponent playerComp = null;

        if (!entity.GetExistComp<PlayerComponent>())
        {
            playerComp = new PlayerComponent();

            playerComp.characterID = "1";
            playerComp.nickName = "Test model";

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

            entity.AddComp(playerComp);
        }

        if (!entity.GetExistComp<CommandComponent>())
        {
            CommandComponent c = new CommandComponent();
            entity.AddComp(c);
        }

        if (!entity.GetExistComp<TransfromComponent>())
        {
            TransfromComponent c = new TransfromComponent();
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

    }

    Deserializer deserializer = new Deserializer();
    public void InitMap()
    {
        List<Area> list = new List<Area>();

        string content = ResourceManager.ReadTextFile("mapData");
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

            m_world.CreateEntity("Block" + i,cc);
        }


        ////创建一个可以捡的道具
        //ItemComponent ic = new ItemComponent();

        //AssetComponent assert = new AssetComponent();
        //assert.m_assetName = "EFX_res_bolt";

        //CollisionComponent colc = new CollisionComponent();
        //colc.area.position = new Vector3(10,0.5f,0);
        //colc.area.areaType = AreaType.Circle;
        //colc.area.radius = 0.5f;

        //m_world.CreateEntity(colc, ic, assert);
    }

    void InitElementCreatePoint()
    {
        string content = ResourceManager.ReadTextFile("elementCreatePointData");
        string[] contentArray = content.Split('\n');

        for (int i = 0; i < contentArray.Length; i++)
        {
            if (contentArray[i] != "")
            {
                ItemCreatePointComponent tmp = deserializer.Deserialize<ItemCreatePointComponent>(contentArray[i]);

                m_world.CreateEntity("ElementCreatePoint" + i, tmp);
            }
        }
    }
}
