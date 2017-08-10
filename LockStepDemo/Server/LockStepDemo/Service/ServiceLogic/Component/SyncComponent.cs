using LockStepDemo.Service.ServiceLogic.Component;
using LockStepDemo.ServiceLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncComponent : ServiceComponent
{
    public List<ConnectionComponent> m_waitSyncList = new List<ConnectionComponent>(); 
}
