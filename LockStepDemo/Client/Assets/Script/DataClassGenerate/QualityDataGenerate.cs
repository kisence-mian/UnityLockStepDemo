using System;
using UnityEngine;

//QualityDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class QualityDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_qualityName;
	public string m_fxName; //掉落特效名称

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("QualityData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("QualityDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_qualityName = data.GetString("qualityName");
		m_fxName = data.GetString("fxName");
	}
}
