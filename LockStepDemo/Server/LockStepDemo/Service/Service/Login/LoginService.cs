using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LoginService
{
    const string c_tableName = "PlayerTable";

    public void Init()
    {
        EventService.AddTypeEvent<PlayerLoginMsg_s>(OnPlayerLogin);
    }

    public void OnPlayerLogin(SyncSession session, PlayerLoginMsg_s e)
    {
        if(session.player != null)
        {
            Debug.Log(""+ session.player.ID +" 已经登录，不需要重复登录！ ");
        }

        string clauseContent = "ID ='" + e.playerID + "'";
        var result = DataBaseService.database.Query(c_tableName,null, clauseContent, null,null,null,null);

        if(result.MoveToNext())
        {
            Debug.Log("查询到记录！ ");

            result.Close();
        }
        else
        {
            result.Close();
            Debug.Log("未查询到记录！");

            Dictionary<string, string> value = new Dictionary<string, string>();
            value.Add("ID", e.playerID);
            DataBaseService.database.Insert(c_tableName, null, value);
        }

        session.player = new Player();
        session.player.ID = e.playerID;
        session.player.session = session;

        PlayerLoginMsg_c msg = new PlayerLoginMsg_c();
        ProtocolAnalysisService.SendMsg(session,msg);

        EventService.DispatchTypeEvent(session, session.player);
    }
}
