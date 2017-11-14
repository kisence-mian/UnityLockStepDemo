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
                typeof(CommandComponent),
                typeof(SkillStatusComponent),
                typeof(PlayerComponent),
                typeof(MoveComponent),
                typeof(LifeComponent),
            };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent cc = (CommandComponent)list[i].GetComp("CommandComponent");
            SkillStatusComponent ssc = (SkillStatusComponent)list[i].GetComp("SkillStatusComponent");
            PlayerComponent pc = (PlayerComponent)list[i].GetComp("PlayerComponent");
            MoveComponent mc = (MoveComponent)list[i].GetComp("MoveComponent");
            LifeComponent lc = (LifeComponent)list[i].GetComp("LifeComponent");

            //CD
            for (int j = 0; j < ssc.m_skillList.Count; j++)
            {
                ssc.m_CDList[j] -= deltaTime;
            }

            if (ssc.skillDirCache.ToVector() != Vector3.zero
                && cc.isFire
                && !pc.GetIsDizziness()
                && lc.Life > 0
                && cc.skillDir.ToVector() == Vector3.zero
                )
            {
                string skillID = SkillUtils.GetSkillName(cc);
                SkillData data = ssc.GetSkillData(skillID);

                if (ssc.GetSkillCDFinsih(skillID))
                {
                    //Debug.Log("FIRE!!! --> " + skillID);
                    ssc.m_skillTime = 0;
                    ssc.m_skillStstus = SkillStatusEnum.Before;
                    ssc.m_isTriggerSkill = false;
                    ssc.m_currentSkillData = data;
                    ssc.m_currentSkillData.UpdateInfo();

                    ssc.SetSkillCD(skillID, data.CDSpace);

                    ssc.skillDir = ssc.skillDirCache.DeepCopy();
                    AreaDataGenerate areaData = DataGenerateManager<AreaDataGenerate>.GetData(ssc.m_currentSkillData.SkillInfo.m_EffectArea);

                    float distance = ssc.skillDir.ToVector().magnitude;
                    distance = areaData.m_SkewDistance + Mathf.Clamp(distance, 0, areaData.m_distance);

                    Vector3 aimPos = mc.pos.ToVector() + ssc.skillDir.ToVector().normalized * distance;
                    if (areaData.m_Shape != AreaType.Rectangle)
                    {
                        ssc.skillPos.FromVector(aimPos);
                    }
                    else
                    {
                        ssc.skillPos.FromVector(mc.pos.ToVector() + ssc.skillDir.ToVector().normalized * (areaData.m_SkewDistance - areaData.m_Length / 2));
                    }
                }



            }

            ssc.skillDirCache = cc.skillDir.DeepCopy();
        }
    }



}
