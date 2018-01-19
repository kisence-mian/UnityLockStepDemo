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
    public static bool isFlyObject = false;

    public static string[] DebugFilter = new string[] {
        //"LifeSpanComponent",
        //"MoveComponent",
        //"PlayerComponent",
        //"LifeComponent",
        //"SkillStatusComponent",
        //"BlowFlyComponent",
        //"FlyObjectComponent",
        "GrowUpComponent",
        "AIComponent",
    };

    public static string[] SingleCompFilter = new string[] { "MapGridStateComponent",/* "LogicRuntimeMachineComponent" */};

    public static string syncLog = "";

    static Dictionary<string, string> debugContent = new Dictionary<string, string>();

    public static StringBuilder msgCache = new StringBuilder();

    public override Type[] GetFilter()
    {
        return new Type[] {
            
            typeof(ConnectionComponent)
            
        };
    }

    public static void AddDebugMsg(string msg)
    {
        if (!isDebug)
            return;

        msgCache.Append(msg + "\n");
    }

    Deserializer des = new Deserializer();

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
            msgCache.Append( m_world.m_entityList[i].ID + "\n");
        }

        msg.msg = msgCache.ToString();

        msgCache.Clear();

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            EntityBase eb = m_world.m_entityList[i];

            bool isFilter = false;
            if (isPlayerOnly
                 && !eb.GetExistComp(ComponentType.ConnectionComponent))
            {
                isFilter = true;
            }

            if (isFlyObject
                 && !eb.GetExistComp(ComponentType.FlyObjectComponent))
            {
                isFilter = true;
            }

            if(isFilter)
            {
                continue;
            }

            EntityInfo einfo = new EntityInfo();
            einfo.id = eb.ID;

            einfo.infos = new List<ComponentInfo>();

            foreach (var item in eb.comps)
            {
                if (item == null)
                    continue;

                if (item.GetType().IsSubclassOf(typeof(PlayerCommandBase)))
                {
                    CommandComponent cc = (CommandComponent)item;
                    ComponentInfo info = new ComponentInfo();
                    cc.time = 0;
                    cc.id = eb.ID;
                    cc.frame = m_world.FrameCount;
                    info.m_compName = item.GetType().Name;
                    info.content = Serializer.Serialize(item);

                    einfo.infos.Add(info);
                }
                else if(IsFilter(item.GetType().Name))
                {
                    ComponentInfo info = new ComponentInfo();
                    info.m_compName = item.GetType().Name;
                    info.content = Serializer.Serialize(item);

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

            //if(info.m_compName == "MapGridStateComponent")
            //{
            //    MapGridStateComponent msc = des.Deserialize<MapGridStateComponent>(info.content);
            //}

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
        if (!isDebug)
            return;

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
        if (!isDebug)
            return;

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

    public static void RecordGrowUpChange(int frame, string log)
    {
        string key = "GrowUpChange";
        string content = "";

        if (debugContent.ContainsKey(key))
        {
            content = debugContent[key];
        }
        else
        {
            debugContent.Add(key, content);
        }

        content += "\nframe " + frame + " -> " + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key] = (content);
    }

    public static void RecordMsgByStackTrace(string key, int frame, string log)
    {
        string content = "";

        if (debugContent.ContainsKey(key))
        {
            content = debugContent[key];
        }
        else
        {
            debugContent.Add(key, content);
        }

        content += "\nframe " + frame + " -> " + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key] = content;
    }

    public static void OutPutDebugRecord()
    {
        foreach (var item in debugContent)
        {
            FileTool.WriteStringByFile(ProtocolTool.ProjectPath + "/OutPut/" + item.Key + ".txt", item.Value);
        }
    }
}

