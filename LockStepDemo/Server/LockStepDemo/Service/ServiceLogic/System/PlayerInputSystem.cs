using Protocol;
using System;
using System.Collections.Generic;


public class PlayerInputSystem : ServiceSystem /*where T : PlayerCommandBase, new()*/
{
    public override Type[] GetFilter()
    {
        return new Type[] {
                typeof(CommandComponent),
                typeof(ConnectionComponent),
            };
    }

    public override void NoRecalcBeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();
        CommandMsg msg = new CommandMsg();
        msg.frame = m_world.FrameCount;
        msg.msg = new List<CommandInfo>();
        msg.serverTime = ServiceTime.GetServiceTime();

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();

            CommandComponent cmd = (CommandComponent)comp.GetCommand(m_world.FrameCount);
            cmd.id = list[i].ID;
            cmd.frame = m_world.FrameCount;
            CommandInfo info = new CommandInfo();
            info.FromCommand( cmd);
            msg.msg.Add(info);

            list[i].ChangeComp(cmd);
        }

        //派发消息
        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
            ProtocolAnalysisService.SendMsg(comp.m_session , msg);
        }
    }
}
