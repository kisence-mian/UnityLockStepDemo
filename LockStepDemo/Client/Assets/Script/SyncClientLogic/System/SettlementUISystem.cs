using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementUISystem : SystemBase
{

    public override void Init()
    {
        m_world.eventSystem.AddListener(GameUtils.c_gameFinish, OnGameFinish);
    }

    public override void Dispose()
    {
        if(UIManager.GetUI<SettlementWindow>() != null)
        {
            UIManager.CloseUIWindow<SettlementWindow>();
        }
    }

    public void OnGameFinish(EntityBase entity,params object[] pbjs)
    {
        if (UIManager.GetUI<SettlementWindow>() == null)
        {
            UIManager.OpenUIWindow<SettlementWindow>();
        }
    }
}
