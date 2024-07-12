using System;
using UnityEngine;

//WeaponDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class WeaponDataGenerate : DataGenerateBase 
{
	public string m_key;
	public int m_weaponfield; //装备位置id: 1 武器 2 肩膀 3胸甲 4 手腕 5 腰带 6裤子 7鞋子
	public string m_weaponname; //装备名称
	public string m_Qualitylevel; //品质等级
	public float m_qualitynumber; //装备品质系数
	public string m_Qualityframe; //品质边框id
	public string m_icon; //图标id
	public float m_gs; //装备等级
	public float m_outfitnumber; //装备等级系数
	public int m_gem_count; //可镶嵌宝石数量
	public int m_enchant_count; //可附魔条数
	public int m_enchant_money_need; //附魔需要的钱
	public int m_enchant_item_need_211; //附魔需要的材料211数量
	public int m_enchant_item_need_212; //附魔需要的材料212数量
	public int m_enchant_item_need_213; //附魔需要的材料213数量
	public int m_enchant_item_need_214; //附魔需要的材料214数量
	public int m_enchant_item_need_215; //附魔需要的材料215数量
	public int m_att; //攻击力
	public int m_def; //防御力
	public int m_hp; //生命值
	public int m_hprecover; //生命恢复
	public int m_crit; //暴击率
	public int m_critdamage; //暴击伤害
	public int m_ignoredef; //破防
	public int m_hpabsorb; //吸血
	public int m_movespeed; //移动速度
	public int m_tough; //韧性
	public string[] m_ModelName; //武器模型名字，竖线分割给01,02号挂载点
	public Vector3 m_local_position; //位置
	public Vector3 m_local_euler_angles; //旋转
	public Vector3 m_local_scale; //缩放
	public string[] m_AttackList; //攻击列表 
	public string[] m_SkillList; //技能列表
	public string m_WalkAnimName; //行走动画
	public string m_m_idleAnimName; //站立动画

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("WeaponData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("WeaponDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_weaponfield = data.GetInt("weaponfield");
		m_weaponname = data.GetString("weaponname");
		m_Qualitylevel = data.GetString("Qualitylevel");
		m_qualitynumber = data.GetFloat("qualitynumber");
		m_Qualityframe = data.GetString("Qualityframe");
		m_icon = data.GetString("icon");
		m_gs = data.GetFloat("gs");
		m_outfitnumber = data.GetFloat("outfitnumber");
		m_gem_count = data.GetInt("gem_count");
		m_enchant_count = data.GetInt("enchant_count");
		m_enchant_money_need = data.GetInt("enchant_money_need");
		m_enchant_item_need_211 = data.GetInt("enchant_item_need_211");
		m_enchant_item_need_212 = data.GetInt("enchant_item_need_212");
		m_enchant_item_need_213 = data.GetInt("enchant_item_need_213");
		m_enchant_item_need_214 = data.GetInt("enchant_item_need_214");
		m_enchant_item_need_215 = data.GetInt("enchant_item_need_215");
		m_att = data.GetInt("att");
		m_def = data.GetInt("def");
		m_hp = data.GetInt("hp");
		m_hprecover = data.GetInt("hprecover");
		m_crit = data.GetInt("crit");
		m_critdamage = data.GetInt("critdamage");
		m_ignoredef = data.GetInt("ignoredef");
		m_hpabsorb = data.GetInt("hpabsorb");
		m_movespeed = data.GetInt("movespeed");
		m_tough = data.GetInt("tough");
		m_ModelName = data.GetStringArray("ModelName");
		m_local_position = data.GetVector3("local_position");
		m_local_euler_angles = data.GetVector3("local_euler_angles");
		m_local_scale = data.GetVector3("local_scale");
		m_AttackList = data.GetStringArray("AttackList");
		m_SkillList = data.GetStringArray("SkillList");
		m_WalkAnimName = data.GetString("WalkAnimName");
		m_m_idleAnimName = data.GetString("m_idleAnimName");
	}
}
