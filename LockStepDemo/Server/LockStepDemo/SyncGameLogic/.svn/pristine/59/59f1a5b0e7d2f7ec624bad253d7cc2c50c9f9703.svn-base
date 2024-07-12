using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BuffComponent : MomentComponentBase
{
    public List<BuffInfo> buffList = new List<BuffInfo>();

    public override MomentComponentBase DeepCopy()
    {
        BuffComponent bc = new BuffComponent();

        bc.buffList.Clear();

        for (int i = 0; i < buffList.Count; i++)
        {
            bc.buffList.Add(buffList[i].DeepCopy());
        }

        return bc;
    }
}

//public class BuffInfo
//{
//    public string buffID;
//    public int creater;
//    public int buffTime;
//    public int buffCount;

//    BuffDataGenerate buffData;

//    public BuffDataGenerate BuffData
//    {
//        get
//        {
//            if(buffData == null)
//            {
//                buffData = DataGenerateManager<BuffDataGenerate>.GetData(buffID);
//            }

//            return buffData;
//        }
//    }

//    public BuffInfo DeepCopy()
//    {
//        BuffInfo bi = new BuffInfo();

//        bi.buffID = buffID;
//        bi.creater = creater;
//        bi.buffTime = buffTime;
//        bi.buffCount = buffCount;

//        return bi;
//    }
//}
