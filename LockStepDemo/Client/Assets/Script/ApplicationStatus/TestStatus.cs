using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStatus : IApplicationStatus {

    public override void OnEnterStatus()
    {
        NetworkManager.Init<ProtocolService>();
        NetworkManager.SetServer("127.0.0.1", 2012);
        NetworkManager.Connect();

        ProtocolAnalysisService.Init();

        //GlobalEvent.AddTypeEvent<role_login_c>(Recevice);

        WorldManager.Init(1000);
        WorldManager.CreateWorld<DemoWorld>();
    }
    public override void OnUpdate()
    {
        //if(NetworkManager.IsConnect)
        //{
        //    role_login_s msg = new role_login_s();
        //    ProtocolAnalysisService.SendCommand(msg);
        //}
    }

    //void Recevice(role_login_c msg,params object[] obj)
    //{
    //    Debug.Log("Recevice");
    //}
}

public class GameDataClass
{
    public int a = 1;
    public string b = "i love u";
    public List<string> list;
}

