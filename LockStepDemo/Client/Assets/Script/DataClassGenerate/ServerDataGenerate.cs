using System;
using UnityEngine;

//ServerDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class ServerDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_Address;
	public int m_port;
	public string m_name;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("ServerData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("ServerDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_Address = data.GetString("Address");
		m_port = data.GetInt("port");
		m_name = data.GetString("name");
	}
}
