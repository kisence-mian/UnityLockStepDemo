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
        CampComponent cc = entity.GetComp<CampComponent>();

        if (ssc.m_isHit)
        {
            SkillDataGenerate skillData = ssc.m_currentSkillData.SkillInfo;

            //TODO 技能代处理
            SkillUtils.TokenUseSkill(m_world, entity.ID, skillData.m_SkillAgency, mc.pos.ToVector(), ssc.skillDir.ToVector());

            //获取伤害列表
            List<EntityBase> damageList = GetSkillDamageList(entity, skillData);

            //Debug.Log("damageList " + damageList.Count);

            //创建飞行物
            CreateFlyObject(skillData, entity);

            //Debug.Log("SkillLogic hit " + entity.ID + " createrid " + cc.creater + " damageList.Count " + damageList.Count);

            for (int i = 0; i < damageList.Count; i++)
            {
                //伤害处理
                Damage(entity, damageList[i], skillData);

                //击飞处理
                BlowFly(entity, damageList[i], skillData);

                //伤害Buff处理
                DamageBuff(entity, damageList[i], skillData);
            }

            //TODO 恢复
        }
    }

    #region 飞行物

    void CreateFlyObject(SkillDataGenerate skillData, EntityBase skiller)
    {
        CampComponent campComp = skiller.GetComp<CampComponent>();

        //Debug.Log("CreateFlyObject " + skiller.ID + "  " + campComp.creater);

        if (skillData.m_FlyObjectName.Length != 0)
        {
            List<CreatPostionInfo> poss = GetCreatePostionInfos(skillData, skiller, skillData.m_FlyObjectName.Length);

            for (int i = 0; i < poss.Count; i++)
            {
                FlyDataGenerate flyData = DataGenerateManager<FlyDataGenerate>.GetData(skillData.m_FlyObjectName[i]);

                TransfromComponent tc = new TransfromComponent();
                tc.pos.FromVector(poss[i].m_pos);
                tc.dir.FromVector(poss[i].m_dir);

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
                cp.creater = campComp.creater;

                CollisionComponent cc = new CollisionComponent();
                cc.area.areaType = AreaType.Circle;
                cc.area.radius = flyData.m_Radius;

                FlyObjectComponent fc = new FlyObjectComponent();
                fc.createrID = skiller.ID;
                fc.damage = skillData.m_FlyDamageValue;
                fc.flyObjectID = skillData.m_FlyObjectName[i];

                string identify = skiller.ID + "FlyObject" + i + poss[i].m_pos;
                m_world.CreateEntity(identify, tc,mc, ac, cp, lsc, cc, fc);
            }
        }
    }

    Area areaCache = new Area();
    List<CreatPostionInfo> GetCreatePostionInfos(SkillDataGenerate skillData, EntityBase skiller, int Length)
    {
        MoveComponent mc = skiller.GetComp<MoveComponent>();
        SkillStatusComponent ssc = skiller.GetComp<SkillStatusComponent>();

        List<CreatPostionInfo> result = new List<CreatPostionInfo>();
        result.Clear();

        if (Length == 0)
        {
            return result;
        }

        HardPointEnum l_FXCreatPoint = skillData.m_FlyCreatPoint;

        Vector3 forward = ssc.skillDir.ToVector();
        Vector3 dir = forward;
        Vector3 pos = Vector3.zero;

        //获取散射区域
        Area skillArea = SkillUtils.UpdatSkillArea(areaCache, skillData, skiller, null);

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

    #region 击飞处理

    public void BlowFly(EntityBase skiller, EntityBase hurter, SkillDataGenerate skillData)
    {
        MoveComponent amc = skiller.GetComp<MoveComponent>();
        MoveComponent bmc = hurter.GetComp<MoveComponent>();
        SkillStatusComponent assc = skiller.GetComp<SkillStatusComponent>();

        string blowFlyID = skillData.m_BlowFlyID;

        //Debug.Log("BlowFly --> skill id " + skillData.m_key + "  blowfly id " + blowFlyID + " skilltoken pos " + amc.pos.ToVector() + " ");

        if (blowFlyID != "null")
        {
            //Debug.Log("BlowFly " + hurter.ID + " skillID " + skillData.m_key);

            //击飞处理
            if (hurter.GetExistComp<BlowFlyComponent>())
            { 
                BlowFlyComponent bfc = hurter.GetComp<BlowFlyComponent>();
                if(!bfc.isBlow)
                {
                    bfc.isBlow = true;
                    bfc.blowFlyID = blowFlyID;
                    bfc.blowTime = (int)(bfc.BlowData.m_Time * 1000);
                    bfc.SetBlowFly(amc.pos.ToVector(), bmc.pos.ToVector(), assc.skillDir.ToVector());
                }
            }
        }
    }

    #endregion

    #region 技能伤害

    public void Damage(EntityBase skiller, EntityBase hurter, SkillDataGenerate skillData)
    {
        bool isCrit = false;
        bool isDisrupting = false;
        int damageNumber = DamageValueFormula(skillData, skiller, hurter, out isCrit, out isDisrupting);

        if (damageNumber == 0)
        {
            return;
        }

        CampComponent acc = skiller.GetComp<CampComponent>();
        CampComponent bcc = hurter.GetComp<CampComponent>();

        //Debug.Log("Damage == " + damageNumber + " hurter  " + hurter.ID + " acc " + acc.creater + " bcc " + bcc.creater);

        //TODO 吸血
        Absorb(damageNumber, skiller, skillData);

        //伤害处理
        SkillUtils.Damage(m_world, skiller, hurter, damageNumber);
    }

    void Absorb(int damageNumber, EntityBase character, SkillDataGenerate skillData)
    {
        //if (character.m_Property.m_HpAbsorb > 0)
        //{
        //    int AbsorbNumber = (int)((float)damageNumber * (float)character.m_Property.m_HpAbsorb / 10000f);

        //    RecoverCmd rcmd = HeapObjectPool<RecoverCmd>.GetObject();
        //    rcmd.SetData(time + SyncService.SyncAheadTime, character.m_characterID, character.m_characterID, skillID, null, AbsorbNumber, false);
        //    CommandRouteService.SendSyncCommand(rcmd);
        //}
    }

    #endregion

    #region Buff处理

    void DamageBuff(EntityBase skiller, EntityBase hurter, SkillDataGenerate skillData)
    {
        SkillUtils.AddBuff(m_world, skiller, hurter, skillData.m_HurtBuff);
    }

    #endregion

    #region 获取对象

    Area skillAreaCache = new Area();
    List<EntityBase> GetSkillDamageList(EntityBase entity, SkillDataGenerate skillData)
    {
        CampComponent acc = entity.GetComp<CampComponent>();

        List<EntityBase> result = new List<EntityBase>();
        List<EntityBase> list = GetEntityList(new string[] { "CollisionComponent", "LifeComponent", "CampComponent" });

        SkillUtils.UpdateArea(skillAreaCache, skillData.m_EffectArea, entity);

        Debug.DrawRay(skillAreaCache.position, skillAreaCache.direction,Color.red,10);

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent bcc = list[i].GetComp<CollisionComponent>();
            CampComponent bcampc   = list[i].GetComp<CampComponent>();
            LifeComponent lc       = list[i].GetComp<LifeComponent>();

            //Debug.Log("bcampc.creater " + bcampc.creater + " AreaCollideSucceed -->" + skillAreaCache.AreaCollideSucceed(bcc.area));

            if (acc.creater != bcampc.creater 
                && skillAreaCache.AreaCollideSucceed(bcc.area)
                && lc.Life > 0)
            {
                result.Add(list[i]);
            }
        }

        return result;
    }

    #endregion

    #region 伤害公式

    int DamageValueFormula(SkillDataGenerate skillData, EntityBase attacker, EntityBase hurter, out bool isCrit, out bool isDisrupting)
    {
        isCrit = false;
        isDisrupting = false;
        return skillData.m_DamageValue;
    }

    #endregion

    struct CreatPostionInfo
    {
        public Vector3 m_pos;
        public Vector3 m_dir;
    }
}
