using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

//[RequireComponent(typeof(ChangeWeaponCompmoent))]
public class Player : CharacterBase
{
    public ChangeWeaponCompmoent m_weaponComp;

    public override void OnCharacterCreate()
    {
        //加载行为接口
        m_moveStatus = (MoveStatus)LoadStatusInterface<MoveStatus>(CharacterStatusEnum.Move);
        m_atkStatus = (PlayerAttackEffect)LoadStatusInterface<PlayerAttackEffect>(CharacterStatusEnum.Attack);
        m_dieStatus = (DieStatus)LoadStatusInterface<DieStatus>(CharacterStatusEnum.Die);
        m_hurtStatus = (PlayerHurtStatus)LoadStatusInterface<PlayerHurtStatus>(CharacterStatusEnum.Hurt);
        m_skillStatus = (PlayerSkillEffect)LoadStatusInterface<PlayerSkillEffect>(CharacterStatusEnum.Skill);
        m_blowFlyStatus = (BlowFlyStatus)LoadStatusInterface<BlowFlyStatus>(CharacterStatusEnum.Blowfly);
        m_buffStatus = (BuffStatus)LoadStatusInterface<BuffStatus>(CharacterStatusEnum.Buff);

        LoadHurtModel<HurtModel>();
    }

    public override void OnInit(string characterName, int characterID)
    {
        PlayerDataGenerate pdata = DataGenerateManager<PlayerDataGenerate>.GetData(characterName);

        m_Property.HP = pdata.m_hp;
        m_Property.Speed = pdata.m_movespeed;

        //这里增加Buff

        m_attackInfoList = new List<SkillData>();

        m_skilInfoList = new List<SkillData>();

        DataTable data = DataManager.GetData("SkillData");
        for (int i = 0; i < data.TableIDs.Count; i++)
        {
            m_skilInfoList.Add(new SkillData(data.TableIDs[i], i));
        }
    }

    public override void OnDie()
    {
        m_UIComp.OnDie();
        m_materialComp.OnDie();
        m_moveComp.OnDie();
        m_animComp.OnDie();
        m_effectComp.OnDie();
    }

     #region 武器相关

    public void ChangeWeapon(string weaponid)
    {
        WeaponDataGenerate data = DataGenerateManager<WeaponDataGenerate>.GetData(weaponid);
        m_weaponComp.ChangeWeapon(data.m_ModelName);

        m_skilInfoList = GetSkillData(data);
        m_attackInfoList = GetAttackData(data);

        m_Property.m_idleAnimName = data.m_m_idleAnimName;
        m_Property.m_walkAnimName = data.m_WalkAnimName;
    }

    public List<SkillData> GetAttackData(WeaponDataGenerate data)
    {
        List<SkillData> InfoList = new List<SkillData>();
        for (int i = 0; i < data.m_AttackList.Length; i++)
        {
            InfoList.Add(new SkillData(data.m_AttackList[i]));
        }

        return InfoList;
    }

    public List<SkillData> GetSkillData(WeaponDataGenerate data)
    {
        List<SkillData> InfoList = new List<SkillData>();
        for (int i = 0; i < data.m_SkillList.Length; i++)
        {
            InfoList.Add(new SkillData(data.m_SkillList[i], (i + 1)));
        }
        int length = data.m_SkillList.Length;
        InfoList.Add(new SkillData( "caiji",0 ));

        return InfoList;
    }

     #endregion
}
