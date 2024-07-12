/// <summary>
/// 角色基础属性
/// </summary>
using System;
using UnityEngine;

public class CharacterBaseProperty
{
    private string m_name;
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    protected int m_hp;
    /// <summary>
    /// 角色生命值
    /// </summary>
    public int HP
    {
        get { return m_hp; }
        set
        {
            m_hp = value;

            if (m_hp > m_MaxHp)
            {
                m_hp = m_MaxHp;
            }

            if (m_hp < 0)
            {
                m_hp = 0;
            }
        }
    }

    protected int m_MaxHp;

    /// <summary>
    /// 角色最大生命值
    /// </summary>
    public int MaxHp
    {
        get { return m_MaxHp; }
        set { m_MaxHp = value; }
    }

    /// <summary>
    /// 角色护盾值
    /// </summary>
    private int m_Shield;

    public int Shield
    {
        get { return m_Shield; }
        set { m_Shield = value; }
    }


    protected float m_speed;

    /// <summary>
    /// 角色当前速度
    /// </summary>
    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    int m_mp;
    /// <summary>
    /// 角色当前能量
    /// </summary>
    public int MP
    {
        get { return m_mp; }
        set { m_mp = value; }
    }

    int m_MaxMp;
    /// <summary>
    /// 角色最大能量
    /// </summary>
    public int MaxMP
    {
        get { return m_MaxMp; }
        set { m_MaxMp = value; }
    }

    int m_attack;
    /// <summary>
    /// 角色攻击力
    /// </summary>
    public int NormalAttack
    {
        get { return m_attack; }
        set { m_attack = value; }
    }

    int m_magicAttack;
    /// <summary>
    /// 角色魔法攻击力
    /// </summary>
    public int MagicAttack
    {
        get { return m_magicAttack; }
        set { m_magicAttack = value; }
    }

    int m_armor;
    /// <summary>
    /// 角色护甲
    /// </summary>
    public int Armor
    {
        get { return m_armor; }
        set { m_armor = value; }
    }

    int m_defensePuncture;
    /// <summary>
    /// 穿刺防御力
    /// </summary>
    public int DefensePuncture
    {
        get { return m_defensePuncture; }
        set { m_defensePuncture = value; }
    }

    int m_defenseThump;
    /// <summary>
    /// 重击防御力
    /// </summary>
    public int DefenseThump
    {
        get { return m_defenseThump; }
        set { m_defenseThump = value; }
    }

    int m_defenseMagic;
    /// <summary>
    /// 魔法防御力
    /// </summary>
    public int DefenseMagic
    {
        get { return m_defenseMagic; }
        set { m_defenseMagic = value; }
    }

    public string m_walkAnimName;
    public string m_BackWalkAnimName;
    public string m_LeftWalkAnimName;
    public string m_RightWalkAnimName;
    public string m_idleAnimName;
    public string m_cloakAnimName;

    public string m_dieAnimName;
    public string m_hurtAnimName;
    public float m_hurtAnimTime;

    public string m_showAnimName;
    
    public bool m_isOnlyDamageValue;

    public string[] m_AttackAnimList;

    //占地类型
    public AreaType m_areaType = AreaType.Circle;
    //占地半径
    public float m_radius;

    /// <summary>
    /// 可视区域ID
    /// </summary>
    public string m_visableAreaID = "";

    //血条高度
    public float m_bloodHight;

    public string m_dropOutID;

    public string m_modelID;

    public float m_amplification = 1; //增幅

    public string[] m_AttackList;
    public string[] m_SkillList;

    //public int m_Hp;
    public int m_Attack = 1;
    public int m_Defense;
    public int m_HpRecover;
    public int m_Crit;
    public int m_CritDamage;
    public int m_IgnoreDefense;
    public int m_HpAbsorb;
    public int m_Speed;
    public int m_Tough;

    public CharacterBaseProperty(string characterName, CharacterTypeEnum l_characterType)
    {
        LoadData(characterName, l_characterType);
        ResetProperty();
    }

    public virtual void LoadData(string characterName, CharacterTypeEnum l_characterType)
    {

    }

    public void ResetProperty()
    {
        //HP = (int)(MaxHp * m_amplification);
        //MP = (int)(MaxMP * m_amplification);

        HP = (int)(MaxHp );
        MP = (int)(MaxMP );
    }
}

public enum CharacterTypeEnum
{
    Brave,
    Monster,
    Trap,
    SkillToken
}

