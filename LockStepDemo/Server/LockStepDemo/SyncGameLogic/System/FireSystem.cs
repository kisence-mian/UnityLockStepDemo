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

            if (cc.skillDir.ToVector() != Vector3.zero
                && cc.isFire
                && cdc.CD <= 0)
            {
                cdc.CD = 2 * 1000;

                //FIRE!!! 2000 2002技能可用

                Debug.Log("FIRE!!!");
                string skillID = "2001";

                ssc.m_skillTime = 0;
                ssc.m_skillStstus = SkillStatusEnum.Before;
                ssc.m_isTriggerSkill = false;
                ssc.m_currentSkillData = ssc.GetSkillData(skillID);
                ssc.m_currentSkillData.UpdateInfo();
                ssc.skillDir = pc.faceDir.DeepCopy();
            }
        }
    }
}
