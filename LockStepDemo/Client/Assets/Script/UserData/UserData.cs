using Protocol.fightModule;
using Protocol.roleModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    private static int playerID;
    private static string nickName = "";
    private static string characterID = "1";
    private static int coinNumber;
    private static int jewelNumber;

    public static string CharacterID
    {
        get
        {
            return characterID;
        }

        set
        {
           
            characterID = value;
            GlobalEvent.DispatchEvent(UserDataChangeEvent.CharacterChange);
            SendCharacterChange(characterID);
        }
    }

    public static int PlayerID
    {
        get
        {
            return playerID;
        }

        set
        {
            GlobalEvent.DispatchEvent(UserDataChangeEvent.PlayerID);
            playerID = value;
        }
    }

    public static int CoinNumber
    {
        get
        {
            return coinNumber;
        }

        set
        {
            coinNumber = value;
        }
    }

    public static int JewelNumber
    {
        get
        {
            return jewelNumber;
        }

        set
        {
            jewelNumber = value;
        }
    }

    public static string NickName
    {
        get
        {
            return nickName;
        }

        set
        {
            nickName = value;
        }
    }

    public static void Init()
    {
        GlobalEvent.AddTypeEvent<role_attr_c>(ReceviceUserData);
    }

    public static void ReceviceUserData(role_attr_c e,params object[] obj)
    {
        PlayerID = e.role_id;
        characterID = e.hero;
    }


    #region 消息发送
    static void SendCharacterChange(string characterID)
    {
        fight_setrole_s msg = new fight_setrole_s();

        msg.id = characterID;

        ProtocolAnalysisService.SendCommand(msg);
    }

    #endregion

    public enum UserDataChangeEvent
    {
        PlayerID,
        CharacterChange,
    }
}
