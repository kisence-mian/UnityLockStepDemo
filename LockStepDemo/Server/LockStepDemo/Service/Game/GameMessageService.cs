using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameMessageService
{
    public static void Init()
    {
        EventService.AddTypeEvent<PlayerResurgence_s>(OnPlayerResurgence);
    }

    public static void OnPlayerResurgence(SyncSession session, PlayerResurgence_s msg)
    {
        if(session.m_connect == null)
        {
            Debug.LogError("玩家不在游戏中！");
        }

        EntityBase entity = session.m_connect.Entity;

        LifeComponent lc = entity.GetComp<LifeComponent>();
        lc.life = lc.maxLife;

        entity.World.eventSystem.DispatchEvent(ServiceEventDefine.c_ComponentChange, entity);

        Debug.Log(" OnPlayerResurgence");
    }
}
