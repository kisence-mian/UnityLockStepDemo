using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnectSystem : SystemBase
{
    public override void Init()
    {
        InputManager.AddListener<InputNetworkConnectStatusEvent>(ReceviceSocketStatus);
    }

    public override void Dispose()
    {
        InputManager.RemoveListener<InputNetworkConnectStatusEvent>(ReceviceSocketStatus);
    }

    bool isConnect = false;
    int timer = 50; //延迟重连

    public override void FixedUpdate(int deltaTime)
    {
        if(!NetworkManager.IsConnect)
        {
            //暂停游戏
            m_world.IsStart = false;
        }
    }

    public override void RunByPause()
    {
        if (!isConnect)
        {
            timer--;
            if (timer < 0)
            {
                timer = 50;
                isConnect = true;
                NetworkManager.Connect();
            }

        }
    }

    void ReceviceSocketStatus(InputNetworkConnectStatusEvent e)
    {
        isConnect = false;
        if (e.m_status == NetworkState.Connected)
        {
            ApplicationStatusManager.GetStatus<LoginState>().Login(SystemInfo.deviceUniqueIdentifier, UserData.NickName);
        }
    }
}
