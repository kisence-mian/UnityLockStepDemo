using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillToken : CharacterBase
{
    public int CreaterID = -1;

    public override void OnCharacterCreate()
    {
        m_skillStatus = (SkillTokenSkillStstus)LoadStatusInterface<SkillTokenSkillStstus>(CharacterStatusEnum.Skill);
        m_moveStatus = (MoveStatus)LoadStatusInterface<SkillTokenMoveStatus>(CharacterStatusEnum.Move);

        LoadHurtModel<HurtModel>();
    }

    public override void OnInit(string characterName, int characterID)
    {
        m_skilInfoList = new List<SkillData>();
        m_skilInfoList.Add(new SkillData(characterName));
    }

    public override bool GetInvincible()
    {
        return true;
    }
}
