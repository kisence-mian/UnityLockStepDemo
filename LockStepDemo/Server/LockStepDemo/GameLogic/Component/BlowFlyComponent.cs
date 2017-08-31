using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlowFlyComponent : MomentComponentBase
{
    public bool isBlow = false;

    public int blowTime = 0;
    public string blowFlyID;
    public SyncVector3 blowDir = new SyncVector3();
    ShiftDataGenerate blowData;

    public ShiftDataGenerate BlowData
    {
        get
        {
            if (blowData == null)
            {
                blowData = DataGenerateManager<ShiftDataGenerate>.GetData(blowFlyID);
            }

            return blowData;
        }

        set
        {
            blowData = value;
        }
    }

    public override MomentComponentBase DeepCopy()
    {
        BlowFlyComponent mc = new BlowFlyComponent();
        mc.blowFlyID = blowFlyID;
        mc.blowTime = blowTime;
        mc.blowDir = blowDir.DeepCopy();

        return mc;
    }

    public void SetBlowFly(Vector3 attackerPos, Vector3 selfPos, Vector3 attackerDir)
    {
        Vector3 dir = Vector3.zero;
        switch (blowData.m_Direction)
        {
            case DirectionEnum.Forward:
                dir = attackerDir;
                break;
            case DirectionEnum.Backward:
                dir = attackerDir * (-1);
                break;
            case DirectionEnum.Close:
                dir = (attackerPos - selfPos);
                dir.y = 0;
                dir = dir.normalized;
                break;
            case DirectionEnum.Leave:
                dir = (selfPos - attackerPos);
                dir.y = 0;
                dir = dir.normalized;
                break;
        }

        blowDir.FromVector(dir);
    }
}
