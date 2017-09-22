using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ClientTime
{
    public const int Tick2ms = 10000;

    public static int GetTime()
    {
        return (int)(DateTime.Now.Ticks / Tick2ms);
    }
}
