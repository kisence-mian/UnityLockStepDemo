using Lockstep;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhyscisExpandMethod
{
    public static bool IsNotNull(this object obj)
    {
        return obj != null;
    }

    public static bool IsNull(this object obj)
    {
        return obj == null;
    }

    //向量逆时针旋转
    public static Vector2d Vector2dRotateInXZ(this Vector2d dir, long angle)
    {
        angle = angle.Mul(FixedMath.Create(Mathf.Deg2Rad));

        long cos = FixedMath.Trig.Cos(angle);
        long sin = FixedMath.Trig.Sin(angle);

        long dirX = dir.x.Mul(cos) - dir.y.Mul(sin);
        long dirY = dir.x.Mul(sin) + dir.y.Mul(cos);

        Vector2d l_dir = new Vector2d(dirX, dirY);
        return l_dir;
    }

    //向量顺时针
    public static Vector2d Vector2dRotateInXZ2(this Vector2d dir, long angle)
    {
        angle = angle.Mul(FixedMath.Create( Mathf.Deg2Rad));

        long cos = FixedMath.Trig.Cos(angle);
        long sin = FixedMath.Trig.Sin(angle);

        long l_n_dirX = dir.x.Mul(cos) + dir.y.Mul(sin);
        long l_n_dirY = -dir.x.Mul(sin) + dir.y.Mul(cos);

        Vector2d l_dir = new Vector2d(l_n_dirX,  l_n_dirY);

        return l_dir;
    }

    //位置绕点旋转顺时针，逆时针角度乘以-1即可
    public static Vector2d PostionRotateInXZ(this Vector2d pos, Vector2d center, long angle)
    {
        angle = -angle.Mul(FixedMath.Create(Mathf.Deg2Rad));

        long cos = FixedMath.Trig.Cos(angle);
        long sin = FixedMath.Trig.Sin(angle);

        long x = (pos.x - center.x).Mul(cos) - (pos.y - center.y).Mul(sin) + center.x;
        long y = (pos.x - center.x).Mul(sin) + (pos.y - center.y).Mul(cos) + center.y;

        Vector2d newPos = new Vector2d(x, y);

        return newPos;
    }

    //获取一个顺时针夹角(需先标准化向量)
    public static long GetRotationAngle(this Vector2d dir, Vector2d aimDir)
    {
        //dir = dir.normalized;
        //aimDir = aimDir.normalized;

        long angle = FixedMath.Create( Math.Acos(dir.Dot(aimDir).ToFloat())).Mul(FixedMath.Create(180).Div(FixedMath.Create(Math.PI)));

        if (angle != FixedMath.Create(180) && FixedMath.Create(angle) != 0)
        {
            long cross = dir.x.Mul(aimDir.y) - aimDir.x.Mul(dir.y);
            if (cross < FixedMath.Create(0))
            {
                return angle;
            }
            else
            {
                return FixedMath.Create(360) - angle;
            }
        }

        return angle;
    }

}
