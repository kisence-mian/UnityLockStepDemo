using System;
using UnityEngine;

//ConstantDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class ConstantDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_StringValue;
	public float m_FloatValue;
	public Vector3 m_Vector3Value;
	public bool m_BoolValue;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("ConstantData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("ConstantDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_StringValue = data.GetString("StringValue");
		m_FloatValue = data.GetFloat("FloatValue");
		m_Vector3Value = data.GetVector3("Vector3Value");
		m_BoolValue = data.GetBool("BoolValue");
	}
}
