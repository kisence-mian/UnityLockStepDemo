using CDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

public class LoginService : ServiceBase
{
    const string c_playerTableName = "PlayerTable";

    public override void OnInit()
    {
        EventService.AddTypeEvent<PlayerLoginMsg_s>(RecevicePlayerLogin);
    }

    public override void OnSessionClose(SyncSession session, CloseReason reason)
    {
        if (session.player == null)
        {
            return;
        }

        //保存玩家数据
        SavePlayerData(session.player);

        //玩家退出登陆
        m_service.OnPlayerLogout(session.player);
    }

    public void RecevicePlayerLogin(SyncSession session, PlayerLoginMsg_s e)
    {
        Debug.Log("RecevicePlayerLogin");

        if(session.player != null)
        {
            Debug.Log(""+ session.player.playerID +" 已经登录，不需要重复登录！ ");
        }

        string clauseContent = "ID ='" + e.playerID + "'";
        var result = DataBaseService.database.Query(c_playerTableName,null, clauseContent, null,null,null,null);

        if(result.MoveToNext())
        {
            Debug.Log("查询到记录！ ");

            session.player = GetOldPlayer(result);

            result.Close();
        }
        else
        {
            result.Close();
            Debug.Log("未查询到记录！");

            session.player = GetNewPlayer();

            Dictionary<string, string> value = new Dictionary<string, string>();
            value.Add("ID", e.playerID);
            DataBaseService.database.Insert(c_playerTableName, null, value);
        }

        session.player.playerID = e.playerID;
        session.player.session = session;

        PlayerLoginMsg_c msg = new PlayerLoginMsg_c();
        ProtocolAnalysisService.SendMsg(session,msg);

        //派发玩家登陆事件
        m_service.OnPlayerLogin(session.player);
    }

    Player GetOldPlayer(ICursor data)
    {
        Player player = new Player();

        player.characterID = data.GetString("CharacterID");
        player.OwnCharacter = data.GetString("OwnCharacter");
        player.nickName = data.GetString("NickName");

        return player;
    }

    Player GetNewPlayer()
    {
        Player player = new Player();

        //player.characterID = data.GetString("CharacterID");
        //player.OwnCharacter = data.GetString("OwnCharacter");
        //player.nickName = data.GetString("NickName");

        return player;
    }

    void SavePlayerData(Player player)
    {
        string clauseContent = "ID ='" + player.playerID + "'";

        Dictionary<string, string> value = new Dictionary<string, string>();
        value.Add("ID", player.playerID);
        value.Add("NickName", player.nickName);
        value.Add("CharacterID", player.characterID);
        value.Add("OwnCharacter", player.OwnCharacter);

        DataBaseService.database.Update(c_playerTableName, value, clauseContent, null);
    }
}
