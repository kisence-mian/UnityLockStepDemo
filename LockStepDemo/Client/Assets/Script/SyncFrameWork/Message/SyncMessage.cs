using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Protocol
{
    [Module(2, "EntitySyncModule")]
    public abstract class EntitySyncModule : CsharpProtocolInterface
    {

    }

    public class SyncEntityMsg : EntitySyncModule
    {
        public int m_id;
        public List<ComponentInfo> infos;
    }

    public class DestroyEntityMsg : EntitySyncModule
    {
        public int m_id;
    }

    public class ComponentInfo : IProtocolStructInterface
    {
        public string m_compName;
        public string content;
    }


    public class ChangeComponentMsg : EntitySyncModule
    {
        public int m_id;
        public ChangeStatus m_operation;
        public ComponentInfo info;
    }

    public enum ChangeStatus
    {
        Add,
        Remove,
        Replace
    }
}
