using Protocol.fightModule;
using Protocol.roleModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        GameData.Init();

        OpenUIAsync<MainWindow>();
        UserData.Init();

        //监听创建角色事件
        //GlobalEvent.AddTypeEvent <role_create_c>( ReceviceCreateMsg);
        GlobalEvent.AddTypeEvent<PlayerLoginMsg_c>(ReceviceLoginMsg);
        GlobalEvent.AddTypeEvent<PlayerMatchMsg_c>(ReceviceMatchMsg);
        GlobalEvent.AddTypeEvent<fight_loading_c>(ReceviceLoadMsg);
        GlobalEvent.AddTypeEvent<PlayerRename_c>(ReceviceRenameMsg);

        //监听内部事件
        Login();
    }

  

    public override void OnExitStatus()
    {
        base.OnExitStatus();

        GlobalEvent.RemoveTypeEvent<role_create_c>(ReceviceCreateMsg);
        GlobalEvent.RemoveTypeEvent<PlayerLoginMsg_c>(ReceviceLoginMsg);
        GlobalEvent.RemoveTypeEvent<PlayerMatchMsg_c>(ReceviceMatchMsg);
        GlobalEvent.RemoveTypeEvent<fight_loading_c>(ReceviceLoadMsg);
        GlobalEvent.RemoveTypeEvent<PlayerRename_c>(ReceviceRenameMsg);
    }

    void Login()
    {
        PlayerLoginMsg_s msg = new PlayerLoginMsg_s();
        msg.playerID = "haha" + RandomService.GetRand(0, 90000);

        ProtocolAnalysisService.SendCommand(msg);
    }

    #region 操作事件

    public void Match()
    {
        PlayerMatchMsg_s msg = new PlayerMatchMsg_s();
        msg.isCancel = false;
        ProtocolAnalysisService.SendCommand(msg);
      
       // OpenUI<WaitMatchWindow>();
    }

    public void CenelMatch()
    {
        PlayerMatchMsg_s msg = new PlayerMatchMsg_s();
        msg.isCancel = true;
        ProtocolAnalysisService.SendCommand(msg);
    
      //  CloseUI<WaitMatchWindow>();
    }

    public void SwitchSelectWindow(bool isOpen)
    {
        if(isOpen)
        {
            OpenUI<SelectCharacterWindow>();
        }
        else
        {
            CloseUI<SelectCharacterWindow>();
        }
    }

    public void Rename(string newName)
    {
        PlayerRename_s msg = new PlayerRename_s();
        msg.newName = newName;
        ProtocolAnalysisService.SendCommand(msg);
    }
    #endregion

    #region 消息接收

    void ReceviceLoginMsg(PlayerLoginMsg_c e,params object[] obj)
    {
        Debug.Log("ReceviceLoginMsg");

        if (e.code0 == ScoketDefine.SUCCESS)
        {
            
        }
        else
        {
            role_create_s msg = new role_create_s();

            msg.nick = "laofan" + RandomService.GetRand(0, 90000);
            msg.sex = ScoketDefine.MAN;
            msg.model_id = "1";
            msg.head = "hello";

            ProtocolAnalysisService.SendCommand(msg);
        }
    }

    void ReceviceCreateMsg(role_create_c e, params object[] obj)
    {
        if (e.code == ScoketDefine.SUCCESS)
        {
            //Debug.Log("ReceviceCreateMsg code :" + e.code);
        }
        else
        {
            Debug.LogError("ReceviceCreateMsg error code :" + e.code);
        }
    }

    void ReceviceMatchMsg(PlayerMatchMsg_c e,params object[] objs)
    {
        if(e.isMatched)
        {
           
            ApplicationStatusManager.EnterStatus<GameStatus>();
          
        }
        else
        {
         
            //Debug.LogError("ReceviceMatchMsg error code :");
        }
    }

    void ReceviceLoadMsg(fight_loading_c e, params object[] objs)
    {
         ApplicationStatusManager.EnterStatus<GameStatus>();
    }
    private void ReceviceRenameMsg(PlayerRename_c e, object[] args)
    {
        if (e.code == ScoketDefine.SUCCESS)
        {
            UserData.NickName = e.newName;
        }
    }
    #endregion


}
