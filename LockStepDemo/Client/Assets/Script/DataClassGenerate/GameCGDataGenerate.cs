using System;
using UnityEngine;

//GameCGDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class GameCGDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_type; //类型
	public float m_time; //触发相对时间
	public string m_pic; //资源名字
	public string m_regid; //注册名字
	public Vector3 m_pos; //坐标
	public float m_from; //开始点
	public float m_to; //结束点
	public Vector3 m_fromv3; //开始V3
	public Vector3 m_tov3; //结束V3
	public float m_eff_time; //特效时间
	public string m_repeattype; //重复类别

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("GameCGData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("GameCGDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_type = data.GetString("type");
		m_time = data.GetFloat("time");
		m_pic = data.GetString("pic");
		m_regid = data.GetString("regid");
		m_pos = data.GetVector3("pos");
		m_from = data.GetFloat("from");
		m_to = data.GetFloat("to");
		m_fromv3 = data.GetVector3("fromv3");
		m_tov3 = data.GetVector3("tov3");
		m_eff_time = data.GetFloat("eff_time");
		m_repeattype = data.GetString("repeattype");
	}
}
