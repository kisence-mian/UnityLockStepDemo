using System;
using UnityEngine;

//LangData_EnglishGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class LangData_EnglishGenerate : DataGenerateBase 
{
	public string m_key;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("LangData_English");

		if (!table.ContainsKey(key))
		{
			throw new Exception("LangData_EnglishGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
	}
}
