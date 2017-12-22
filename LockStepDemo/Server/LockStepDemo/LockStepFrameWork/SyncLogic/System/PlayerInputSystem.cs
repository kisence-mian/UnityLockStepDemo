using DeJson;
using Protocol;
using System;
using System.Collections.Generic;

public class PlayerInputSystem<T> : ServiceSystem where T : PlayerCommandBase, new()
{
    public override Type[] GetFilter()
    {
        return new Type[] {
                typeof(T),
                typeof(ConnectionComponent),
                typeof(RealPlayerComponent),
            };
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
            comp.m_isInframe = true;

            T cmd = (T)comp.GetCommand(m_world.FrameCount);
            cmd.id = list[i].ID;
            cmd.frame = m_world.FrameCount;

            list[i].ChangeComp(cmd);

            //Debug.Log("USE cmd id "+ list[i].ID + " frame " + cmd.frame + " content " + Serializer.Serialize(cmd));

            //到了这一帧还没有发送命令的，给预测一个并广播给所有前端
            if (comp.LastInputFrame < m_world.FrameCount)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    ConnectionComponent conn = list[j].GetComp<ConnectionComponent>();
                    ProtocolAnalysisService.SendMsg(conn.m_session, cmd);
                }
            }
        }
    }

    public override void EndFrame(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();
            comp.m_isInframe = false;
        }
    }
}
