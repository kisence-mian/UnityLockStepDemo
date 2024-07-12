using System;
using UnityEngine;

//BuffDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class BuffDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_BuffCreateFX; //Buff特效
	public string m_BuffExitFX; //buff结束
	public string m_BuffFX; //buff特效id
	public HardPointEnum m_BuffFollowPos; //buff跟随点
	public float m_BuffTime; //持续时间
	public int m_AttackChange; //攻击改变
	public float m_AttackChangePercantage; //攻击百分比
	public int m_SpeedChange; //速度改变
	public float m_SpeedChangePercentage; //速度百分比
	public bool m_Dizziness;
	public float m_BuffEffectSpace; //生效间隔
	public bool m_Cloaking; //隐形
	public bool m_Invincible; //无敌
	public bool m_BeDamageInterrupt; //伤害打断
	public bool m_UseSkilIinterrupt; //使用技能打断
	public int m_RecoverNumber; //生命恢复值
	public int m_DamageNumber; //伤害值
	public bool m_IsTakeOver; //是否接管角色
	public string m_AnimName; //接管角色后的动画名
	public bool m_TrueSight; //真视
	public string m_BuffCreateSFX; //Buff创建音效
	public string m_BuffExitSFX; //buff消失音效
	public string m_BuffSFX; //Buff持续音效
	public string m_BuffhitSFX; //Buff伤害音效
	public int m_defChange; //防御力改变
	public int m_hprecoverChange; //生命恢复改变
	public int m_critChange; //暴击率改变
	public int m_critdamageChange; //暴击伤害改变
	public int m_ignoredefChange; //破防改变
	public int m_hpabsorbChange; //吸血改变
	public int m_toughChange; //韧性改变
	public float m_defPercantage; //防御力改变百分比
	public float m_hprecoverPercantage; //生命恢复改变百分比
	public float m_critPercantage; //暴击率改变百分比
	public float m_critdamagePercantage; //暴击伤害改变百分比
	public float m_ignoredefPercantage; //破防改变百分比
	public float m_hpabsorbPercantage; //吸血改变百分比
	public float m_toughPercantage; //韧性改变百分比
	public string m_EffectArea; //作用范围
	public int m_limit; //叠加上限

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("BuffData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("BuffDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_BuffCreateFX = data.GetString("BuffCreateFX");
		m_BuffExitFX = data.GetString("BuffExitFX");
		m_BuffFX = data.GetString("BuffFX");
		m_BuffFollowPos = data.GetEnum<HardPointEnum>("BuffFollowPos");
		m_BuffTime = data.GetFloat("BuffTime");
		m_AttackChange = data.GetInt("AttackChange");
		m_AttackChangePercantage = data.GetFloat("AttackChangePercantage");
		m_SpeedChange = data.GetInt("SpeedChange");
		m_SpeedChangePercentage = data.GetFloat("SpeedChangePercentage");
		m_Dizziness = data.GetBool("Dizziness");
		m_BuffEffectSpace = data.GetFloat("BuffEffectSpace");
		m_Cloaking = data.GetBool("Cloaking");
		m_Invincible = data.GetBool("Invincible");
		m_BeDamageInterrupt = data.GetBool("BeDamageInterrupt");
		m_UseSkilIinterrupt = data.GetBool("UseSkilIinterrupt");
		m_RecoverNumber = data.GetInt("RecoverNumber");
		m_DamageNumber = data.GetInt("DamageNumber");
		m_IsTakeOver = data.GetBool("IsTakeOver");
		m_AnimName = data.GetString("AnimName");
		m_TrueSight = data.GetBool("TrueSight");
		m_BuffCreateSFX = data.GetString("BuffCreateSFX");
		m_BuffExitSFX = data.GetString("BuffExitSFX");
		m_BuffSFX = data.GetString("BuffSFX");
		m_BuffhitSFX = data.GetString("BuffhitSFX");
		m_defChange = data.GetInt("defChange");
		m_hprecoverChange = data.GetInt("hprecoverChange");
		m_critChange = data.GetInt("critChange");
		m_critdamageChange = data.GetInt("critdamageChange");
		m_ignoredefChange = data.GetInt("ignoredefChange");
		m_hpabsorbChange = data.GetInt("hpabsorbChange");
		m_toughChange = data.GetInt("toughChange");
		m_defPercantage = data.GetFloat("defPercantage");
		m_hprecoverPercantage = data.GetFloat("hprecoverPercantage");
		m_critPercantage = data.GetFloat("critPercantage");
		m_critdamagePercantage = data.GetFloat("critdamagePercantage");
		m_ignoredefPercantage = data.GetFloat("ignoredefPercantage");
		m_hpabsorbPercantage = data.GetFloat("hpabsorbPercantage");
		m_toughPercantage = data.GetFloat("toughPercantage");
		m_EffectArea = data.GetString("EffectArea");
		m_limit = data.GetInt("limit");
	}
}
