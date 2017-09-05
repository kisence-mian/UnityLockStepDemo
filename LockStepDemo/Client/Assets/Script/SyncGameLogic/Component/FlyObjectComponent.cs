using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FlyObjectComponent : ComponentBase
{
    public int createrID;
    public int damage;
    public string flyObjectID;

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
}
