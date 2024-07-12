using System;
using UnityEngine;

//OnLineDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class OnLineDataGenerate : DataGenerateBase 
{
	public string m_key;
	public int m_time; //在线时间
	public int m_good1; //物品1
	public int m_num1; //物品1的数量
	public int m_good2; //物品2
	public int m_num2; //物品2的数量
	public int m_good3; //物品3
	public int m_num3; //物品3的数量

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("OnLineData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("OnLineDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_time = data.GetInt("time");
		m_good1 = data.GetInt("good1");
		m_num1 = data.GetInt("num1");
		m_good2 = data.GetInt("good2");
		m_num2 = data.GetInt("num2");
		m_good3 = data.GetInt("good3");
		m_num3 = data.GetInt("num3");
	}
}
