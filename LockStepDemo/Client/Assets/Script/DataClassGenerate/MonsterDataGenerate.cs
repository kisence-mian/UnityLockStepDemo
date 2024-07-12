using System;
using UnityEngine;

//MonsterDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class MonsterDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_MonsterName;
	public string m_HeadIcon; //头像图标
	public string m_ModelID;
	public string m_walkAnimName;
	public string m_IdleAnimName;
	public string m_HurtAnimName;
	public float m_HurtAnimTime;
	public string m_DieAnimName;
	public string m_ShowAniName;
	public string m_DropOutID; //ID
	public float m_movespeed;
	public float m_Radius;
	public int m_hp;
	public int m_lv; //等级
	public int m_power; //战斗力
	public int m_att; //攻击力
	public int m_def; //防御力
	public int m_hprecover; //生命恢复
	public int m_crit; //暴击率
	public int m_critdamage; //暴击伤害
	public int m_ignoredef; //破防
	public int m_hpabsorb; //吸血
	public int m_tough; //韧性
	public bool m_IsOnlyDamageValue;
	public float m_BloodHight; //Y
	public string[] m_AttackList;
	public string[] m_SkillList;
	public string m_AIConfig; //AI
	public string m_VisableArea; //AI ID

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("MonsterData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("MonsterDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_MonsterName = data.GetString("MonsterName");
		m_HeadIcon = data.GetString("HeadIcon");
		m_ModelID = data.GetString("ModelID");
		m_walkAnimName = data.GetString("walkAnimName");
		m_IdleAnimName = data.GetString("IdleAnimName");
		m_HurtAnimName = data.GetString("HurtAnimName");
		m_HurtAnimTime = data.GetFloat("HurtAnimTime");
		m_DieAnimName = data.GetString("DieAnimName");
		m_ShowAniName = data.GetString("ShowAniName");
		m_DropOutID = data.GetString("DropOutID");
		m_movespeed = data.GetFloat("movespeed");
		m_Radius = data.GetFloat("Radius");
		m_hp = data.GetInt("hp");
		m_lv = data.GetInt("lv");
		m_power = data.GetInt("power");
		m_att = data.GetInt("att");
		m_def = data.GetInt("def");
		m_hprecover = data.GetInt("hprecover");
		m_crit = data.GetInt("crit");
		m_critdamage = data.GetInt("critdamage");
		m_ignoredef = data.GetInt("ignoredef");
		m_hpabsorb = data.GetInt("hpabsorb");
		m_tough = data.GetInt("tough");
		m_IsOnlyDamageValue = data.GetBool("IsOnlyDamageValue");
		m_BloodHight = data.GetFloat("BloodHight");
		m_AttackList = data.GetStringArray("AttackList");
		m_SkillList = data.GetStringArray("SkillList");
		m_AIConfig = data.GetString("AIConfig");
		m_VisableArea = data.GetString("VisableArea");
	}
}
