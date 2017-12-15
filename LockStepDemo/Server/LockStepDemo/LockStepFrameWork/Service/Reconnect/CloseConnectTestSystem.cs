using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CloseConnectTestSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(ConnectionComponent),

        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        if(m_world.FrameCount % 100 ==0)
        {
            List<EntityBase> list = GetEntityList();
            Debug.Log("close connect");

            for (int i = 0; i < list.Count;)
            {
                ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();

                if(cc.m_session != null)
                {
                    cc.m_session.Close();
                }

                break;
            }
        }
    }
}
