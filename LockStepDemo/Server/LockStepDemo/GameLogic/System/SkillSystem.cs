using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SkillStatusComponent),
            typeof(MoveComponent),
            typeof(PlayerComponent),
            typeof(CampComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            SkillLogic(list[i], deltaTime);
        }
    }

    void SkillLogic(EntityBase entity, int deltaTime)
    {
        SkillStatusComponent ssc = entity.GetComp<SkillStatusComponent>();
        MoveComponent mc = entity.GetComp<MoveComponent>();

        if (ssc.m_isHit)
        {
            SkillDataGenerate skillData = ssc.m_currentSkillData.m_skillInfo;
            Debug.Log("isHit");
            //获取伤害列表

            //创建飞行物
            CreateFlyObject(skillData, entity);
        }
    }

    #region 飞行物

    void CreateFlyObject(SkillDataGenerate skillData, EntityBase skiller)
    {
        CampComponent campComp = skiller.GetComp<CampComponent>();

        if (skillData.m_FlyObjectName.Length != 0)
        {
            List<CreatPostionInfo> poss = GetCreatePostionInfos(skillData, skiller, skillData.m_FlyObjectName.Length);

            for (int i = 0; i < poss.Count; i++)
            {
                FlyDataGenerate flyData = DataGenerateManager<FlyDataGenerate>.GetData(skillData.m_FlyObjectName[i]);

                MoveComponent mc = new MoveComponent();
                mc.pos.FromVector(poss[i].m_pos);
                mc.dir.FromVector(poss[i].m_dir);
                mc.m_velocity = (int)(flyData.m_Speed * 1000);

                LifeSpanComponent lsc = new LifeSpanComponent();
                lsc.lifeTime = (int)(flyData.m_LiveTime * 1000);

                AssetComponent ac = new AssetComponent();
                ac.m_assetName = flyData.m_ModelName;

                CampComponent cp = new CampComponent();
                cp.camp = campComp.camp;

                CollisionComponent cc = new CollisionComponent();
                cc.area.areaType = AreaType.Circle;
                cc.area.radius = flyData.m_Radius;

                FlyObjectComponent fc = new FlyObjectComponent();
                fc.createrID = skiller.ID;
                fc.damage = skillData.m_DamageValue;

                m_world.CreateEntity(mc, ac, cp, lsc, cc, fc);

                Debug.Log(poss[i].m_pos.ToString());
            }
        }
    }

    Area areaCache = new Area();
    List<CreatPostionInfo> GetCreatePostionInfos(SkillDataGenerate skillData, EntityBase skiller, int Length)
    {
        MoveComponent mc = skiller.GetComp<MoveComponent>();
        PlayerComponent pc = skiller.GetComp<PlayerComponent>();

        List<CreatPostionInfo> result = new List<CreatPostionInfo>();
        result.Clear();

        if (Length == 0)
        {
            return result;
        }

        HardPointEnum l_FXCreatPoint = skillData.m_FlyCreatPoint;

        Vector3 forward = pc.faceDir.ToVector();
        Vector3 dir = forward;
        Vector3 pos = Vector3.zero;

        //获取散射区域
        Area skillArea = UpdatSkillArea(areaCache, skillData, skiller, null);

        //TODO 寻敌方法
        //CharacterBase enemy = GetRecentlyEnemy(skillArea, skiller.m_camp, false);
        //CharacterBase enemy = null;
        MoveComponent emc = null;

        if (l_FXCreatPoint != HardPointEnum.enemy)
        {
            pos = mc.pos.ToVector();
        }
        else
        {
            if (emc == null)
            {
                return result;
            }
            else
            {
                pos = emc.pos.ToVector();
            }
        }

        Vector3 leftBorder = Vector3.zero;
        Vector3 leftDir = Vector3.zero;
        Vector3 leftPos = Vector3.zero;
        float sectorStep = 0;
        float rectangleStep = 0;

        AreaDataGenerate area = DataGenerateManager<AreaDataGenerate>.GetData(skillData.m_FlyObjectArea);

        switch (area.m_SkewDirection)
        {
            case DirectionEnum.Forward: break;
            case DirectionEnum.Backward: forward *= -1; break;
            case DirectionEnum.Close: forward = (emc.pos.ToVector() - mc.pos.ToVector()).normalized; break;
            case DirectionEnum.Leave: forward = (mc.pos.ToVector() - emc.pos.ToVector()).normalized; break;
        }

        switch (area.m_Shape)
        {
            case AreaType.Circle:
                leftBorder = forward.Vector3RotateInXZ(360 * 0.5f);
                sectorStep = 360 / (Length + 1);
                break;
            case AreaType.Sector:
                leftBorder = forward.Vector3RotateInXZ(area.m_Angle * 0.5f);
                sectorStep = area.m_Angle / (Length + 1);
                break;
            case AreaType.Rectangle:
                leftDir = forward.Vector3RotateInXZ(90);
                leftPos = pos + leftDir * area.m_Width * 0.5f;
                rectangleStep = area.m_Width / (Length + 1);
                break;
        }

        for (int i = 0; i < Length; i++)
        {
            switch (area.m_Shape)
            {
                case AreaType.Circle:
                case AreaType.Sector:
                    dir = leftBorder.Vector3RotateInXZ2((i + 1) * sectorStep);
                    pos = pos + forward * area.m_SkewDistance;
                    break;
                case AreaType.Rectangle:
                    pos = leftPos - leftDir * rectangleStep * (i + 1);
                    break;
            }


            CreatPostionInfo cpi = new CreatPostionInfo();
            cpi.m_pos = pos;
            cpi.m_dir = dir;

            result.Add(cpi);
        }

        return result;
    }

    #endregion

    #region 范围拓展方法

    public static Area UpdatSkillArea(Area area, SkillDataGenerate skillData, EntityBase skiller, EntityBase aim = null)
    {
        string effectArea = skillData.m_EffectArea;
        UpdateArea(area, effectArea, skiller, aim);
        return area;
    }

    public static void UpdateArea(Area area, string areaID, EntityBase skiller, EntityBase aim = null)
    {
        MoveComponent smc = skiller.GetComp<MoveComponent>();

        AreaDataGenerate areaData = DataGenerateManager<AreaDataGenerate>.GetData(areaID);
        Vector3 dir = GetAreaDir(area, areaData, skiller, aim);

        area.areaType = areaData.m_Shape;
        area.length = areaData.m_Length;
        area.Width = areaData.m_Width;
        area.angle = areaData.m_Angle;
        area.radius = areaData.m_Radius;

        area.direction = dir.normalized;
        area.position = smc.pos.ToVector() + area.direction * areaData.m_SkewDistance;

        //Debug.Log( "skiller forward"+skiller.transform.forward);
    }

    public static Vector3 GetAreaDir(Area area, AreaDataGenerate areaData, EntityBase skiller, EntityBase aim = null)
    {
        MoveComponent smc = skiller.GetComp<MoveComponent>();
        PlayerComponent pc = skiller.GetComp<PlayerComponent>();

        Vector3 l_dir = Vector3.zero;
        if (aim == null)
        {
            switch (areaData.m_SkewDirection)
            {
                case DirectionEnum.Forward:
                    l_dir = pc.faceDir.ToVector();
                    break;
                case DirectionEnum.Backward:
                    l_dir = pc.faceDir.ToVector() * -1;
                    break;
                case DirectionEnum.Close:
                    Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为forward");
                    l_dir = pc.faceDir.ToVector();
                    break;
                case DirectionEnum.Leave:
                    Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为Backward");
                    l_dir = pc.faceDir.ToVector() * -1; break;
                default:
                    Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向");
                    break;
            }
        }

        if (aim != null)
        {
            MoveComponent amc = aim.GetComp<MoveComponent>();
            switch (areaData.m_SkewDirection)
            {
                case DirectionEnum.Forward: l_dir = pc.faceDir.ToVector(); break;
                case DirectionEnum.Backward: l_dir = pc.faceDir.ToVector() * -1; break;
                case DirectionEnum.Leave: l_dir = amc.pos.ToVector() - smc.pos.ToVector(); break;
                case DirectionEnum.Close: l_dir = smc.pos.ToVector() - amc.pos.ToVector(); break;
            }

        }
        return l_dir;
    }

    #endregion

    struct CreatPostionInfo
    {
        public Vector3 m_pos;
        public Vector3 m_dir;
    }

}

public enum DirectionEnum
{
    Forward, //施法者前方
    Backward,//施法者后方
    Leave,//受击者远离施法者方向
    Close,//受击者靠近施法者方向
}
