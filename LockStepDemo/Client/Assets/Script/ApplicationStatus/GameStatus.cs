using Protocol.fightModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : IApplicationStatus
{
    PlayerControl playerControl = new PlayerControl();
    WorldBase world;
    public override void OnEnterStatus()
    {
        WorldManager.Init(200);
         world =  WorldManager.CreateWorld<DemoWorld>();

        //SendLoadingFinsih();

        //OpenUI<OperationWindow>();

        //SyncService.Init();
        //CommandRouteService.Init();
        //ItemManager.Init();

        //CameraService.Instance.m_targetID = UserData.PlayerID;
        //playerControl.m_targetCharacterID = UserData.PlayerID;
        //playerControl.Init();

        //SendJoinMsg();
        //SyncService.SendTimeSyncMessage();

        //CharacterManager.AddListener(UserData.PlayerID,CharacterEventType.Die, RecevicePlayerDie);

        //GlobalEvent.AddTypeEvent<fight_end_c>(ReceviceSettlementMsg);
        //GlobalEvent.AddTypeEvent<fight_relive_c>(RecviceReliveMsg);
    }

    public override void OnExitStatus()
    {
        Debug.Log("OnExitStatus");

        //SyncService.Dispose();
        //CommandRouteService.Dispose();
        //ItemManager.Dispose();

        ////CameraService.Instance.Dispose();
        //CameraService.Instance.m_target = null;

        //playerControl.Dispose();

        //CharacterManager.CleanCharacter();
        //FlyObjectManager.CleanFlyObject();

        //GlobalEvent.RemoveTypeEvent<fight_end_c>(ReceviceSettlementMsg);
        //GlobalEvent.RemoveTypeEvent<fight_relive_c>(RecviceReliveMsg);

        //CharacterManager.RemoveListener(UserData.PlayerID, CharacterEventType.Die, RecevicePlayerDie);
        
        WorldManager.Dispose();
    }

    public override void OnGUI()
    {

    }

    public void ChangeElement(List<int> list)
    {
        GameData.ChoiceList = list;
    }

    public void Resurgence()
    {
        PlayerResurgence_s msg = new PlayerResurgence_s();
        ProtocolAnalysisService.SendCommand(msg);
    }

    #region 消息发送
    public void SendLoadingFinsih()
    {
        fight_loading_s msg = new fight_loading_s();
        ProtocolAnalysisService.SendCommand(msg);
    }

    public static string CharacterName = "1";
    public void SendJoinMsg()
    {

    }

    #endregion

    #region 消息接收

    void ReceviceSettlementMsg(fight_end_c e,params object[] objs)
    {
        CloseUI<OperationWindow>();
        OpenUI<SettlementWindow>();
    }

    void RecviceReliveMsg(fight_relive_c e, params object[] objs)
    {
        if(e.code == ScoketDefine.SUCCESS)
        {
            CloseUI<ResurgenceWindow>();
        }
        else
        {
            Debug.LogError("RecviceReliveMsg error code : ->" + e.code);
        }
    }

    void RecevicePlayerDie(CharacterEventType eventType, CharacterBase character, params object[] args)
    {
        OpenUI<ResurgenceWindow>();
    }

    #endregion
}
