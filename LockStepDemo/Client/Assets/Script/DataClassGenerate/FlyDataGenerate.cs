using System;
using UnityEngine;

//FlyDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class FlyDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_ModelName; //
	public HardPointEnum m_HitFXCreatPoint; //
	public string m_HitEffect; //
	public float m_Speed; //
	public float m_Radius; //
	public float m_LiveTime; //
	public bool m_CollisionTrigger; //
	public string m_TriggerSkill; //
	public bool m_AcrossEnemy; //
	public string m_HitSFX; //
	public string[] m_HurtBuff; //buff
	public string m_BlowFlyID; //ID

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("FlyData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("FlyDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_ModelName = data.GetString("ModelName");
		m_HitFXCreatPoint = data.GetEnum<HardPointEnum>("HitFXCreatPoint");
		m_HitEffect = data.GetString("HitEffect");
		m_Speed = data.GetFloat("Speed");
		m_Radius = data.GetFloat("Radius");
		m_LiveTime = data.GetFloat("LiveTime");
		m_CollisionTrigger = data.GetBool("CollisionTrigger");
		m_TriggerSkill = data.GetString("TriggerSkill");
		m_AcrossEnemy = data.GetBool("AcrossEnemy");
		m_HitSFX = data.GetString("HitSFX");
		m_HurtBuff = data.GetStringArray("HurtBuff");
		m_BlowFlyID = data.GetString("BlowFlyID");
	}
}
