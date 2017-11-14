using CDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

public class LoginService : ServiceBase
{
    const string c_playerTableName = "PlayerTable";

    Dictionary<string, Player> m_onLinePlayer = new Dictionary<string, Player>();

    public override void OnInit(IServerConfig config)
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

    public override void OnPlayerLogout(Player player)
    {
        if(m_onLinePlayer.ContainsKey(player.playerID))
        {
            m_onLinePlayer.Remove(player.playerID);
        }
        else
        {
            Debug.LogError("玩家退出 该玩家不在在线玩家中");
        }
    }

    public override void OnPlayerLogin(Player player)
    {
        if (!m_onLinePlayer.ContainsKey(player.playerID))
        {
            m_onLinePlayer.Add(player.playerID,player);
        }
        else
        {
            Debug.LogError("玩家登录 该玩家已经在 在线玩家中");
        }
    }

    public void RecevicePlayerLogin(SyncSession session, PlayerLoginMsg_s e)
    {
        //玩家已登录
        if (m_onLinePlayer.ContainsKey(e.playerID))
        {
            return;
        }

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
        session.player.nickName = e.nickName;
        session.player.session = session;

        PlayerLoginMsg_c msg = new PlayerLoginMsg_c();

        msg.code = ServiceErrorCode.c_Success;
        msg.characterID  = session.player.characterID;
        msg.ownCharacter = session.player.OwnCharacter;
        msg.diamond      = session.player.Diamond;
        msg.coin         = session.player.Coin;
        ProtocolAnalysisService.SendMsg(session,msg);

        //派发玩家登陆事件
        m_service.OnPlayerLogin(session.player);
    }

    Player GetOldPlayer(ICursor data)
    {
        Player player = new Player();

        player.characterID  = data.GetString("CharacterID");
        player.OwnCharacter = StringToList(data.GetString("OwnCharacter"));
        player.nickName     = data.GetString("NickName");

        player.OwnCharacter = new List<string>() { "1" };

        player.Coin    = data.GetInt("Coin");
        player.Diamond = data.GetInt("Diamond");

        return player;
    }

    Player GetNewPlayer()
    {
        Player player = new Player();

        player.OwnCharacter = new List<string>() { "1"};
        player.Diamond = 100000;

        return player;
    }

    void SavePlayerData(Player player)
    {
        string clauseContent = "ID ='" + player.playerID + "'";

        Dictionary<string, string> value = new Dictionary<string, string>();
        value.Add("ID", player.playerID);
        value.Add("NickName", player.nickName);
        value.Add("CharacterID", player.characterID);
        value.Add("OwnCharacter", ToSaveString(player.OwnCharacter));

        value.Add("Coin", player.Coin.ToString());
        value.Add("Diamond", player.Diamond.ToString());

        DataBaseService.database.Update(c_playerTableName, value, clauseContent, null);
    }

    public string ToSaveString(List<string> content)
    {
        string result = "";

        for (int i = 0; i < content.Count; i++)
        {
            result += content[i];

            if(i != content.Count -1)
            {
                result += "|";
            }
        }

        return result;
    }

    public List<string> StringToList(string content)
    {
        List<string> list = new List<string>();

        if(content != null)
        {
            string[] tmp = content.Split('|');

            for (int i = 0; i < tmp.Length; i++)
            {
                list.Add(tmp[i]);
            }
        }


        return list;
    }
}
