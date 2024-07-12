using System;
using UnityEngine;

//MapInfoDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class MapInfoDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_name; //地图名字
	public string m_info; //地图说明
	public string m_playmode; //玩法类别
	public string m_playModeName; //玩法名字
	public string m_modelicon; //玩法图标
	public string m_pic; //显示图片
	public int m_players; //需求玩家
	public int m_needpower; //需要体力
	public int m_gold; //奖励金币
	public int m_diamonds; //奖励钻石
	public int m_exp; //奖励经验
	public string m_drop1; //掉落展示1
	public string m_drop2; //掉落展示2
	public string m_drop3; //掉落展示3
	public string m_drop4; //掉落展示4
	public string m_drop5; //掉落展示5
	public string m_dropA; //A点掉落包
	public string m_dropB; //B点掉落包
	public string m_dropC; //C点掉落包
	public string m_dropD; //D点掉落包
	public string m_dropE; //E点掉落包
	public string m_dropF; //F点掉落包
	public string m_dropG; //G点掉落包
	public string m_dropH; //H点掉落包
	public string m_dropI; //I点掉落包

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("MapInfoData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("MapInfoDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_name = data.GetString("name");
		m_info = data.GetString("info");
		m_playmode = data.GetString("playmode");
		m_playModeName = data.GetString("playModeName");
		m_modelicon = data.GetString("modelicon");
		m_pic = data.GetString("pic");
		m_players = data.GetInt("players");
		m_needpower = data.GetInt("needpower");
		m_gold = data.GetInt("gold");
		m_diamonds = data.GetInt("diamonds");
		m_exp = data.GetInt("exp");
		m_drop1 = data.GetString("drop1");
		m_drop2 = data.GetString("drop2");
		m_drop3 = data.GetString("drop3");
		m_drop4 = data.GetString("drop4");
		m_drop5 = data.GetString("drop5");
		m_dropA = data.GetString("dropA");
		m_dropB = data.GetString("dropB");
		m_dropC = data.GetString("dropC");
		m_dropD = data.GetString("dropD");
		m_dropE = data.GetString("dropE");
		m_dropF = data.GetString("dropF");
		m_dropG = data.GetString("dropG");
		m_dropH = data.GetString("dropH");
		m_dropI = data.GetString("dropI");
	}
}
