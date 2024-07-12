using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillEffect : SkillStatus
{

    CreatMesh RangeTip;

    public override void EnterSkillBefore()
    {
        base.EnterSkillBefore();

        if (m_currentSkillData.GetInfo().m_IsAreaTip)
        {
            if (RangeTip == null)
            {
                GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
                RangeTip = tip.GetComponent<CreatMesh>();
            }
            RangeTip.gameObject.SetActive(true);
            RangeTip.SetMesh(m_character, m_currentSkillData.GetInfo().m_EffectArea, true, m_currentSkillData.GetInfo().m_AreaTexture);
        }
    }

    public override void EnterSkillLater()
    {
        base.EnterSkillLater();
        if (RangeTip != null)
        {
            RangeTip.gameObject.SetActive(false);
        }
    }

    public override void OnExitStatus()
    {
        base.OnExitStatus();
        if (RangeTip != null)
        {
            GameObjectManager.DestroyGameObjectByPool(RangeTip.gameObject);
            RangeTip = null;
        }
    }
}
