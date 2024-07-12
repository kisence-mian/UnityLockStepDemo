using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatService : IApplicationGlobalLogic 
{
    const float c_HeartBeatSpace = 30;
    static float s_HeartBeatTimer = 0;

    public override void Update()
    {
        s_HeartBeatTimer -= Time.deltaTime;

        if (s_HeartBeatTimer < 0)
        {
            s_HeartBeatTimer = c_HeartBeatSpace;
            SendHeartBeatMessage();
        }
    }

    void SendHeartBeatMessage()
    {
        if (NetworkManager.IsConnect)
        {
            Dictionary<string, object> data = HeapObjectPool.GetSODict();
            //NetworkManager.SendMessage("user_heartbeat", data);
        }
    }
}
