using System;
using UnityEngine;

//DropPackageDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class DropPackageDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_item1; //物品ID
	public int m_num1; //物品数量
	public int m_rate1; //物品概率
	public string m_item2; //物品ID
	public int m_num2; //物品数量
	public int m_rate2; //物品概率
	public string m_item3; //物品ID
	public int m_num3; //物品数量
	public int m_rate3; //物品概率
	public string m_item4; //物品ID
	public int m_num4; //物品数量
	public int m_rate4; //物品概率
	public string m_item5; //物品ID
	public int m_num5; //物品数量
	public int m_rate5; //物品概率
	public string m_item6; //物品ID
	public int m_num6; //物品数量
	public int m_rate6; //物品概率
	public string m_item7; //物品ID
	public int m_num7; //物品数量
	public int m_rate7; //物品概率
	public string m_item8; //物品ID
	public int m_num8; //物品数量
	public int m_rate8; //物品概率
	public string m_item9; //物品ID
	public int m_num9; //物品数量
	public int m_rate9; //物品概率
	public string m_item10; //物品ID
	public int m_num110; //物品数量
	public int m_rate10; //物品概率

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("DropPackageData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("DropPackageDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_item1 = data.GetString("item1");
		m_num1 = data.GetInt("num1");
		m_rate1 = data.GetInt("rate1");
		m_item2 = data.GetString("item2");
		m_num2 = data.GetInt("num2");
		m_rate2 = data.GetInt("rate2");
		m_item3 = data.GetString("item3");
		m_num3 = data.GetInt("num3");
		m_rate3 = data.GetInt("rate3");
		m_item4 = data.GetString("item4");
		m_num4 = data.GetInt("num4");
		m_rate4 = data.GetInt("rate4");
		m_item5 = data.GetString("item5");
		m_num5 = data.GetInt("num5");
		m_rate5 = data.GetInt("rate5");
		m_item6 = data.GetString("item6");
		m_num6 = data.GetInt("num6");
		m_rate6 = data.GetInt("rate6");
		m_item7 = data.GetString("item7");
		m_num7 = data.GetInt("num7");
		m_rate7 = data.GetInt("rate7");
		m_item8 = data.GetString("item8");
		m_num8 = data.GetInt("num8");
		m_rate8 = data.GetInt("rate8");
		m_item9 = data.GetString("item9");
		m_num9 = data.GetInt("num9");
		m_rate9 = data.GetInt("rate9");
		m_item10 = data.GetString("item10");
		m_num110 = data.GetInt("num110");
		m_rate10 = data.GetInt("rate10");
	}
}
