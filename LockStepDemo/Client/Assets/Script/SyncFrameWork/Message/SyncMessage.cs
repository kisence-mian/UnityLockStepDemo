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

    public class SyncEntityMsg : SyncModule
    {
        public int frame;
        public int id;
        public List<ComponentInfo> infos;
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
