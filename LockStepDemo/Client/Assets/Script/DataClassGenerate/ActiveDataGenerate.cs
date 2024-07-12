using System;
using UnityEngine;

//ActiveDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class ActiveDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_Incident; //活跃度事件
	public string m_Icon; //事情Icon
	public int m_Number; //事件次数
	public int m_Active; //单次活跃度获得数量

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("ActiveData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("ActiveDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_Incident = data.GetString("Incident");
		m_Icon = data.GetString("Icon");
		m_Number = data.GetInt("Number");
		m_Active = data.GetInt("Active");
	}
}
