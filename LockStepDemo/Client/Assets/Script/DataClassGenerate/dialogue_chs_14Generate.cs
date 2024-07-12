using System;
using UnityEngine;

//dialogue_chs_14Generate类
//该类自动生成请勿修改，以避免不必要的损失
public class dialogue_chs_14Generate : DataGenerateBase 
{
	public string m_key;
	public string m_func; //"功能选择,dia为对话,其他待定"
	public string m_name_text_left; //左边的角色名字
	public string m_img_left; //左边的角色头像
	public string m_name_text_right; //右边的角色名字
	public string m_img_right; //右边的角色头像
	public string m_dia_text; //对话内容

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("dialogue_chs_14");

		if (!table.ContainsKey(key))
		{
			throw new Exception("dialogue_chs_14Generate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_func = data.GetString("func");
		m_name_text_left = data.GetString("name_text_left");
		m_img_left = data.GetString("img_left");
		m_name_text_right = data.GetString("name_text_right");
		m_img_right = data.GetString("img_right");
		m_dia_text = data.GetString("dia_text");
	}
}
