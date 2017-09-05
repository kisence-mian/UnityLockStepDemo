using System;
using UnityEngine;

//AreaDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class AreaDataGenerate : DataGenerateBase 
{
	public string m_key;
	public DirectionEnum m_SkewDirection; //方向
	public float m_SkewDistance; //偏移距离
	public AreaType m_Shape; //形状
	public float m_Length; //矩形长
	public float m_Width; //矩形宽
	public float m_Angle; //扇形角度
	public float m_Radius; //半径

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("AreaData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("AreaDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_SkewDirection = data.GetEnum<DirectionEnum>("SkewDirection");
		m_SkewDistance = data.GetFloat("SkewDistance");
		m_Shape = data.GetEnum<AreaType>("Shape");
		m_Length = data.GetFloat("Length");
		m_Width = data.GetFloat("Width");
		m_Angle = data.GetFloat("Angle");
		m_Radius = data.GetFloat("Radius");
	}
}
