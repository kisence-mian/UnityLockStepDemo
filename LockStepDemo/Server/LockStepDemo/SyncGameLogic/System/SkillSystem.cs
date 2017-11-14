using DeJson;
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

            //技能代处理
            SkillUtils.TokenUseSkill(m_world, entity.ID, skillData.m_SkillAgency, mc.pos.ToVector(), ssc.skillDir.ToVector());

            //获取伤害列表
            List<EntityBase> damageList = GetSkillDamageList(entity, skillData);

            //Debug.Log("damageList " + damageList.Count);

            //创建飞行物
            CreateFlyObject(skillData, entity);

            //自身buff
            SkillUtils.AddBuff(m_world, entity, entity, skillData.m_SelfBuff);

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
            Recover(entity, entity, skillData);
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
                tc.pos = poss[i].m_pos;
                tc.dir = poss[i].m_dir;

                MoveComponent mc = new MoveComponent();
                mc.pos = poss[i].m_pos;
                mc.dir = poss[i].m_dir;
                mc.m_velocity = flyData.m_Speed;

                LifeSpanComponent lsc = new LifeSpanComponent();
                lsc.lifeTime = flyData.m_LiveTime;

                AssetComponent ac = new AssetComponent();
                ac.m_assetName = flyData.m_ModelName;

                CampComponent cp = new CampComponent();
                cp.camp = campComp.camp;
                cp.creater = campComp.creater;

                CollisionComponent cc = new CollisionComponent();
                cc.area.areaType = AreaType.Circle;
                cc.area.radius = flyData.m_Radius / 1000;

                FlyObjectComponent fc = new FlyObjectComponent();
                fc.createrID = skiller.ID;
                fc.damage = skillData.m_FlyDamageValue;
                fc.flyObjectID = skillData.m_FlyObjectName[i];

                string identify = skiller.ID + "FlyObject" + i + poss[i].m_pos;
                m_world.CreateEntity(identify, tc, mc, ac, cp, lsc, cc, fc);
            }
        }
    }

    Area areaCache = new Area();
    List<CreatPostionInfo> GetCreatePostionInfos(SkillDataGenerate skillData, EntityBase skiller, int Length)
    {
        MoveComponent mc = skiller.GetComp<MoveComponent>();
        SkillStatusComponent ssc = skiller.GetComp<SkillStatusComponent>();

        List<CreatPostionInfo> result = new List<CreatPostionInfo>();
        //result.Clear();

        if (Length == 0)
        {
            return result;
        }

        HardPointEnum l_FXCreatPoint = skillData.m_FlyCreatPoint;

        SyncVector3 forward = new SyncVector3().FromVector(ssc.skillDir.ToVector().normalized);
        SyncVector3 dir = forward;
        SyncVector3 pos = mc.pos;

        SyncVector3 leftBorder = new SyncVector3();
        SyncVector3 leftDir = new SyncVector3();
        SyncVector3 leftPos = new SyncVector3();
        float sectorStep = 0;
        float rectangleStep = 0;

        AreaDataGenerate area = DataGenerateManager<AreaDataGenerate>.GetData(skillData.m_FlyObjectArea);

        switch (area.m_SkewDirection)
        {
            case DirectionEnum.Forward: break;
            case DirectionEnum.Backward: forward *= -1; break;
                //case DirectionEnum.Close: forward = (emc.pos - mc.pos); break;
                //case DirectionEnum.Leave: forward = (mc.pos - emc.pos); break;
        }

        switch (area.m_Shape)
        {
            case AreaType.Circle:
                leftBorder = forward.RotateInXZ(360 * 0.5f);
                sectorStep = 360 / (Length + 1);
                break;
            case AreaType.Sector:
                leftBorder = forward.RotateInXZ(area.m_Angle * 0.5f);
                sectorStep = area.m_Angle / (Length + 1);
                pos = pos + forward * area.m_SkewDistance;
                break;
            case AreaType.Rectangle:
                leftDir = forward.RotateInXZ(90);
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
                    dir = leftBorder.RotateInXZ2((i + 1) * sectorStep);

                    break;
                case AreaType.Rectangle:
                    pos = leftPos - leftDir * rectangleStep * (i + 1);
                    break;
            }
            CreatPostionInfo cpi = new CreatPostionInfo();
            pos.y = 0;
            dir.y = 0;

            cpi.m_pos = pos;
            cpi.m_dir = dir;
            result.Add(cpi);

            //CreatPostionInfo cpi = new CreatPostionInfo();
            //cpi.m_pos = mc.pos.DeepCopy();
            //cpi.m_dir.FromVector( ssc.skillDir.DeepCopy().ToVector().normalized);

            //result.Add(cpi);
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
                if (!bfc.isBlow)
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

        //Debug.Log("Damage == " + damageNumber + " hurter  " + hurter.ID + " acc " + acc.creater + " bcc " + bcc.creater + " frame " + m_world.FrameCount);

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

    #region 技能恢复

    void Recover(EntityBase skiller, EntityBase recover, SkillDataGenerate skillData)
    {
        int revoverNumber = RecoverValueFormula(skillData, skiller, recover);
        if (revoverNumber == 0)
        {
            return;
        }

        SkillUtils.Recover(m_world, skiller, recover, revoverNumber);
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
        SkillStatusComponent ssc = entity.GetComp<SkillStatusComponent>();

        List<EntityBase> result = new List<EntityBase>();
        List<EntityBase> list = GetEntityList(new string[] { "CollisionComponent", "LifeComponent", "CampComponent" });

        SkillUtils.UpdateArea(skillAreaCache, skillData.m_EffectArea, ssc.skillDir.ToVector(), entity);

        Debug.DrawRay(skillAreaCache.position, skillAreaCache.direction, Color.red, 10);

        for (int i = 0; i < list.Count; i++)
        {
            CollisionComponent bcc = list[i].GetComp<CollisionComponent>();
            CampComponent bcampc = list[i].GetComp<CampComponent>();
            LifeComponent lc = list[i].GetComp<LifeComponent>();

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

    static int RecoverValueFormula(SkillDataGenerate skillData, EntityBase skiller, EntityBase recover)
    {
        if (skillData.m_ReValuep >= 0)
        {
            return (int)(skillData.m_RecoverValue);
        }
        else
        {
            return (int)(skillData.m_RecoverValue);
        }
    }

    #endregion

    struct CreatPostionInfo
    {
        public SyncVector3 m_pos;
        public SyncVector3 m_dir;
    }
}
