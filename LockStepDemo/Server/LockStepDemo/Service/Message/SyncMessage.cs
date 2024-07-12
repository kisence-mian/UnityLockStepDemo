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

        //public List<string> m_commandList;
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

    //转发玩家消息
    public class CommandMsg : SyncModule
    {
        public int frame;
        public int serverTime;   //服务器时间
        public List<CommandInfo> msg;
    }

    public class CommandInfo : IProtocolStructInterface
    {
        public int frame;
        public int id;

        public SyncVector3 moveDir = new SyncVector3();
        public SyncVector3 skillDir = new SyncVector3();

        public int element1;
        public int element2;

        public bool isFire = false;

        public void FromCommand(CommandComponent comp)
        {
            moveDir = comp.moveDir.DeepCopy();
            skillDir = comp.skillDir.DeepCopy();

            element1 = comp.element1;
            element2 = comp.element2;
            isFire   = comp.isFire;

            frame = comp.frame;
            id = comp.id;
        }

        public CommandComponent ToCommand()
        {
            CommandComponent cmd = new CommandComponent();

            cmd.moveDir = moveDir.DeepCopy();
            cmd.skillDir = skillDir.DeepCopy();
            cmd.element1 = element1;
            cmd.element2 = element2;

            cmd.isFire = isFire;

            cmd.frame = frame;
            cmd.id = id;

            return cmd;
        }
    }

    public enum ChangeStatus
    {
        Add,
        Remove,
        Replace
    }
}
