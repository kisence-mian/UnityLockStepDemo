using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CDComponent : MomentComponentBase
{
    public int CD;

    public override MomentComponentBase DeepCopy()
    {
        CDComponent cc = new CDComponent();

        cc.ID = ID;
        cc.Frame = Frame;

        cc.CD = CD;

        return cc;
    }
}
