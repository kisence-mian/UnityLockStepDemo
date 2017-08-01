using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LockStepDemo.Service
{
    public static class UpdateEngine
    {
        const int Tick2ms = 10000;
        static long s_intervalTime = Tick2ms * 200; //单位毫微秒
        public static void Init(int intervalTime) //单位ms
        {
            s_intervalTime = Tick2ms *  intervalTime; //毫秒 转化为100毫微秒

            Thread t = new Thread(UpdateLogic);
            t.Start();
        }

        static void UpdateLogic()
        {
            long time     = DateTime.Now.Ticks;
            long lastTime = DateTime.Now.Ticks;

            while (true)
            {
                lastTime = DateTime.Now.Ticks;

                UpdateWorld((int)(s_intervalTime / Tick2ms));

                time = DateTime.Now.Ticks;

                int sleepTime = (int)((s_intervalTime - (time - lastTime)) / Tick2ms);

                if(sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }

        static void UpdateWorld(int deltaTime) 
        {
            for (int i = 0; i < WorldManager.WorldList.Count; i++)
            {
                try
                {
                    WorldManager.WorldList[i].FixedLoop(deltaTime);
                }
                catch(Exception e)
                {
                    Debug.LogError("UpdateWorld Exception："+ e.ToString());
                }
            }
        }
    }
}
