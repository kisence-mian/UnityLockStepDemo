using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Protocol
{
    [Module(2, "SyncModule")]
    public abstract class SyncModule : CsharpProtocolInterface
    {
        
    }

    /// <summary>
    /// 同步帧ID和帧间隔
    /// </summary>
    public class StartSyncMsg : SyncModule
    {
        public int frame;
        public int advanceCount; //客户端提前量
        public int intervalTime;
        public int createEntityIndex;
        public SyncRule SyncRule;
    }

    public class PursueMsg : SyncModule
    {
        public int id;           //服务器时间

        public int recalcFrame;  //重计算帧
        public int frame;
        public int advanceCount; //客户端提前量
        public int serverTime;   //服务器时间

        public List<string> m_commandList;
    }

    public class SyncEntityMsg : SyncModule
    {
        public int frame;
        public List<EntityInfo> infos;

        public List<int> destroyList;
    }

    //TODO 废弃
    public class DestroyEntityMsg : SyncModule
    {
        public int frame;
        public int id;
    }

    public class ChangeComponentMsg : SyncModule
    {
        public int frame;
        public int id;
        public ComponentInfo info;
    }

    public class ChangeSingletonComponentMsg : SyncModule
    {
        public int frame;
        public ComponentInfo info;
    }

    //客户端确认消息送达，并用以计算Ping值
    public class AffirmMsg : SyncModule
    {
        public int frame;
        public int time;
    }

    public class DebugMsg : SyncModule
    {
        public int frame;
        public List<EntityInfo> infos;
    }

    public class EntityInfo : IProtocolStructInterface
    {
        public int id;
        public List<ComponentInfo> infos;
    }

    public class ComponentInfo : IProtocolStructInterface
    {
        public string m_compName;
        public string content;
    }

    public enum ChangeStatus
    {
        Add,
        Remove,
        Replace
    }
}
