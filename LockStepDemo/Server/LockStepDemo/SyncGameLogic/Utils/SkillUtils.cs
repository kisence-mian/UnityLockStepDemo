using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillUtils
{
    public static void FlyDamageLogic(WorldBase world, EntityBase fly, EntityBase entity)
    {
        FlyObjectComponent fc = fly.GetComp<FlyObjectComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();
        CampComponent campComp = fly.GetComp<CampComponent>();
        MoveComponent mc = fly.GetComp<MoveComponent>();

        //Debug.Log("FlyDamageLogic " + entity.ID + " ===> " + fc.damage);
        lc.Life -= fc.damage;

        if(fc.FlyData.m_TriggerSkill != "null" && fc.FlyData.m_TriggerSkill != "Null")
        {
            SkillDataGenerate blowSkill = DataGenerateManager<SkillDataGenerate>.GetData(fc.FlyData.m_TriggerSkill);

            //飞行物击飞
            BlowFly(world, fly, entity, blowSkill);
        }

        //释放触发技能
        //TokenUseSkill(world,campComp.creater,fc.FlyData.m_TriggerSkill, mc.pos.ToVector(),mc.dir.ToVector());
    }

    #region 技能代处理

    public static void TokenUseSkill(WorldBase world,int createrID, string skillID, Vector3 pos, Vector3 dir)
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

            if(ssc.m_currentSkillData.LaterTime == 0)
            {
                Debug.LogError("技能代 " + skillID + "的持续时间不能为 0 !");
                return;
            }

            MoveComponent mc = new MoveComponent();
            mc.pos.FromVector(pos);
            mc.dir.FromVector(dir);

            //打印
            Debug.DrawRay(mc.pos.ToVector(), -mc.dir.ToVector(),Color.green,10);

            CampComponent cc = new CampComponent();
            cc.creater = createrID;

            LifeSpanComponent lsc = new LifeSpanComponent();
            lsc.lifeTime = (int)(ssc.m_currentSkillData.LaterTime * 1000);

            world.CreateEntity(mc, ssc, cc, lsc);
        }
    }

    #endregion

    #region 击飞处理

    public static void BlowFly(WorldBase world,EntityBase skiller, EntityBase hurter, SkillDataGenerate skillData)
    {
        MoveComponent amc = skiller.GetComp<MoveComponent>();
        MoveComponent bmc = hurter.GetComp<MoveComponent>();

        Vector3 dir = amc.dir.ToVector();
        if(skiller.GetExistComp<SkillStatusComponent>())
        {
            SkillStatusComponent assc = skiller.GetComp<SkillStatusComponent>();
            dir = assc.skillDir.ToVector();
        }

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
                    bfc.SetBlowFly(amc.pos.ToVector(), bmc.pos.ToVector(), dir);
                }
            }
        }
    }

    #endregion
}
