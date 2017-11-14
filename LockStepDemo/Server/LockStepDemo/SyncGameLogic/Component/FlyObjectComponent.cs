using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FlyObjectComponent : MomentComponentBase
{
    public int createrID;
    public int damage;
    public string flyObjectID;
    public List<int> damageList = new List<int>();

    FlyDataGenerate flyData;

    public FlyDataGenerate FlyData
    {
        get
        {
            if(flyData == null)
            {
                flyData = DataGenerateManager<FlyDataGenerate>.GetData(flyObjectID);
            }

            return flyData;
        }

        set
        {
            flyData = value;
        }
    }

    public override MomentComponentBase DeepCopy()
    {
        FlyObjectComponent flyObjCom = new FlyObjectComponent();

        flyObjCom.createrID = createrID;
        flyObjCom.damage = damage;
        flyObjCom.flyObjectID = flyObjectID;

        for (int i = 0; i < damageList.Count; i++)
        {
            flyObjCom.damageList.Add(damageList[i]);
        }

        return flyObjCom;
    }
}
