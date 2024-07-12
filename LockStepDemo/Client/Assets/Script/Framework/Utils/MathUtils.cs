using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils  {

    /// <summary>
    /// 返回二维向量的垂直向量
    /// </summary>
    /// <param name="v2"></param>
    /// <returns></returns>
	public static Vector2[] VerticalVector2(Vector2 v2)
    {
        v2 = v2.normalized;
        Vector2[] res = new Vector2[2];
        float y = 1f / (v2.y * v2.y / (v2.x * v2.x) + 1);
        y = Mathf.Sqrt(y);
        float x = -v2.y * y / v2.x;     
        res[0] = new Vector2(x, y);
        res[1] = new Vector2(-x, -y);

        return res;
    }
}
