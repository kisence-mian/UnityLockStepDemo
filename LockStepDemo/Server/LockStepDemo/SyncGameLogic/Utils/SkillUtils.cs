using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillUtils
{
    const int Element_NoChoice = -1;

    #region 技能查找

    static DataTable s_comboData;
    public static string GetSkillName(CommandComponent cmd)
    {
        if (s_comboData == null)
        {
            s_comboData = DataManager.GetData("CombineData");
        }

        if (cmd.element1 == Element_NoChoice && cmd.element2 == Element_NoChoice)
        {
            return DataGenerateManager<CombineDataGenerate>.GetData(s_comboData.TableIDs[0]).m_key;
        }

        for (int i = 0; i < s_comboData.TableIDs.Count; i++)
        {
            CombineDataGenerate data = DataGenerateManager<CombineDataGenerate>.GetData(s_comboData.TableIDs[i]);
            if (data.m_ele_1 != Element_NoChoice &&
                data.m_ele_2 != Element_NoChoice
                )
            {
                if ((data.m_ele_1 == cmd.element1 && data.m_ele_2 == cmd.element2)
                    || (data.m_ele_2 == cmd.element1 && data.m_ele_1 == cmd.element2)
                    )
                {
                    return data.m_key;
                }
            }
            else
            {
                if ((data.m_ele_1 == cmd.element1 && data.m_ele_2 == 0)
                    || (data.m_ele_2 == cmd.element2 && data.m_ele_1 == 0)
                    )
                {
                    return data.m_key;
                }
            }
        }

        return null;

        //Error!
        throw new System.Exception("Not Find SkillName!");
    }

    #endregion

    #region 飞行物
    public static void FlyDamageLogic(WorldBase world, EntityBase fly, EntityBase entity)
    {
        FlyObjectComponent fc = fly.GetComp<FlyObjectComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();
        CampComponent campComp = fly.GetComp<CampComponent>();
        MoveComponent mc = fly.GetComp<MoveComponent>();

        //Debug.Log("FlyDamageLogic " + entity.ID + " ===> campComp.creater " + campComp.creater + " " + world.GetEntity(campComp.creater).ID);
        Damage(world, world.GetEntity(campComp.creater), entity, fc.damage);

        //飞行物击飞
        BlowFly(world, fly, entity, fc.FlyData.m_BlowFlyID);

        //飞行物Buff
        AddBuff(world, world.GetEntity(campComp.creater), entity, fc.FlyData.m_HurtBuff);

        //释放触发技能
        //TokenUseSkill(world,campComp.creater,fc.FlyData.m_TriggerSkill, mc.pos.ToVector(),mc.dir.ToVector());
    }

    #endregion

    #region 技能代处理

    public static void TokenUseSkill(WorldBase world, int createrID, string skillID, Vector3 pos, Vector3 dir)
    {
        //Debug.Log("TokenUseSkill pos" + pos + " frame " + world.FrameCount + " skillID" + skillID);

        if (skillID != null
            && skillID != "null"
            && skillID != "Null")
        {

            SkillStatusComponent ssc = new SkillStatusComponent();
            ssc.m_skillTime = 0;
            ssc.m_skillStstus = SkillStatusEnum.Before;
            ssc.m_isTriggerSkill = false;
            ssc.m_isHit = false;
            ssc.m_skillList.Add(new SkillData(skillID));
            ssc.m_currentSkillData = ssc.GetSkillData(skillID);
            ssc.m_currentSkillData.UpdateInfo();
            ssc.skillDir.FromVector(dir);

            if (ssc.m_currentSkillData.LaterTime == 0)
            {
                Debug.LogError("技能代 " + skillID + "的持续时间不能为 0 !");
                return;
            }

            TransfromComponent tc = new TransfromComponent();
            tc.pos.FromVector(pos);
            tc.dir.FromVector(dir);

            //打印
            Debug.DrawRay(tc.pos.ToVector(), -tc.dir.ToVector(), Color.green, 10);

            CampComponent cc = new CampComponent();
            cc.creater = createrID;

            LifeSpanComponent lsc = new LifeSpanComponent();
            lsc.lifeTime = (int)(ssc.m_currentSkillData.LaterTime * 1000);

            world.CreateEntity(createrID + "SkillToken", tc, ssc, cc, lsc);
        }
    }

    #endregion

    #region 击飞处理

    public static void BlowFly(WorldBase world, EntityBase skiller, EntityBase hurter, string blowFlyID)
    {
        MoveComponent amc = skiller.GetComp<MoveComponent>();
        MoveComponent bmc = hurter.GetComp<MoveComponent>();

        Vector3 dir = amc.dir.ToVector();
        if (skiller.GetExistComp<SkillStatusComponent>())
        {
            SkillStatusComponent assc = skiller.GetComp<SkillStatusComponent>();
            dir = assc.skillDir.ToVector();
        }

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
                    bfc.SetBlowFly(amc.pos.ToVector(), bmc.pos.ToVector(), dir);
                }
            }
        }
    }

    #endregion

    #region Buff处理

    public static void AddBuff(WorldBase world, EntityBase skiller, EntityBase hurter, string[] hurtBuff)
    {
        if (!hurter.GetExistComp<PlayerComponent>())
        {
            return;
        }

        PlayerComponent pc = hurter.GetComp<PlayerComponent>();

        for (int i = 0; i < hurtBuff.Length; i++)
        {
            //Debug.Log(" AddBuff " + skiller.ID);

            BuffInfo bi = pc.AddBuff(hurtBuff[i], skiller.ID);
            world.eventSystem.DispatchEvent(GameUtils.c_addBuff, hurter, bi);
        }
    }

    #endregion

    #region 伤害处理

    public static void Damage(WorldBase world, EntityBase attacker, EntityBase hurter, int damageNumber)
    {
        LifeComponent lc = hurter.GetComp<LifeComponent>();

        //已经死亡不造成伤害
        if (lc.life < 0)
        {
            return;
        }

        lc.Life -= damageNumber;
        //Debug.Log("派发伤害 frame " + world.FrameCount);
        world.eventSystem.DispatchEvent(GameUtils.GetEventKey(hurter.ID, CharacterEventType.Damage), hurter);

        if (lc.Life <= 0)
        {
            if (attacker.GetExistComp<PlayerComponent>()
                && hurter.GetExistComp<PlayerComponent>()
                )
            {
                PlayerComponent pc = attacker.GetComp<PlayerComponent>();
                pc.score++;

                world.eventSystem.DispatchEvent(GameUtils.c_scoreChange, null);
            }
        }
    }

    #endregion

    #region

    public static void Recover(WorldBase world, EntityBase attacker, EntityBase recover, int recoverNumber)
    {
        LifeComponent lc = recover.GetComp<LifeComponent>();

        //已经死亡不造成伤害
        if (lc.life < 0)
        {
            return;
        }

        lc.Life += recoverNumber;
        //Debug.Log("派发伤害 frame " + world.FrameCount);
        world.eventSystem.DispatchEvent(GameUtils.GetEventKey(recover.ID, CharacterEventType.Recover), recover);

        //if (lc.Life <= 0)
        //{
        //    if (attacker.GetExistComp<PlayerComponent>()
        //        && hurter.GetExistComp<PlayerComponent>()
        //        )
        //    {
        //        PlayerComponent pc = attacker.GetComp<PlayerComponent>();
        //        pc.score++;

        //        world.eventSystem.DispatchEvent(GameUtils.c_scoreChange, null);
        //    }
        //}
    }

    #endregion

    #region 范围拓展方法

    public static Area UpdatSkillArea(Area area, Vector3 dir, SkillDataGenerate skillData, EntityBase skiller, EntityBase aim = null)
    {
        string effectArea = skillData.m_EffectArea;
        UpdateArea(area, effectArea, dir, skiller, aim);
        return area;
    }

    public static void UpdateArea(Area area, string areaID, Vector3 dir, EntityBase skiller, EntityBase aim = null)
    {
        //MoveComponent smc = skiller.GetComp<MoveComponent>();
        SkillStatusComponent ssc = skiller.GetComp<SkillStatusComponent>();

        AreaDataGenerate areaData = DataGenerateManager<AreaDataGenerate>.GetData(areaID);
        Vector3 dirTmp = GetAreaDir(area, areaData, dir, skiller, aim);

        area.areaType = areaData.m_Shape;
        area.length = areaData.m_Length;
        area.Width = areaData.m_Width;
        area.angle = areaData.m_Angle;
        area.radius = areaData.m_Radius;

        area.direction = dirTmp.normalized;
        //area.position = smc.pos.ToVector() + area.direction * areaData.m_SkewDistance;

        area.position = ssc.skillPos.ToVector() + areaData.m_SkewDistance * ssc.skillDir.ToVector().normalized;

        //Debug.Log( "skiller forward"+skiller.transform.forward);
    }

    public static Vector3 GetAreaDir(Area area, AreaDataGenerate areaData, Vector3 dir, EntityBase skiller, EntityBase aim = null)
    {
        MoveComponent smc = skiller.GetComp<MoveComponent>();
        //SkillStatusComponent ssc = skiller.GetComp<SkillStatusComponent>();

        Vector3 l_dir = Vector3.zero;
        if (aim == null)
        {
            switch (areaData.m_SkewDirection)
            {
                case DirectionEnum.Forward:
                    l_dir = dir;
                    break;
                case DirectionEnum.Backward:
                    l_dir = dir * -1;
                    break;
                case DirectionEnum.Close:
                    Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为forward");
                    l_dir = dir;
                    break;
                case DirectionEnum.Leave:
                    Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为Backward");
                    l_dir = dir * -1; break;
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
                case DirectionEnum.Forward: l_dir = dir; break;
                case DirectionEnum.Backward: l_dir = dir * -1; break;
                case DirectionEnum.Leave: l_dir = amc.pos.ToVector() - smc.pos.ToVector(); break;
                case DirectionEnum.Close: l_dir = smc.pos.ToVector() - amc.pos.ToVector(); break;
            }
        }
        return l_dir;
    }

    #endregion
}
