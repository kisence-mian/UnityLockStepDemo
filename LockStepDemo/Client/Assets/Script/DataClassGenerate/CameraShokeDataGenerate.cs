using System;
using UnityEngine;

//CameraShokeDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class CameraShokeDataGenerate : DataGenerateBase 
{
	public string m_key;
	public float m_TimeLength; //时长
	public float m_Amount; //振幅
	public float m_DecreaseFactor; //震动速度
	public Vector3 m_RandomOffest; //起始偏移
	public Vector3 m_weight; //权重

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("CameraShokeData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("CameraShokeDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_TimeLength = data.GetFloat("TimeLength");
		m_Amount = data.GetFloat("Amount");
		m_DecreaseFactor = data.GetFloat("DecreaseFactor");
		m_RandomOffest = data.GetVector3("RandomOffest");
		m_weight = data.GetVector3("weight");
	}
}
