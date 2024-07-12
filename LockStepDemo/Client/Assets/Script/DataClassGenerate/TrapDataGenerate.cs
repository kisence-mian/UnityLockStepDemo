using System;
using UnityEngine;

//TrapDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class TrapDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_ModelID; //
	public string m_IdleAnimName; //
	public string m_HurtAnimName; //
	public float m_HurtAnimTime; //
	public string m_DieAnimName; //
	public string m_DropOutID; //ID
	public int m_hp; //
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
	public float m_BloodHight; //Y
	public string[] m_AttackList; //
	public string[] m_SkillList; //
	public bool m_AutoTrigger; //
	public float m_TriggerSpace; //
	public string m_TriggerArea; //
	public string m_BodyArea; //
	public bool m_EveryOneCanRemove; //每个人都能拆除
	public string m_RemoveSkill; //拆除技能
	public bool m_isCollideTrigger; //是否通过碰撞触发

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("TrapData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("TrapDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_ModelID = data.GetString("ModelID");
		m_IdleAnimName = data.GetString("IdleAnimName");
		m_HurtAnimName = data.GetString("HurtAnimName");
		m_HurtAnimTime = data.GetFloat("HurtAnimTime");
		m_DieAnimName = data.GetString("DieAnimName");
		m_DropOutID = data.GetString("DropOutID");
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
		m_BloodHight = data.GetFloat("BloodHight");
		m_AttackList = data.GetStringArray("AttackList");
		m_SkillList = data.GetStringArray("SkillList");
		m_AutoTrigger = data.GetBool("AutoTrigger");
		m_TriggerSpace = data.GetFloat("TriggerSpace");
		m_TriggerArea = data.GetString("TriggerArea");
		m_BodyArea = data.GetString("BodyArea");
		m_EveryOneCanRemove = data.GetBool("EveryOneCanRemove");
		m_RemoveSkill = data.GetString("RemoveSkill");
		m_isCollideTrigger = data.GetBool("isCollideTrigger");
	}
}
