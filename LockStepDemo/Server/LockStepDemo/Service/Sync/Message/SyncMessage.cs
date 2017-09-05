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
        public int frame;
        public int advanceCount; //客户端提前量
    }

    public class SyncEntityMsg : SyncModule
    {
        public int frame;
        public List<EntityInfo> infos;

        public List<int> destroyList;
    }

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
