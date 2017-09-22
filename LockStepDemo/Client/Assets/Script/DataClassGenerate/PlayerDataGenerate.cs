using System;
using UnityEngine;

//PlayerDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class PlayerDataGenerate : DataGenerateBase 
{
	public string m_key;
	public int m_type; //类型
	public string m_playername; //玩家名
	public string m_playerint; //玩家介绍
	public int m_att; //攻击力
	public int m_def; //防御力
	public int m_hp; //生命值
	public int m_critdamage; //暴击伤害
	public int m_crit; //暴击率
	public float m_movespeed; //移动速度
	public int m_specialid; //特性
	public string m_ModelID; //模型ID
	public string m_walkAnimName; //前进动画
	public string m_BackWalkAnim; //后退动画
	public string m_LeftWalkAnim; //左走动画
	public string m_RightWalkAnim; //右走动画
	public string m_cloakAnimName;
	public string m_IdleAnimName;
	public string m_HurtAnimName;
	public float m_HurtAnimTime;
	public string m_DieAnimName;
	public string m_ShowAniName;
	public float m_Radius; //半径
	public bool m_IsOnlyDamageValue;
	public float m_BloodHight; //Y
	public string m_FootstepSFX; //脚步声
	public int m_gold; //金币
	public float m_diamond; //钻石
	public int m_lv; //等级
	public int m_exp; //经验值
	public int m_expneed; //升级经验
	public int m_phy; //体力值
	public int m_renown; //声望
	public int m_power; //战斗力
	public int m_hprecover; //生命恢复
	public int m_ignoredef; //破防
	public int m_hpabsorb; //吸血
	public int m_tough; //韧性

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("PlayerData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("PlayerDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_type = data.GetInt("type");
		m_playername = data.GetString("playername");
		m_playerint = data.GetString("playerint");
		m_att = data.GetInt("att");
		m_def = data.GetInt("def");
		m_hp = data.GetInt("hp");
		m_critdamage = data.GetInt("critdamage");
		m_crit = data.GetInt("crit");
		m_movespeed = data.GetFloat("movespeed");
		m_specialid = data.GetInt("specialid");
		m_ModelID = data.GetString("ModelID");
		m_walkAnimName = data.GetString("walkAnimName");
		m_BackWalkAnim = data.GetString("BackWalkAnim");
		m_LeftWalkAnim = data.GetString("LeftWalkAnim");
		m_RightWalkAnim = data.GetString("RightWalkAnim");
		m_cloakAnimName = data.GetString("cloakAnimName");
		m_IdleAnimName = data.GetString("IdleAnimName");
		m_HurtAnimName = data.GetString("HurtAnimName");
		m_HurtAnimTime = data.GetFloat("HurtAnimTime");
		m_DieAnimName = data.GetString("DieAnimName");
		m_ShowAniName = data.GetString("ShowAniName");
		m_Radius = data.GetFloat("Radius");
		m_IsOnlyDamageValue = data.GetBool("IsOnlyDamageValue");
		m_BloodHight = data.GetFloat("BloodHight");
		m_FootstepSFX = data.GetString("FootstepSFX");
		m_gold = data.GetInt("gold");
		m_diamond = data.GetFloat("diamond");
		m_lv = data.GetInt("lv");
		m_exp = data.GetInt("exp");
		m_expneed = data.GetInt("expneed");
		m_phy = data.GetInt("phy");
		m_renown = data.GetInt("renown");
		m_power = data.GetInt("power");
		m_hprecover = data.GetInt("hprecover");
		m_ignoredef = data.GetInt("ignoredef");
		m_hpabsorb = data.GetInt("hpabsorb");
		m_tough = data.GetInt("tough");
	}
}
