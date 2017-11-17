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
        public int randomSeed;
        public SyncRule SyncRule;
    }

    public class PursueMsg : SyncModule
    {
        public int id;           //服务器时间

        public int recalcFrame;  //重计算帧
        public int frame;
        public int advanceCount; //客户端提前量
        public int serverTime;   //服务器时间

        public float updateSpeed; //客户端执行速度 1 为正常 2为两倍速

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
        public int index;
        public int time;
    }

    public class QueryCommand : SyncModule
    {
        public int frame;
        public int id;
    }

    //相同的指令发送这个消息，节约带宽
    //相同的指令发送这个消息，节约带宽
    public class SameCommand : SyncModule
    {
        public int time;
        public int frame;
        public int id;
    }

    public class DebugMsg : SyncModule
    {
        public int frame;
        public List<EntityInfo> infos;
    }

    public class VerificationMsg : SyncModule
    {
        public int frame;
        public int hash;
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
        public int index;        //消息编号
        public int serverTime;   //服务器时间
        public List<CommandInfo> msg;

        public bool GetIsExist(int frame, int id)
        {
            for (int i = 0; i < msg.Count; i++)
            {
                if (msg[i].id == id && msg[i].frame == frame)
                {
                    return true;
                }
            }

            return false;
        }
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
            isFire = comp.isFire;

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
