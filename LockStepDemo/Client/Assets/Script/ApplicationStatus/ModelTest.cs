using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTest : IApplicationStatus
{
    PlayerControl playerControl = new PlayerControl();
    public override void OnEnterStatus()
    {
        OpenUI<OperationWindow>();

        SyncService.Init();
        CommandRouteService.Init();
        //SyncService.ServiceType = RouteRule.Local;
        ItemManager.Init();

        UserData.PlayerID = 1;

        CameraService.Instance.m_targetID = UserData.PlayerID;
        playerControl.m_targetCharacterID = UserData.PlayerID;

        playerControl.Init();

        CreatePlayer();
    }

    void CreatePlayer()
    {
        CreateCharacterCmd cmd = new CreateCharacterCmd();
        cmd.m_characterID = UserData.PlayerID;

        cmd.m_characterName = "3";

        cmd.m_characterType = CharacterTypeEnum.Brave;
        cmd.m_pos = new Vector3(0, 0, 0);

        CommandRouteService.SendSyncCommand(cmd);
    }
}
