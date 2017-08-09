using LockStepDemo.GameLogic.System;
using LockStepDemo.ServiceLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Service.ServiceLogic.System
{
    class PlayerInputSystem<T> :ServiceSystem where T: PlayerCommandBase,new()
    {
        public override Type[] GetFilter()
        {
            return new Type[] {
                typeof(T),
                typeof(ConnectionComponent),
            };
        }

        public override void BeforeFixedUpdate(int deltaTime)
        {
            List<EntityBase> list = GetEntityList();

            for (int i = 0; i < list.Count; i++)
            {
                ConnectionComponent comp = list[i].GetComp<ConnectionComponent>();

                if(comp.m_commandList.Count > 0)
                {
                    T cmd = (T)comp.m_commandList[0];

                    list[i].ChangeComp(cmd);
                }
            }
        }
    }
}
