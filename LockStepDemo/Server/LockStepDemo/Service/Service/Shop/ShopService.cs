using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ShopService : ServiceBase
{
    public override void OnInit()
    {
        EventService.AddTypeEvent<PlayerSelectCharacter_s>(ReceviceSelectCharacter);
        EventService.AddTypeEvent<PlayerBuyCharacter_s>(RecveviceBuyCharacter);
        EventService.AddTypeEvent<PlayerRename_s>(RecveviceRenameCharacter);
    }

    public void RecveviceBuyCharacter(SyncSession session ,PlayerBuyCharacter_s msg)
    {
        for (int i = 0; i < session.player.OwnCharacter.Count; i++)
        {
            if(session.player.OwnCharacter[i] == msg.characterID)
            {
                PlayerBuyCharacter_c result = new PlayerBuyCharacter_c();
                result.code = ServiceErrorCode.c_HasOwnCharacter;

                ProtocolAnalysisService.SendMsg(session, result);
                return;
            }
        }
        ShopDataGenerate data = DataGenerateManager<ShopDataGenerate>.GetData(msg.characterID);

        if(data.m_cost <= session.player.Diamond)
        {
            session.player.Diamond -= (int)data.m_cost;
            session.player.OwnCharacter.Add(data.m_item_id.ToString());

            PlayerBuyCharacter_c result = new PlayerBuyCharacter_c();
            result.code = ServiceErrorCode.c_Success;

            ProtocolAnalysisService.SendMsg(session, result);
        }
        else
        {
            PlayerBuyCharacter_c result = new PlayerBuyCharacter_c();
            result.code = ServiceErrorCode.c_NotEnoughDiamond;

            ProtocolAnalysisService.SendMsg(session, result);
        }
    }

    public void ReceviceSelectCharacter(SyncSession session,PlayerSelectCharacter_s msg)
    {
        if(session.player == null)
        {
            Debug.LogError("玩家未登录");
        }

        bool isHave = false;
        //判断该角色是否在玩家的拥有角色列表
        for (int i = 0; i < session.player.OwnCharacter.Count; i++)
        {
            if (session.player.OwnCharacter[i] == msg.characterID)
            {
                isHave = true;
            }
        }

        //如果是，替换玩家角色
        if(isHave)
        {
            session.player.characterID = msg.characterID;
            PlayerSelectCharacter_c m = new PlayerSelectCharacter_c();
            m.code = ServiceErrorCode.c_Success;

            ProtocolAnalysisService.SendMsg(session, m);
        }
        else
        {
            PlayerSelectCharacter_c m = new PlayerSelectCharacter_c();
            m.code = ServiceErrorCode.c_DontOwnCharacter;

            ProtocolAnalysisService.SendMsg(session, m);
        }
    }

    public void RecveviceRenameCharacter(SyncSession session, PlayerRename_s msg)
    {
        if (session.player == null)
        {
            Debug.LogError("玩家未登录");
        }

        session.player.nickName = msg.newName;

        PlayerRename_c res = new PlayerRename_c();
        res.code = ServiceErrorCode.c_Success;
        res.newName = msg.newName;

        ProtocolAnalysisService.SendMsg(session, res);
    }
}
