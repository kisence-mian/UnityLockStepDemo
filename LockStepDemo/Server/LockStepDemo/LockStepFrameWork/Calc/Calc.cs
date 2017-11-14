using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class Calc
{

}

public struct SyncVector3 : IProtocolStructInterface
{
    public int x;
    public int y;
    public int z;

    public Vector3 ToVector()
    {
        return new Vector3(x * 0.001f, y * 0.001f, z * 0.001f);
    }

    public SyncVector3 FromVector(Vector3 v)
    {
        x = (int)(v.x * 1000);
        y = (int)(v.y * 1000);
        z = (int)(v.z * 1000);

        return this;
    }

    public SyncVector3 DeepCopy()
    {
        SyncVector3 sv = new SyncVector3();
        sv.x = x;
        sv.y = y;
        sv.z = z;

        return sv;
    }

    public bool Equals(SyncVector3 sv)
    {
        if (sv.x != x)
        {
            return false;
        }

        if (sv.y != y)
        {
            return false;
        }

        if (sv.z != z)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return "SyncV3(" + x + "," + y + "," + z + ")";
    }

    public static SyncVector3 operator +(SyncVector3 a, SyncVector3 b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;

        return result;
    }

    public static SyncVector3 operator -(SyncVector3 a, SyncVector3 b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;

        return result;
    }

    public static SyncVector3 operator *(SyncVector3 a, SyncVector3 b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x * b.x / 1000;
        result.y = a.y * b.y / 1000;
        result.z = a.z * b.z / 1000;

        return result;
    }

    public static SyncVector3 operator /(SyncVector3 a, SyncVector3 b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x / b.x * 1000;
        result.y = a.y / b.y * 1000;
        result.z = a.z / b.z * 1000;

        return result;
    }

    public static SyncVector3 operator *(SyncVector3 a, int b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x * b;
        result.y = a.y * b;
        result.z = a.z * b;

        return result;
    }

    public static SyncVector3 operator *(SyncVector3 a, float b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = (int)(a.x * b);
        result.y = (int)(a.y * b);
        result.z = (int)(a.z * b);

        return result;
    }

    public static SyncVector3 operator /(SyncVector3 a, int b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = a.x / b;
        result.y = a.y / b;
        result.z = a.z / b;

        return result;
    }

    public static SyncVector3 operator /(SyncVector3 a, float b)
    {
        SyncVector3 result = new SyncVector3();
        result.x = (int)(a.x / b);
        result.y = (int)(a.y / b);
        result.z = (int)(a.z / b);

        return result;
    }

    //逆时针旋转
    public SyncVector3 RotateInXZ(float angle)
    {
        SyncVector3 result = new SyncVector3();

        angle *= Mathf.Deg2Rad;
        result.x = (int)(x * Mathf.Cos(angle) - z * Mathf.Sin(angle));
        result.z = (int)(x * Mathf.Sin(angle) + z * Mathf.Cos(angle));

        return result;
    }

    //顺时针
    public SyncVector3 RotateInXZ2(float angle)
    {
        SyncVector3 result = new SyncVector3();
        angle *= Mathf.Deg2Rad;
        result.x = (int)(x * Mathf.Cos(angle) + z * Mathf.Sin(angle));
        result.z = (int)(-x * Mathf.Sin(angle) + z * Mathf.Cos(angle));

        return result;
    }
}
