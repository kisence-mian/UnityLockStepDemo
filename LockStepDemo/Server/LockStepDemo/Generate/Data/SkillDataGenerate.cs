using System;
using UnityEngine;

//SkillDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class SkillDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_name; //技能名
	public string m_info; //技能描述
	public string m_icon; //技能图标
	public string m_HitFXName; //特效id
	public string m_EffectArea; //攻击范围
	public int m_DamageValue; //伤害值
	public float m_HurtSFXDelay; //命中音效延迟
	public string m_HurtSFX; //音效文件名
	public bool m_CanBreak; //这个技能可打断别人
	public string m_BlowFlyID; //造成目标击飞ID
	public string[] m_HurtBuff; //给别人buff
	public int m_CD; //冷却时间
	public bool m_Moment; //是否瞬间伤害（非瞬发的意思）
	public int m_TriggerSpaceTime; //周期伤害的伤害间隔
	public bool m_allowMove; //放这个技能期间是否可以移动
	public bool m_noclip; //穿墙
	public string m_FlyObjectArea; //飞行物释放范围
	public int m_FlyDamageValue; //飞行物伤害
	public string[] m_FlyObjectName; //飞行物模组名
	public int m_flydistance; //陨石掉落身前距离
	public int m_uplv; //升级等级
	public string m_nextskill; //下级技能
	public int m_moneytype; //金币或者钻石
	public int m_moneynum; //货币数量
	public int m_material; //材料类型
	public int m_materialnum; //材料数量
	public string m_BeforeStatus; //前段
	public string m_CurrentStatus; //中段
	public string m_LaterStatus; //后段
	public float m_RaiseTime; //不要配这个字段
	public int m_HitTime; //中段命中时间
	public HardPointEnum m_HitFXCreatPoint; //命中特效挂载点
	public string m_AreaTexture; //范围花纹
	public int m_DamagePer; //技能增幅
	public int m_RecoverValue; //恢复值
	public int m_ReValuep; //恢复比例
	public string m_HurtCameraShoke; //震屏效果类型
	public int m_flydamageper; //飞行物伤害增幅
	public HardPointEnum m_FlyCreatPoint; //飞行物出现点
	public bool m_CanBeBreakInC; //在C段中能否被打断
	public bool m_CanBeBreak; //在B段能否被打断
	public string m_MoveID; //自身位移ID
	public string[] m_SelfBuff; //自己上buff
	public string[] m_RecoverBuff; //给队友buff
	public string[] m_SummonMonster; //召唤怪物的ID（ID来自怪物表）
	public bool m_AffectTrap; //是否影响陷阱
	public bool m_IsAreaTip; //是否产生技能范围指示器
	public string m_SkillAgency; //代放技能

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("SkillData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("SkillDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_name = data.GetString("name");
		m_info = data.GetString("info");
		m_icon = data.GetString("icon");
		m_HitFXName = data.GetString("HitFXName");
		m_EffectArea = data.GetString("EffectArea");
		m_DamageValue = data.GetInt("DamageValue");
		m_HurtSFXDelay = data.GetFloat("HurtSFXDelay");
		m_HurtSFX = data.GetString("HurtSFX");
		m_CanBreak = data.GetBool("CanBreak");
		m_BlowFlyID = data.GetString("BlowFlyID");
		m_HurtBuff = data.GetStringArray("HurtBuff");
		m_CD = data.GetInt("CD");
		m_Moment = data.GetBool("Moment");
		m_TriggerSpaceTime = data.GetInt("TriggerSpaceTime");
		m_allowMove = data.GetBool("allowMove");
		m_noclip = data.GetBool("noclip");
		m_FlyObjectArea = data.GetString("FlyObjectArea");
		m_FlyDamageValue = data.GetInt("FlyDamageValue");
		m_FlyObjectName = data.GetStringArray("FlyObjectName");
		m_flydistance = data.GetInt("flydistance");
		m_uplv = data.GetInt("uplv");
		m_nextskill = data.GetString("nextskill");
		m_moneytype = data.GetInt("moneytype");
		m_moneynum = data.GetInt("moneynum");
		m_material = data.GetInt("material");
		m_materialnum = data.GetInt("materialnum");
		m_BeforeStatus = data.GetString("BeforeStatus");
		m_CurrentStatus = data.GetString("CurrentStatus");
		m_LaterStatus = data.GetString("LaterStatus");
		m_RaiseTime = data.GetFloat("RaiseTime");
		m_HitTime = data.GetInt("HitTime");
		m_HitFXCreatPoint = data.GetEnum<HardPointEnum>("HitFXCreatPoint");
		m_AreaTexture = data.GetString("AreaTexture");
		m_DamagePer = data.GetInt("DamagePer");
		m_RecoverValue = data.GetInt("RecoverValue");
		m_ReValuep = data.GetInt("ReValuep");
		m_HurtCameraShoke = data.GetString("HurtCameraShoke");
		m_flydamageper = data.GetInt("flydamageper");
		m_FlyCreatPoint = data.GetEnum<HardPointEnum>("FlyCreatPoint");
		m_CanBeBreakInC = data.GetBool("CanBeBreakInC");
		m_CanBeBreak = data.GetBool("CanBeBreak");
		m_MoveID = data.GetString("MoveID");
		m_SelfBuff = data.GetStringArray("SelfBuff");
		m_RecoverBuff = data.GetStringArray("RecoverBuff");
		m_SummonMonster = data.GetStringArray("SummonMonster");
		m_AffectTrap = data.GetBool("AffectTrap");
		m_IsAreaTip = data.GetBool("IsAreaTip");
		m_SkillAgency = data.GetString("SkillAgency");
	}
}
