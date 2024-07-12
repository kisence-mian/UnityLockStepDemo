using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameDataCacheComponent :SingletonComponent
{
    //没有处理的其他玩家输入
    //public List<PlayerCommandBase> m_noExecuteCommandList = new List<PlayerCommandBase>();

    public List<CommandMsg> m_noExecuteCommandList = new List<CommandMsg>();
}
