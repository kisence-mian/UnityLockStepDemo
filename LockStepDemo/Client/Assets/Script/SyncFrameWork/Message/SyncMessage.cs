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
        public int frame;
        public int id;
        public List<ComponentInfo> infos;
    }

    public class DestroyEntityMsg : EntitySyncModule
    {
        public int frame;
        public int id;
    }

    public class ComponentInfo : IProtocolStructInterface
    {
        public string m_compName;
        public string content;
    }


    public class ChangeComponentMsg : EntitySyncModule
    {
        public int frame;
        public int id;
        public ComponentInfo info;
    }

    public class ChangeSingletonComponentMsg : EntitySyncModule
    {
        public int frame;
        public ComponentInfo info;
    }

    public enum ChangeStatus
    {
        Add,
        Remove,
        Replace
    }
}
