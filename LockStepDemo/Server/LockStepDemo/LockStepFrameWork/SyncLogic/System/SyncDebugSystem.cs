using DeJson;
using LockStepDemo.Protocol;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SyncDebugSystem : SystemBase
{
    public static bool isDebug = true;
    public static bool isPlayerOnly = true;

    public static string[] DebugFilter = new string[] { "MoveComponent", "GrowUpComponent", "CollisionComponent", "LifeComponent" };

    public static string[] SingleCompFilter = new string[] { "MapGridStateComponent" };

    public static string syncLog = "";

    static Dictionary<string, string> debugContent = new Dictionary<string, string>();

    public override Type[] GetFilter()
    {
        return new Type[] {
            
            typeof(ConnectionComponent)
        };
    }

    public override void EndFrame(int deltaTime)
    {
        if(!isDebug)
        {
            return;
        }

        DebugMsg msg = new DebugMsg();
        msg.frame = m_world.FrameCount;
        msg.seed = m_world.m_RandomSeed;
        msg.infos = new List<EntityInfo>();
        msg.singleCompInfo = new List<ComponentInfo>();

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            EntityBase eb = m_world.m_entityList[i];

            if (isPlayerOnly
                 && !eb.GetExistComp<ConnectionComponent>())
            {
                continue;
            }

            EntityInfo einfo = new EntityInfo();
            einfo.id = eb.ID;

            einfo.infos = new List<ComponentInfo>();

            foreach (var item in eb.CompDict)
            {
                if (item.Value.GetType().IsSubclassOf(typeof(PlayerCommandBase)))
                {
                    CommandComponent cc = (CommandComponent)item.Value;
                    ComponentInfo info = new ComponentInfo();
                    cc.time = 0;
                    cc.id = eb.ID;
                    cc.frame = m_world.FrameCount;
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

        for (int i = 0; i < SingleCompFilter.Length; i++)
        {
            SingletonComponent sc = m_world.GetSingletonComp(SingleCompFilter[i]);
            ComponentInfo info = new ComponentInfo();

            info.m_compName = SingleCompFilter[i];
            info.content = Serializer.Serialize(sc);

            msg.singleCompInfo.Add(info);
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
            return false;
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

    public static void RecordMsg(string key, int frame, string msg)
    {
        if (msg == null)
            return;

        string content = "";

        if (debugContent.ContainsKey(key))
        {
            content = debugContent[key];
        }
        else
        {
            debugContent.Add(key, content);
        }

        content += "\nframe " + frame + " -> " + msg;
        debugContent[key] = content;

        OutPutDebugRecord();
    }

    public static void RecordRandomChange(int frame, int seed,string log = "")
    {
        string key = "local_randomChange";
        string content = "";

        if (debugContent.ContainsKey(key))
        {
            content = debugContent[key];
        }
        else
        {
            debugContent.Add(key, content);
        }

        content += "\nframe " + frame + " -> " + seed + " content " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key] = content;

        OutPutDebugRecord();
    }

    public static void OutPutDebugRecord()
    {
        foreach (var item in debugContent)
        {
            FileTool.WriteStringByFile(ProtocolTool.ProjectPath + "/OutPut/" + item.Key + ".txt", item.Value);
        }
    }
}

