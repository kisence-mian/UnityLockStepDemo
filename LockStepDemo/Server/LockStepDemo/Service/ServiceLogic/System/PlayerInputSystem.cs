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

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
            CommandComponent cmd = (CommandComponent)comp.GetCommand(m_world.FrameCount);
            cmd.id = list[i].ID;
            cmd.frame = m_world.FrameCount;

            list[i].ChangeComp(cmd);

            //到了这一帧还没有发送命令的，给预测一个并广播给所有前端
            if (comp.lastInputFrame < m_world.FrameCount)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    ConnectionComponent conn = list[j].GetComp<ConnectionComponent>();
                    //补发未确认的帧
                    for (int k = 0; k < conn.unConfirmFrame.Count; k++)
                    {
                        CommandComponent msg = (CommandComponent)conn.GetCommand(conn.unConfirmFrame[k]);
                        ProtocolAnalysisService.SendMsg(comp.m_session, msg);
                    }
                    
                    conn.unConfirmFrame.Add(cmd.frame);

                    ProtocolAnalysisService.SendMsg(comp.m_session, cmd);
                }
            }
        }
    }
}
