using System;
using UnityEngine;

//ShopDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class ShopDataGenerate : DataGenerateBase 
{
	public string m_key;
	public int m_page; //页签1-充值、2-兑换、3-英雄、4-道具
	public int m_item_id; //售卖物品ID
	public int m_item_num; //单词售卖物品数量
	public string m_name; //英雄道具名
	public string m_info; //信息描述
	public int m_ItemType; //物品类型，0消耗类型，1非消耗类型
	public string m_IconName; //图标名称
	public string m_pay; //付款类型，可以填道具表内存在的任何实体道具或货币，比如101和102
	public int m_cost; //价格
	public int m_hot; //0
	public string m_hot_type;

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("ShopData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("ShopDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_page = data.GetInt("page");
		m_item_id = data.GetInt("item_id");
		m_item_num = data.GetInt("item_num");
		m_name = data.GetString("name");
		m_info = data.GetString("info");
		m_ItemType = data.GetInt("ItemType");
		m_IconName = data.GetString("IconName");
		m_pay = data.GetString("pay");
		m_cost = data.GetInt("cost");
		m_hot = data.GetInt("hot");
		m_hot_type = data.GetString("hot_type");
	}
}
