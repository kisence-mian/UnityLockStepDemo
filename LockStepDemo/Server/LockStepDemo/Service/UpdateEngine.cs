using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LockStepDemo.Service
{
    class UpdateEngine
    {
        public static void Init()
        {
            Thread t = new Thread(UpdateLogic);
            t.Start();
        }

        static void UpdateLogic()
        {

        }
    }
}
