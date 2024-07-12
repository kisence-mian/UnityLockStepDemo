using System;
using UnityEngine;

//LangData_ChineseGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class LangData_ChineseGenerate : DataGenerateBase 
{
	public string m_key;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("LangData_Chinese");

		if (!table.ContainsKey(key))
		{
			throw new Exception("LangData_ChineseGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
	}
}
