using LockStepDemo.GameLogic.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.ServiceLogic.System
{
    class ConnectSystem : ServiceSystem
    {

        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(ConnectionComponent),
                typeof(CommandComponent),
            };
        }

        public override void BeforeFixedUpdate(int deltaTime)
        {
            
        }
    }
}
