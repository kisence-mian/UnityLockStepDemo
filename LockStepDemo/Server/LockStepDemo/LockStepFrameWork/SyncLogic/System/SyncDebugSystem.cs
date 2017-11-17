using DeJson;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SyncDebugSystem : SystemBase
{
    public static bool isDebug = true;

    public static string[] DebugFilter = new string[] {"MoveComponent"/*, "LifeSpanComponent"*/ , "LifeComponent" };

    public static string syncLog = "";

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
                if (item.Value.GetType().IsSubclassOf(typeof(PlayerCommandBase)))
                {
                    CommandComponent cc = (CommandComponent)item.Value;
                    ComponentInfo info = new ComponentInfo();
                    cc.time = 0;
                    info.m_compName = item.Value.GetType().Name;
                    info.content = Serializer.Serialize(item.Value);

                    einfo.infos.Add(info);
                }
                else if(IsFilter(item.Value.GetType().Name))
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = item.Value.GetType().Name;
                    info.content = Serializer.Serialize(item.Value);

                    einfo.infos.Add(info);
                }
            }

            if(einfo.infos.Count >0)
            {
                msg.infos.Add(einfo);
            }
        }

        List<EntityBase> list = GetEntityList();
        for (int i = 0; i < list.Count; i++)
        {
            ConnectionComponent cc = list[i].GetComp<ConnectionComponent>();
            ProtocolAnalysisService.SendMsg(cc.m_session, msg);
        }
    }

    public static bool IsFilter(string compName)
    {
        if (DebugFilter.Length == 0)
        {
            return true;
        }

        for (int i = 0; i < DebugFilter.Length; i++)
        {
            if (DebugFilter[i] == compName)
            {
                return true;
            }
        }

        return false;
    }
}

