using System;
using UnityEngine;

//BraveDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class BraveDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_ModelID;
	public string m_walkAnimName;
	public string m_cloakAnimName;
	public string m_IdleAnimName;
	public string m_HurtAnimName;
	public float m_HurtAnimTime;
	public string m_DieAnimName;
	public string m_ShowAniName;
	public string m_DropOutID; //ID
	public float m_Speed;
	public float m_Radius;
	public int m_MaxHp;
	public int m_MaxMp;
	public bool m_IsOnlyDamageValue;
	public float m_BloodHight; //Y
	public string m_FootstepSFX; //脚步声

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("BraveData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("BraveDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_ModelID = data.GetString("ModelID");
		m_walkAnimName = data.GetString("walkAnimName");
		m_cloakAnimName = data.GetString("cloakAnimName");
		m_IdleAnimName = data.GetString("IdleAnimName");
		m_HurtAnimName = data.GetString("HurtAnimName");
		m_HurtAnimTime = data.GetFloat("HurtAnimTime");
		m_DieAnimName = data.GetString("DieAnimName");
		m_ShowAniName = data.GetString("ShowAniName");
		m_DropOutID = data.GetString("DropOutID");
		m_Speed = data.GetFloat("Speed");
		m_Radius = data.GetFloat("Radius");
		m_MaxHp = data.GetInt("MaxHp");
		m_MaxMp = data.GetInt("MaxMp");
		m_IsOnlyDamageValue = data.GetBool("IsOnlyDamageValue");
		m_BloodHight = data.GetFloat("BloodHight");
		m_FootstepSFX = data.GetString("FootstepSFX");
	}
}
