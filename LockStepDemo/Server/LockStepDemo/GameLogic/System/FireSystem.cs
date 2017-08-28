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

            cdc.CD -= deltaTime;

            if (cc.skillDir.ToVector() != Vector3.zero
                && cc.isFire
                && cdc.CD <= 0)
            {
                cdc.CD = 2 * 1000;

                Debug.Log("FIRE!!!");

                //FIRE!!!
                string skillID = "2000";

                ssc.m_skillTime = 0;
                ssc.m_skillStstus = SkillStatusEnum.Before;
                ssc.m_currentSkillData = ssc.GetSkillData(skillID);
                ssc.m_currentSkillData.UpdateInfo();
            }
        }
    }
}
