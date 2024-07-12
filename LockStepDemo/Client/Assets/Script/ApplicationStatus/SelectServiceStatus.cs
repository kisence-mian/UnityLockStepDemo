using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectServiceStatus : IApplicationStatus
{

    public override void OnEnterStatus()
    {
        //Debug.Log(MD5Tool.GetStringToHash("123456PlayerHaHa"));
        //Debug.Log(MD5Tool.GetStringToHash("1000Fly3000"));
        //Debug.Log(MD5Tool.GetStringToHash("assdf"));
        //Debug.Log(MD5Tool.GetStringToHash("200PlayerHeiHei"));

        ProtocolAnalysisService.Init();
        MsgTransverter.Init();

        InputManager.AddListener<InputNetworkConnectStatusEvent>(ReceviceSocketStatus);

        OpenUI<SelectServiceWindow>();
    }

    public override void OnExitStatus()
    {
        InputManager.RemoveListener<InputNetworkConnectStatusEvent>(ReceviceSocketStatus);
    }


    void ReceviceSocketStatus(InputNetworkConnectStatusEvent e)
    {
        if(e.m_status == NetworkState.Connected)
        {
            ApplicationStatusManager.EnterStatus<MainMenuStatus>();
        }
        else
        {
            Debug.Log(e.m_status);
        }
    }
}
