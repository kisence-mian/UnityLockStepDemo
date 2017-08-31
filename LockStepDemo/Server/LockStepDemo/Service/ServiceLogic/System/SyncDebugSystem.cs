using DeJson;
using LockStepDemo.Service.ServiceLogic.Component;
using LockStepDemo.ServiceLogic;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SyncDebugSystem : SystemBase
{
    public static bool isDebug = true;
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(ConnectionComponent)
        };
    }

    public override void NoRecalcLateFixedUpdate(int deltaTime)
    {
        if(!isDebug)
        {
            return;
        }

        DebugMsg msg = new DebugMsg();
        msg.frame = m_world.FrameCount;
        msg.infos = new List<EntityInfo>();

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            EntityBase eb = m_world.m_entityList[i];
            EntityInfo einfo = new EntityInfo();
            einfo.id = eb.ID;

            einfo.infos = new List<ComponentInfo>();

            foreach (var item in eb.m_compDict)
            {
                if(item.Value.GetType().IsSubclassOf(typeof(MomentComponentBase)))
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = item.Value.GetType().Name;
                    info.content = Serializer.Serialize(item.Value);

                    einfo.infos.Add(info);

                    if(info.m_compName == "MoveComponent" 
                        || info.m_compName == "CommandComponent")
                    {
                        //Debug.Log(".id " + einfo.id + " m_compName "+ info.m_compName + " content : " + info.content);
                    }
                }
            }

            msg.infos.Add(einfo);
        }

        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();
            ProtocolAnalysisService.SendMsg(cc.m_session, msg);
        }
    }
}

