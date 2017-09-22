using System;
using UnityEngine;

//SkillStatusDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class SkillStatusDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_AnimName; //动作名字
	public float m_Time; //状态持续时间
	public float m_SFXDelay; //音效延迟
	public string m_SFXName; //音效文件
	public HardPointEnum m_FXCreatPoint; //特效创建点
	public float m_FXLifeTime; //特效存活时间
	public float m_FXDelay; //特效延迟
	public string m_FXName; //特效名称
	public HardPointEnum m_FollowFXCreatPoint; //跟随特效创建点
	public float m_FollowFXLifeTime; //特效时间
	public string m_FollowFXName; //特效名
	public string m_CameraMove; //镜头特写
	public float m_FollowFXOffSet; //跟随特效偏移
	public float m_FXOffSet; //特效偏移

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("SkillStatusData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("SkillStatusDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_AnimName = data.GetString("AnimName");
		m_Time = data.GetFloat("Time");
		m_SFXDelay = data.GetFloat("SFXDelay");
		m_SFXName = data.GetString("SFXName");
		m_FXCreatPoint = data.GetEnum<HardPointEnum>("FXCreatPoint");
		m_FXLifeTime = data.GetFloat("FXLifeTime");
		m_FXDelay = data.GetFloat("FXDelay");
		m_FXName = data.GetString("FXName");
		m_FollowFXCreatPoint = data.GetEnum<HardPointEnum>("FollowFXCreatPoint");
		m_FollowFXLifeTime = data.GetFloat("FollowFXLifeTime");
		m_FollowFXName = data.GetString("FollowFXName");
		m_CameraMove = data.GetString("CameraMove");
		m_FollowFXOffSet = data.GetFloat("FollowFXOffSet");
		m_FXOffSet = data.GetFloat("FXOffSet");
	}
}
