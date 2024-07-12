using UnityEngine;
using System.Collections;

public class BraveProperty : CharacterBaseProperty
{
    public BraveProperty(string l_s_characterName)
        : base(l_s_characterName, CharacterTypeEnum.Brave)
    {

    }

    public override void LoadData(string characterName,CharacterTypeEnum roleType)
    {
        PlayerDataGenerate info = DataGenerateManager<PlayerDataGenerate>.GetData(characterName);

        m_walkAnimName = info.m_walkAnimName;
        m_BackWalkAnimName = info.m_BackWalkAnim;
        m_LeftWalkAnimName = info.m_LeftWalkAnim;
        m_RightWalkAnimName = info.m_RightWalkAnim;

        m_idleAnimName = info.m_IdleAnimName;
        m_dieAnimName = info.m_DieAnimName;
        m_hurtAnimName = info.m_HurtAnimName;
        m_hurtAnimTime = info.m_HurtAnimTime;
        m_cloakAnimName = info.m_cloakAnimName;
        m_showAnimName = info.m_ShowAniName;

        m_MaxHp = info.m_hp;
        m_speed = info.m_movespeed;

        m_radius = info.m_Radius;
        m_isOnlyDamageValue = info.m_IsOnlyDamageValue;

        m_bloodHight = info.m_BloodHight;
        m_modelID = info.m_ModelID;
        m_dropOutID = null;

        WeaponDataGenerate data =  DataGenerateManager<WeaponDataGenerate>.GetData("1201");

        m_AttackList = data.m_AttackList;
        m_SkillList = data.m_SkillList;

        ResetProperty();
    }
	
}
