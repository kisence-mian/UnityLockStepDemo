using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSyncStatus :IApplicationStatus
{
    public override void EnterStatusTestData()
    {
        UIManager.OpenUIWindow<SelectServiceWindow>();
    }

    public override void OnGUI()
    {

    }
}
