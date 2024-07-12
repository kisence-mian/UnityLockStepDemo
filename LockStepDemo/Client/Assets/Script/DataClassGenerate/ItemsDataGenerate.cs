using System;
using UnityEngine;

//ItemsDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class ItemsDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_name; //名字
	public string m_info; //物品说明
	public string m_item_type; //类别
	public string m_icon; //图标
	public int m_att; //攻击
	public int m_hp; //血量
	public float m_movespeed; //移动速度
	public int m_resistdamage; //抵挡伤害
	public int m_invincible; //无敌
	public bool m_noclip; //穿墙
	public float m_time; //持续时间
	public int m_chipid; //碎片
	public int m_chipnum; //碎片数量
	public int m_addatt; //增加攻击力
	public int m_addhp; //增加血量上限
	public int m_addmovespeed; //增加移速
	public int m_adddef; //增加防御
	public string m_modelid; //模型ID
	public string m_pickupEffect; //捡的动作
	public string m_Qualityframe; //品质边框id
	public int m_quality; //1白，2蓝，3紫，4金，5橙

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("ItemsData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("ItemsDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_name = data.GetString("name");
		m_info = data.GetString("info");
		m_item_type = data.GetString("item_type");
		m_icon = data.GetString("icon");
		m_att = data.GetInt("att");
		m_hp = data.GetInt("hp");
		m_movespeed = data.GetFloat("movespeed");
		m_resistdamage = data.GetInt("resistdamage");
		m_invincible = data.GetInt("invincible");
		m_noclip = data.GetBool("noclip");
		m_time = data.GetFloat("time");
		m_chipid = data.GetInt("chipid");
		m_chipnum = data.GetInt("chipnum");
		m_addatt = data.GetInt("addatt");
		m_addhp = data.GetInt("addhp");
		m_addmovespeed = data.GetInt("addmovespeed");
		m_adddef = data.GetInt("adddef");
		m_modelid = data.GetString("modelid");
		m_pickupEffect = data.GetString("pickupEffect");
		m_Qualityframe = data.GetString("Qualityframe");
		m_quality = data.GetInt("quality");
	}
}
