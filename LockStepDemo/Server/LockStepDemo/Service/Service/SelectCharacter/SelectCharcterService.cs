using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SelectCharcterService : ServiceBase
{
    public override void OnInit()
    {
        EventService.AddTypeEvent<PlayerSelectCharacter_s>(ReceviceSelectCharacter);
    }

    public void ReceviceSelectCharacter(SyncSession session,PlayerSelectCharacter_s msg)
    {
        if(session.player == null)
        {
            Debug.LogError("玩家未登录");
        }

        //判断该角色是否在玩家的拥有角色列表

        //如果是，替换玩家角色

        session.player.characterID = msg.characterID;
        PlayerSelectCharacter_c m = new PlayerSelectCharacter_c();


        ProtocolAnalysisService.SendMsg(session,m);
    }
}
