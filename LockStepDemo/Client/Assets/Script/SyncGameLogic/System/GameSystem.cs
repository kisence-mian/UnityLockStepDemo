using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : SystemBase
{
    public override void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        GameTimeComponent gtc = m_world.GetSingletonComp<GameTimeComponent>();

        gtc.GameTime -= deltaTime;

        //Debug.Log("gtc.GameTime " + gtc.GameTime);

        if (gtc.GameTime <0)
        {
            m_world.isFinish = true;
            m_world.IsStart = false;

            //派发游戏结束
            m_world.eventSystem.DispatchEvent(GameUtils.c_gameFinish, null);
        }
    }
}
