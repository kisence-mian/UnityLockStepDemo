using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FireSystem : ViewSystemBase
{
    const int Element_NoChoice = -1;

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
                
                string skillID = GetSkillName(cc);

                //Debug.Log("FIRE!!! --> " + skillID);

                ssc.m_skillTime = 0;
                ssc.m_skillStstus = SkillStatusEnum.Before;
                ssc.m_isTriggerSkill = false;
                ssc.m_currentSkillData = ssc.GetSkillData(skillID);
                ssc.m_currentSkillData.UpdateInfo();
                ssc.skillDir = cc.skillDir.DeepCopy();
            }
        }
    }


    DataTable m_comboData;
    string GetSkillName(CommandComponent cmd)
    {
        if (m_comboData == null)
        {
            m_comboData = DataManager.GetData("CombineData");
        }

        if (cmd.element1 == Element_NoChoice && cmd.element2 == Element_NoChoice)
        {
            return DataGenerateManager<CombineDataGenerate>.GetData(m_comboData.TableIDs[0]).m_key;
        }

        for (int i = 0; i < m_comboData.TableIDs.Count; i++)
        {
            CombineDataGenerate data = DataGenerateManager<CombineDataGenerate>.GetData(m_comboData.TableIDs[i]);
            if (data.m_ele_1 != Element_NoChoice &&
                data.m_ele_2 != Element_NoChoice
                )
            {
                if ((data.m_ele_1 == cmd.element1 && data.m_ele_2 == cmd.element2)
                    || (data.m_ele_2 ==cmd.element1 && data.m_ele_1 == cmd.element2)
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

        //Error!
        throw new System.Exception("Not Find SkillName!");
    }
}
