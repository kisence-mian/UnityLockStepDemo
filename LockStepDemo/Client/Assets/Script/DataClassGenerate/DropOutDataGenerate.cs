using System;
using UnityEngine;

//DropOutDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class DropOutDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_FXName; //特效名称，非装备使用
	public int m_FXNum; //特效数目
	public float m_FXRange; //扩散范围
	public float m_FXLifeTime; //特效存活时间
	public string m_SFXName; //音效名称
	public float m_SFXDelay; //音效延迟
	public float m_OutTime; //特效飞到目的地的时间
	public float m_WaitTime; //在目的地停留时间
	public float m_BackTime; //飞回来的时间
	public float m_DeltaTime; //两个之间的间隔
	public string m_ModelName; //模型名称，非装备使用

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("DropOutData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("DropOutDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_FXName = data.GetString("FXName");
		m_FXNum = data.GetInt("FXNum");
		m_FXRange = data.GetFloat("FXRange");
		m_FXLifeTime = data.GetFloat("FXLifeTime");
		m_SFXName = data.GetString("SFXName");
		m_SFXDelay = data.GetFloat("SFXDelay");
		m_OutTime = data.GetFloat("OutTime");
		m_WaitTime = data.GetFloat("WaitTime");
		m_BackTime = data.GetFloat("BackTime");
		m_DeltaTime = data.GetFloat("DeltaTime");
		m_ModelName = data.GetString("ModelName");
	}
}
