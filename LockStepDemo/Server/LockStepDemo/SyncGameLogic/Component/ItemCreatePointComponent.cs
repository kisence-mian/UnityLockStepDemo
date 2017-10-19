using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ItemCreatePointComponent : MomentComponentBase
{
    public SyncVector3 pos = new SyncVector3();
    public List<string> randomList = new List<string>();

    public int CreateTimer = 0;
    public int CreateItemID;

    public override MomentComponentBase DeepCopy()
    {
        ItemCreatePointComponent mc = new ItemCreatePointComponent();

        mc.pos = pos.DeepCopy();
        mc.randomList = randomList;

        mc.CreateTimer = CreateTimer;
        mc.CreateItemID = CreateItemID;

        return mc;
    }
}
