using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ConnectStatusComponent :SingletonComponent
{
    public int rtt = 0;
    public int confirmFrame = -1;
    public int aheadFrame = -1;
}
