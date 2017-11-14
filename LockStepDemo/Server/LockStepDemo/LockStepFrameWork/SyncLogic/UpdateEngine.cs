using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


public static class UpdateEngine
{

    public static int s_intervalTime =  100; //单位毫微秒

    /// <summary>
    ///         给外部使用的间隔时间，单位毫秒
    /// </summary>

    public static int IntervalTime
    {
        get
        {
            return s_intervalTime;
        }

        set
        {
            s_intervalTime = value ;
        }
    }

    public static void Init(int intervalTime) //单位ms
    {
        s_intervalTime = intervalTime; //毫秒 转化为100毫微秒

        Thread t = new Thread(UpdateLogic);
        t.Start();
    }

    static void UpdateLogic()
    {
        int time = ServiceTime.GetServiceTime();
        int lastTime = ServiceTime.GetServiceTime();

        while (true)
        {
            lastTime = ServiceTime.GetServiceTime();

            UpdateWorld(s_intervalTime);

            time = ServiceTime.GetServiceTime();

            int sleepTime = s_intervalTime - (time - lastTime);

            if (sleepTime > 0)
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

                if (WorldManager.WorldList[i].isFinish)
                {
                    //游戏结束
                    EventService.DispatchEvent(ServiceEventDefine.ServiceEvent.GameFinsih, WorldManager.WorldList[i]);

                    WorldManager.WorldList.RemoveAt(i);
                    i--;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateWorld Exception：" + e.ToString());
            }
        }
    }
}
