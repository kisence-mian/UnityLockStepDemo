using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using System.Threading;

/// <summary>
/// 提供报时服务
/// </summary>
public class TimerService : ServiceBase
{
    Timer timer;
    int m_secondCount = 0;

    protected override void OnInit(IServerConfig config)
    {
        timer = new Timer(
            new TimerCallback(ReceviceTimerTick), null,
            0, 100);//100ms定时器  
    }

    void ReceviceTimerTick(object state)
    {
        EventService.DispatchEvent(TimerEnum.HundredMillisecond);

        m_secondCount++;

        if(m_secondCount == 10)
        {
            m_secondCount = 0;
            EventService.DispatchEvent(TimerEnum.Second);
        }
    }
}

public enum TimerEnum
{
    HundredMillisecond,
    Second,
    Day,
    Week,
    Month,
    Year,
}
