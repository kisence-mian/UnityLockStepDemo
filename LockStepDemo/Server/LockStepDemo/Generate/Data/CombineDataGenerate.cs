using System;
using UnityEngine;

//CombineDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class CombineDataGenerate : DataGenerateBase 
{
	public string m_key;
	public int m_ele_1; //元素1
	public int m_ele_2; //元素2

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("CombineData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("CombineDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_ele_1 = data.GetInt("ele_1");
		m_ele_2 = data.GetInt("ele_2");
	}
}
