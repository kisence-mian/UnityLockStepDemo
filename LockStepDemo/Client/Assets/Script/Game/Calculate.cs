#define DEBUG
#undef DEBUG
using System;
using UnityEngine;

/// <summary>
/// 范围类
/// </summary>
[System.Serializable]
public class Area
{
    public AreaType areaType = AreaType.Circle;
    public Vector3 position;
    public Vector3 direction;
    public float radius;

    public float length;//x轴长度
    public float Width;//z轴长度
    public float angle;


    static public Area CreatArea(string areaID, CharacterBase skiller, CharacterBase aim = null)
    {
        Area area = new Area();
        area.UpdateArea(areaID, skiller, aim);
        return area;
    }
    static public Area CreatSkillArea(string skillID, CharacterBase skiller, CharacterBase aim = null)
    {
        string effectArea = DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_EffectArea;
        return CreatArea(effectArea, skiller, aim);
    }


    public static Vector3 GetAreaDir(AreaDataGenerate areaData, CharacterBase skiller, CharacterBase aim = null)
    {
        Vector3 l_dir = Vector3.zero;
        if (aim == null)
        {
            switch (areaData.m_SkewDirection)
            {
                
                case DirectionEnum.Forward: l_dir = skiller.transform.forward; break;
                case DirectionEnum.Backward: l_dir = skiller.transform.forward * -1; break;
                case DirectionEnum.Close:
                    Debug.Log("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为forward");
                    l_dir = skiller.transform.forward; break;
                case DirectionEnum.Leave:
                    Debug.Log("没有aim，不能使用" + areaData.m_SkewDirection + "方向,修正为Backward");
                    l_dir = skiller.transform.forward * -1; break;
                default: Debug.LogError("没有aim，不能使用" + areaData.m_SkewDirection + "方向"); break;

            }
        }


        if (aim != null)
        {
            switch (areaData.m_SkewDirection)
            {
                case DirectionEnum.Forward: l_dir = skiller.transform.forward; break;
                case DirectionEnum.Backward: l_dir = skiller.transform.forward * -1; break;
                case DirectionEnum.Leave: l_dir = aim.transform.position - skiller.transform.position; break;
                case DirectionEnum.Close: l_dir = skiller.transform.position - aim.transform.position; break;
            }

        }
        return l_dir;
    }

    public void UpdateArea(string areaID, CharacterBase skiller, CharacterBase aim = null)
    {
        AreaDataGenerate areaData = DataGenerateManager<AreaDataGenerate>.GetData(areaID);
        Vector3 dir = GetAreaDir(areaData, skiller, aim);

        areaType = areaData.m_Shape;
        length = areaData.m_Length;
        Width = areaData.m_Width;
        angle = areaData.m_Angle;
        radius = areaData.m_Radius;

        direction = dir.normalized;
        position = skiller.transform.position + direction * areaData.m_SkewDistance;

        //Debug.Log( "skiller forward"+skiller.transform.forward);
    }

    public void UpdatSkillArea(string skillID, CharacterBase skiller, CharacterBase aim = null)
    {
        string effectArea = DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_EffectArea;
        UpdateArea(effectArea, skiller, aim);
    }

    /// <summary>
    /// 区域碰撞成功
    /// </summary>
    public bool AreaCollideSucceed(Area area)
    {
        area.position.y = 0;
        position.y = 0;
        switch (areaType)
        {
            case AreaType.Circle:    return Circle(area);
            case AreaType.Rectangle: return Rectangle(area);
            case AreaType.Sector:    return Sector(area);
        }

        return true;
    }

    #region 自己形状的三种情况
    //自己是圆形
    private bool Circle(Area area)
    {
        switch (area.areaType)
        {
            case AreaType.Circle: return Circle_Circle(this, area);
            case AreaType.Rectangle: return Circle_Rectangle(this,area);
            case AreaType.Sector: return Circle_Sector(this,area);
        }
        return true;
        
    }

    //自己是矩形
    private bool Rectangle(Area area)
    {

        //Debug.Log(area.position + "长 ：" + area.length + "宽： " + area.Width + "半径"+ area.radius);
        //Debug.Log(this.position + "长 ：" + this.length + "宽： " + this.Width + "forward" + direction);
        switch (area.areaType)
        {
            case AreaType.Circle: return Circle_Rectangle(area,this);
            case AreaType.Rectangle: return Rectangle_Rectangle(area,this);
            case AreaType.Sector: return Sector_Rectangle(area,this);
        }

        return true;
    }

    //自己是扇形
    private bool Sector(Area area)
    {

        switch (area.areaType)
        {
            case AreaType.Circle: return Circle_Sector(area,this);
            case AreaType.Rectangle: return Sector_Rectangle(this,area);
            case AreaType.Sector: return Sector_Sector(area,this);
        }
        return true;
    }
    #endregion

    #region 各种形状间的相交判断

    //圆——圆相交
    private bool Circle_Circle(Area area1, Area area2)
    {
        return Vector3.Distance(area1.position, area2.position) < (area1.radius + area2.radius);
    }

    //圆——矩形相交（近似）
    private bool Circle_Rectangle(Area areaCircle, Area areaRectangle)
    {
        ////先进行一次剪枝
        //if(Vector3.Distance(areaCircle.position, areaCircle.position) > (areaCircle.radius +  1.42f * (areaRectangle.Width + areaRectangle.length)))
        //{
        //    return false;
        //}

        //近似方式： 将矩形四个边扩大圆的半径，将圆缩小为一个点，然后判断这个点是否在扩大的矩形内
        return PointInRectangle(areaCircle.position, areaRectangle, areaCircle.radius * 2, areaCircle.radius * 2);
    }

    //圆——扇形相交（近似）
    private bool Circle_Sector(Area areaCircle, Area areaSector)
    {
        
        //近似方法：将扇形半径根据圆的半径进行扩大，然后判断圆心是否在扩大后的扇形内
        if (PointInSector(areaCircle.position, areaSector, areaCircle.radius))
        {
            return true;
        }

        Vector3[] sectorPoints = GetSectorPoints(areaSector);
        for (int i = 0; i < sectorPoints.Length; i++)
        {
            Vector3 dis = sectorPoints[i] - areaCircle.position;
            if (dis.magnitude < areaCircle.radius)
            {
                return true;
            }
        }


            return false;
    }

    //扇形——扇形相交（近似）
    private bool Sector_Sector(Area area1, Area area2)
    {
        
        //分别判断扇形的三个顶点以及中轴线上的N个点是否在另一个扇形内
        Vector3[] area1Points = GetSectorPoints(area1);
        Vector3[] area2Points = GetSectorPoints(area2);

        for (int i = 0; i < area1Points.Length; i++)
        {
            if (PointInSector(area1Points[i], area2))
            {
                return true;
            }
            if (PointInSector(area2Points[i], area1))
            {
                return true;
            }
        }
        return false;
    }

    //扇形——矩形相交（近似）
    private bool Sector_Rectangle(Area areaSector, Area areaRectangle)
    {
        
        //1.先判断扇形的重要点是否在矩形内
        Vector3[] l_sectorPoints = GetSectorPoints(areaSector);
        for (int i = 0; i < l_sectorPoints.Length; i++)
        {
            if (PointInRectangle(l_sectorPoints[i], areaRectangle))
            {
                //Debug.Log(l_sectorPoints[i]);
                return true;
            }
        }

        //2.再判断矩形的顶点是否在扇形内
        Vector3[] l_rectanglePoints = GetRectangle4Point(areaRectangle);
        for (int i = 0; i < l_rectanglePoints.Length; i++)
        {
            if (PointInSector(l_rectanglePoints[i], areaSector))
            {
                //Debug.Log(l_rectanglePoints[i]);
                return true;
            }
 
        }

        //3.判断矩形的长边方向的N个点是否在扇形内
        Vector3[] l_rectangleLongDirPoints = GetRectangleLongDirPoints(areaRectangle,areaSector.radius*0.3f);

        for (int i = 0; i < l_rectangleLongDirPoints.Length; i++)
        {
            if (PointInSector(l_rectangleLongDirPoints[i], areaSector))
            {
                //Debug.Log(l_rectangleLongDirPoints[i]);
                return true;
            }
        }

        return false;

    }

    //矩形——矩形相交(近似)  如果一个矩形十分狭长，并没有将另一个矩形的顶点包含在内，可能判断没有相交
    private bool Rectangle_Rectangle(Area area1, Area area2)
    {
        //三种可能

        //1.矩形中心的距离小于两矩形中心分别到达边界的距离之和
        Vector3 l_v3_pToP = area2.position - area1.position;
        float l_distance = l_v3_pToP.magnitude;

        float l_dis1 = GetRectangleCenterToPointDistance(area1, area2.position);
        float l_dis2 = GetRectangleCenterToPointDistance(area2, area1.position);

        //Debug.Log("l_distance" + l_distance);
        //Debug.Log(l_dis1);
        //Debug.Log(l_dis2);


        if ((l_dis1 + l_dis2) > l_distance)
        {
            return true;
        }


        //2.一个矩形的某个顶点在另一个矩形内；
        Vector3[] area1Points = GetRectangle4Point(area1);
        Vector3[] area2Points = GetRectangle4Point(area2);

        for (int i = 0; i < area1Points.Length; i++)
        {
            if (PointInRectangle(area1Points[i], area2))
            {
                return true;
            }
            if (PointInRectangle(area2Points[i], area1))
            {
                return true;
            }
        }

        //3.如果一个矩形十分狭长，并没有将另一个矩形的顶点包含在内

        float l_short1 = 0;
        if (area2.length < area2.Width)
        {
            l_short1 = area2.length;
        }
        else
        {
            l_short1 = area2.Width;
        }
        Vector3[] area1LongDirPoints = GetRectangleLongDirPoints(area1, l_short1);
        for (int i = 0; i < area1LongDirPoints.Length; i++)
        {
            if (PointInRectangle(area1LongDirPoints[i], area2))
            {
                return true;
            }
        }


        float l_short2 = 0;
        if (area1.length < area1.Width)
        {
            l_short2 = area2.length;
        }
        else
        {
            l_short2 = area2.Width;
        }
        Vector3[] area2LongDirPoints = GetRectangleLongDirPoints(area2, l_short2);
        for (int i = 0; i < area2LongDirPoints.Length; i++)
        {
            if (PointInRectangle(area2LongDirPoints[i], area1))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 基础判断函数

    /// <summary>
    /// 判断一个点是否在一个(可能经过扩大的)矩形内
    /// </summary>
    public bool PointInRectangle(Vector3 point, Area area,float lenghAdd = 0,float widthAdd = 0)
    {

        Vector3 l_v3_newPos = GetPointPosInRectangle(area, point);
        //Debug.Log(l_v3_newPos);

        if (Mathf.Abs( l_v3_newPos.x) < (area.length + lenghAdd) * 0.5f 
            && Mathf.Abs( l_v3_newPos.z) < (area.Width + widthAdd) * 0.5f)
        {
            //Debug.Log("点：" + point + "在矩形：" + area.position + "内");
            return true;
        }
        else
        {
            //MyDebug("原 点坐标" + point);
            //MyDebug("变幻后的点：" + l_v3_newPos + "不在矩形：" + area.position + "内");
            //MyDebug("area.length + lenghAdd" + (area.length + lenghAdd));
            //MyDebug("area.Width + widthAdd)" + (area.Width + widthAdd));
            return false;
        }
 
    }

    /// <summary>
    /// 判断一个点，是否在一个（可能扩大过的）扇形的
    /// </summary>
    private bool PointInSector(Vector3 point, Area area, float radiusAdd = 0)
    {
        radiusAdd *= 1.1f;
        Vector3 l_v3_forward = area.direction;//扇形的正方向
        Vector3 l_v3_newPos = area.position - l_v3_forward  * Mathf.Cos(area.angle * 0.5f*Mathf.Deg2Rad) * 2 * radiusAdd;//扇形新圆心
        Vector3 l_v3_v = point - l_v3_newPos;//新扇形中心指向该点
        if (l_v3_v == Vector3.zero)
        {
            return true;
        }

        float l_angle = Vector3.Angle(l_v3_forward, l_v3_v);
        float l_newRadius = area.radius + radiusAdd * Mathf.Cos(area.angle * 0.5f * Mathf.Deg2Rad) * 2 * radiusAdd;

        if (l_angle < area.angle * 0.5f || l_angle<0.05f)
        {
            if (l_v3_v.magnitude < l_newRadius)
            {
                return true;
            }
        }

        return false;

    }


    #endregion

    #region 工具函数

    
    /// <summary>
    /// 获取一个点在一个矩形内的局部坐标
    /// </summary>
    private Vector3 GetPointPosInRectangle(Area area, Vector3 point)
    {
        Vector3 m_v3_newPos = new Vector3();
        Vector3 l_v3_v = point - area.position;//矩形中心指向该点
        Vector3 l_v3_forward = area.direction; //矩形的正方向
        float l_angle = Vector3.Angle(l_v3_v, l_v3_forward);//两者夹角
        if (l_angle > 90)
        {
            l_angle = 180 - l_angle;
        }
         
        float l_magnitude = l_v3_v.magnitude;

        Vector3 l_v3_roForward = l_v3_forward.Vector3RotateInXZ(l_angle * Mathf.Rad2Deg);

        //转换为弧度
        l_angle = l_angle * Mathf.Deg2Rad;

        //判断象限，暂时不需要
        if (Vector3.Dot(l_v3_roForward, l_v3_v) < 0.9f)
        {
            l_angle = 2 * Mathf.PI - l_angle;
        }
        m_v3_newPos.x = l_magnitude * Mathf.Cos(l_angle);//点在矩形局部坐标内的X
        m_v3_newPos.z = l_magnitude * Mathf.Sin(l_angle);//点在矩形局部坐标内的Z

        return m_v3_newPos;
    }

    /// <summary>
    /// 一个矩形中心，在该中心到某点的方向上，到达边界的长度
    /// </summary>
    private float GetRectangleCenterToPointDistance(Area area,Vector3 point)
    {
 

        Vector3 l_v3_forward = area.direction;
        Vector3 l_v3_left = l_v3_forward.Vector3RotateInXZ(90);

        Vector3 l_v3_v1 = point - area.position;
        //Debug.Log(l_v3_v1);

        Vector3 l_v3_toAcme1 = l_v3_forward * area.length * 0.5f + l_v3_left * area.Width * 0.5f;
        //Debug.Log(l_v3_toAcme1);

        float l_acmeAngle1 = Vector3.Angle(l_v3_toAcme1, l_v3_forward)*Mathf.Deg2Rad;
        l_acmeAngle1 = Rad2FirstQuartile(l_acmeAngle1);
        //Debug.Log(l_acmeAngle1);

        float l_vAngle1 = Vector3.Angle(l_v3_v1, l_v3_forward) * Mathf.Deg2Rad;   //Mathf.Acos(Vector3.Dot(l_v3_v1.normalized, l_v3_forward.normalized));
        //Debug.Log(l_v3_v1);
        //Debug.Log(l_v3_forward);
        //Debug.Log(l_vAngle1);
        l_vAngle1 = Rad2FirstQuartile(l_vAngle1);
        //Debug.Log(l_vAngle1);

        float l_dis = 0;
        if (l_acmeAngle1 > l_vAngle1)
        {
            l_dis = (area.length * 0.5f / Mathf.Cos(l_vAngle1));
            //Debug.Log(l_dis);
        }
        else
        {
            l_dis = (area.Width * 0.5f / Mathf.Sin(l_vAngle1));
            //Debug.Log(area.Width * 0.5f);
            //Debug.Log(Mathf.Cos(l_vAngle1));
            //Debug.Log(l_dis);
        }

        return l_dis;
 
    }

    /// <summary>
    /// 获取矩形四个顶点
    /// </summary>
    private Vector3[] GetRectangle4Point(Area area)
    {
        Vector3[] l_points = new Vector3[5];

        Vector3 l_v3_forward = area.direction * area.length*0.5f;
        Vector3 l_v3_back = -1 * l_v3_forward; 
        Vector3 l_v3_left = l_v3_forward.Vector3RotateInXZ(90).normalized * area.Width*0.5f;
        Vector3 l_v3_right = -1 * l_v3_left;

        l_points[0] = area.position + l_v3_forward + l_v3_left;
        l_points[1] = area.position + l_v3_forward + l_v3_right;
        l_points[2] = area.position + l_v3_back + l_v3_left;
        l_points[3] = area.position + l_v3_back + l_v3_right;
        l_points[4] = area.position;
        return l_points;
    }

    /// <summary>
    /// 获取矩形长边方向上的N个点,步长为l_short的四分之一
    /// </summary>
    private Vector3[] GetRectangleLongDirPoints(Area area,float l_short)
    {
        Vector3 l_v3_forward = area.direction * area.length*0.5f;

        Vector3 l_v3_longDir;

        float l_long1 = 0;
        if(area.length >area.Width)
        {
            l_v3_longDir = l_v3_forward;
             l_long1 = area.length;
        }
        else
        {
            l_v3_longDir = l_v3_forward.Vector3RotateInXZ(90).normalized * area.Width*0.5f;
            l_long1 = area.Width;
        }

        int num = (int)(4 * l_long1 / l_short);

        float step = l_long1 / num;
        //Debug.Log("中心线上" + num + "个点");

        Vector3[] l_points = new Vector3[num];

            
        for (int i = 0; i < num; i++)
        {
            l_points[i] = area.position - l_v3_longDir.normalized * l_long1 * 0.5f + l_v3_longDir.normalized * step * i;
        }

        return l_points;
    }

    Vector3[] areaPoints = new Vector3[9];
    /// <summary>
    /// 获取扇形重要点  (三个顶点，以及正方向上的N个点)
    /// </summary>
    private Vector3[] GetSectorPoints(Area area)
    {
        //float l_offest = area.radius * (1 - Mathf.Cos(area.angle * 0.5f * Mathf.Deg2Rad)); //弧形与三角形差距的补偿值
        float l_offest = 0;
        Vector3 l_v3_forward = area.direction; //扇形的正方向

        areaPoints[0] = area.position;
        areaPoints[1] = ExpandMethod.Vector3RotateInXZ(l_v3_forward, area.angle * 0.5f).normalized * (area.radius + l_offest) + area.position;
        areaPoints[2] = ExpandMethod.Vector3RotateInXZ2(l_v3_forward, area.angle * 0.5f).normalized * (area.radius + l_offest) + area.position;

        float step = 1 / (float)(6);
        for (int i = 3; i < 9; i++)
        {
            areaPoints[i] = area.position + area.direction * area.radius * (step * (i-2));
        }

        return areaPoints;
    }

    /// <summary>
    /// 获取一个弧度转换为第一象限内同等大小角度的弧度
    /// </summary>
    private float Rad2FirstQuartile(float rad)
    {
        if(rad <0)
        {
            rad *= -1;
        }
        if (rad > (Math.PI * 1.5f))
        {
            return (float)(2* Mathf.PI -  rad );
        }
        if (rad > Math.PI)
        {
            return (float)(rad - Math.PI);
        }
        if (rad > (Math.PI * 0.5f))
        {
            return (float)(Math.PI - rad);
        }
        else
        {
            return rad;
        }
    }



    #endregion

    #region 暂时保留在这里的测试函数
    //Area c1 = new Area();
    //    c1.radius = 2;
    //    c1.areaType = AreaType.Circle;
    //    c1.position = new Vector3(-3,0,0);


    //    Area r1 = new Area();
    //    r1.length = 2*1.4f;
    //    r1.Width = 1.4f;
    //    r1.areaType = AreaType.Rectangle;
    //    r1.position = new Vector3(-0.5f, 0, 1.5f);
    //    r1.direction = new Vector3(0, 45, 0);


    //    Area r2 = new Area();
    //    r2.length = 2 ;
    //    r2.Width = 1f;
    //    r2.areaType = AreaType.Rectangle;
    //    r2.position = new Vector3(0, 0, 0);
    //    r2.direction = new Vector3(0, 0, 0);

    //    Area s1 = new Area();
    //    s1.radius = 1.2f;
    //    s1.angle = 90;
    //    s1.areaType = AreaType.Sector;
    //    s1.position = Vector3.zero;
    //    s1.direction = new Vector3(0,75,0);

    //    //Debug.Log(c1.GetIsArea(r1));
    //    //Vector3 v3_1 = new Vector3(-1, 0, 0);
    //    //Debug.Log(v3_1.ToString()+r1.PointInRectangle(v3_1, r1));

    //    //Vector3 v3_2 = new Vector3(-1, 0, 0.2f);
    //    //Debug.Log(v3_2.ToString() + r1.PointInRectangle(v3_2, r1));

    //    //Vector3 v3_3 = new Vector3(0.5f, 0, 1.7f);
    //    //Debug.Log(v3_3.ToString() + r1.PointInRectangle(v3_3, r1));


    //    //Debug.Log(c1.GetIsArea(s1));

    //    Area s2 = new Area();
    //    s2.radius = 1.3f;
    //    s2.angle = 90;
    //    s2.areaType = AreaType.Sector;
    //    s2.position = new Vector3(0, 0,-1); 
    //    s2.direction = new Vector3(0, 0, 0);

    //    //Debug.Log(s1.GetIsArea(s2));
    //    Debug.Log(s2.AreaCollideSucceed(r2));
    //Debug.Log(r1.AreaCollideSucceed(r2));
    #endregion
}

public enum AreaType
{
    Circle,
    Rectangle,
    Sector,
}
