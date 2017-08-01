using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LockStepDemo.Service
{
    public static class UpdateEngine
    {
        static int s_intervalTime;
        public static void Init(int intervalTime)
        {
            s_intervalTime = intervalTime;

            Thread t = new Thread(UpdateLogic);
            t.Start();
        }

        static void UpdateLogic()
        {

        }
    }
}
