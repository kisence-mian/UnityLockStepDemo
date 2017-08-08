using LockStepDemo.Service;
using LockStepDemo.Service.ServiceLogic.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.ServiceLogic
{
    class ConnectionComponent : ServiceComponent
    {
        public SyncSession m_session; 
    }
}
