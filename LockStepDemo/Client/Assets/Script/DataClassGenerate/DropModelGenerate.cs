using System;
using UnityEngine;

//DropModelGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class DropModelGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_modelName;
	public string m_dropOutID;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("DropModel");

		if (!table.ContainsKey(key))
		{
			throw new Exception("DropModelGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_modelName = data.GetString("modelName");
		m_dropOutID = data.GetString("dropOutID");
	}
}
