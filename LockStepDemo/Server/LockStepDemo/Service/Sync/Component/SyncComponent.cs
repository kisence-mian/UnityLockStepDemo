using LockStepDemo.ServiceLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncComponent : ComponentBase
{
    public List<ConnectionComponent> m_waitSyncList = new List<ConnectionComponent>(); 
}
