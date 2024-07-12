using System;
using UnityEngine;

//StoryModeDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class StoryModeDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_mapid; //进入的地图ID
	public int m_limit_level; //进入等级限制
	public string m_dialogue; //触发的对话ID
	public string m_dialogue_after; //战斗回主城后触发的对话ID

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("StoryModeData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("StoryModeDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_mapid = data.GetString("mapid");
		m_limit_level = data.GetInt("limit_level");
		m_dialogue = data.GetString("dialogue");
		m_dialogue_after = data.GetString("dialogue_after");
	}
}
