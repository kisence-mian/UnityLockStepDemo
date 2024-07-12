using UnityEngine;
using System.Collections;

public class PlayerAttackEffect : StandardAttackStatus
{
    CreatMesh RangeTip;

    public override void EnterAttackBefore()
    {
        base.EnterAttackBefore();

        if (m_currentAttackData.GetInfo().m_IsAreaTip)
        {
            if (RangeTip == null)
            {
                GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
                RangeTip = tip.GetComponent<CreatMesh>();
            }
            RangeTip.gameObject.SetActive(true);
            RangeTip.SetMesh(m_character, m_currentAttackData.GetInfo().m_EffectArea, false, m_currentAttackData.GetInfo().m_AreaTexture);
        }
    }

    public override void OnAttackCurrent()
    {
        base.OnAttackCurrent();
    }

    public override void OnAttackLater()
    {
        base.OnAttackLater();

        if (RangeTip != null)
        {
            RangeTip.gameObject.SetActive(false);
        }
    }

    public override void EnterAttackNone()
    {
        if (RangeTip != null)
        {
            RangeTip.gameObject.SetActive(false);
        }

        base.EnterAttackNone();
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
