using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.GameLogic
{
    class DemoWorld :WorldBase
    {
        public override Type[] GetSystemTypes()
        {
            return new Type[] { typeof(CountSystem)};
        }
    }
}
