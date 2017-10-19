using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class Calc  {

}

public struct SyncVector3 :IProtocolStructInterface
{
    public int x;
    public int y;
    public int z;

    public Vector3 ToVector()
    {
        return new Vector3(x * 0.001f, y * 0.001f, z * 0.001f);
    }

    public void FromVector(Vector3 v)
    {
        x = (int)(v.x * 1000);
        y = (int)(v.y * 1000);
        z = (int)(v.z * 1000);
    }

    public SyncVector3 DeepCopy()
    {
        SyncVector3 sv = new SyncVector3();
        sv.x = x;
        sv.y = y;
        sv.z = z;

        return sv;
    }

    public  bool Equals(SyncVector3 sv)
    {
        if(sv.x != x)
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
}
