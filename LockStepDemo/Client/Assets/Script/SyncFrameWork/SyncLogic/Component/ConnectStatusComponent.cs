using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConnectStatusComponent :SingletonComponent
{
    public int rtt = 0;
    public List<int> unConfirmFrame = new List<int>(); //未确认的消息
    //public List<int> confirmFrame = new List<int>();   //已确认的消息
    public int aheadFrame = -1;
    public int ClearFrame = -1;
    public int confirmFrame = -1;
}
