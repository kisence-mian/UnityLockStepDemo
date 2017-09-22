using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SkillBehaviorCompoent : MomentComponentBase
{
    public int FXTimer = 0;
    public bool isTriggerFX;

    public override MomentComponentBase DeepCopy()
    {
        SkillBehaviorCompoent c = new SkillBehaviorCompoent();

        c.FXTimer = FXTimer;
        c.isTriggerFX = isTriggerFX;

        return c;
    }
}
