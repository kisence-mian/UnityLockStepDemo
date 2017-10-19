using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FireSystem : ViewSystemBase
{


    public override Type[] GetFilter()
    {
        return new Type[] {
                typeof(CDComponent),
                typeof(CommandComponent),
                typeof(SkillStatusComponent),
                typeof(PlayerComponent),
            };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent cc = list[i].GetComp<CommandComponent>();
            CDComponent cdc = list[i].GetComp<CDComponent>();
            SkillStatusComponent ssc = list[i].GetComp<SkillStatusComponent>();
            PlayerComponent pc = list[i].GetComp<PlayerComponent>();

            cdc.CD -= deltaTime;

            //Debug.Log(cc.element1 + " --> " + cc.element2 + " CanFire " + (cdc.CD <= 0));

            if (ssc.skillDirCache.ToVector() != Vector3.zero
                && cc.isFire
                && cdc.CD <= 0
                && !pc.GetIsDizziness()
                
                && cc.skillDir.ToVector() == Vector3.zero
                )
            {
                cdc.CD = 2 * 1000;

                string skillID = SkillUtils.GetSkillName(cc);

                //Debug.Log("FIRE!!! --> " + skillID);

                ssc.m_skillTime = 0;
                ssc.m_skillStstus = SkillStatusEnum.Before;
                ssc.m_isTriggerSkill = false;
                ssc.m_currentSkillData = ssc.GetSkillData(skillID);
                ssc.m_currentSkillData.UpdateInfo();

                ssc.skillDir = ssc.skillDirCache.DeepCopy();
            }

            ssc.skillDirCache = cc.skillDir.DeepCopy();
        }
    }



}
