using FrameWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SyncDebugSystem : SystemBase
{
    public static bool isDebug = true;
    public static bool isPlayerOnly = true;

    public const string c_isAllMessage = "AllMessage";
    public const string c_isConflict   = "Conflict";
    public const string c_MissData     = "MissData";
    public const string c_Recalc       = "Recalc";

    public static string[] DebugFilter = new string[] { "MoveComponent", "GrowUpComponent" , "CollisionComponent","LifeComponent"/* "LifeSpanComponent" */};

    public static string[] SingleCompFilter = new string[] { "LogicRuntimeMachineComponent", /*"MapGridStateComponent"*/ };

    public static string syncLog = "";

    List<DebugMsg> debugList = new List<DebugMsg>();

    static Dictionary<string, string> debugContent = new Dictionary<string, string>();

    public override void Init()
    {
        //AddEntityCreaterLisnter();
        //AddEntityDestroyLisnter();

        GlobalEvent.AddTypeEvent<DebugMsg>(ReceviceDebugMsg);
    }

    public override void Dispose()
    {
        //RemoveEntityCreaterLisnter();
        //RemoveEntityDestroyLisnter();

        GlobalEvent.RemoveTypeEvent<DebugMsg>(ReceviceDebugMsg);
    }

    public override void OnEntityOptimizeCreate(EntityBase entity)
    {
        if (!isDebug)
            return;

        //string content = "EntityCreate: id:" + entity.ID + " FrameCount " + m_world.FrameCount + "\n";
        //foreach (var item in entity.CompDict)
        //{
        //    content += "" + item.Key + " " + Serializer.Serialize(item.Value) + "\n";
        //}

        ////Debug.Log(content);
    }

    //Deserializer deserializer = new Deserializer();
    public void ReceviceDebugMsg(DebugMsg msg, params object[] objs)
    {
        //Debug.Log("ReceviceDebugMsg " + isDebug);

        if (!isDebug)
            return;

        //ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        debugList.Add(msg);
        return;

    }

    public override void EndFrame(int deltaTime)
    {

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        for (int i = 0; i < debugList.Count; i++)
        {
            if (debugList[i].frame <= csc.confirmFrame)
            {
                DebugLogic(debugList[i]);
                debugList.RemoveAt(i);
                i--;
            }
        }

        Record(null);

        //for (int i = 0; i < m_world.m_entityList.Count; i++)
        //{
        //    int hash = 0;
        //    hash += m_world.m_entityList[i].ToHash();
        //}

        //sendHash(hash);

        //SendHash();
    }

    public override void Update(int deltaTime)
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            OutPutDebugRecord();
        }
    }

    private void SendHash()
    {
        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            int hash = 0;
            hash += m_world.m_entityList[i].ToHash();
        }

        //PlayerResurgence_s msg = new PlayerResurgence_s();
        //msg.diamond = costDiamond;
        //ProtocolAnalysisService.SendCommand(msg);
    }

    public void DebugLogic(DebugMsg msg)
    {
        //Debug.Log("DebugLogic ");

        if (msg.frame == m_world.FrameCount)
        {
            CheckCurrentFrame(msg);
        }
        else if (msg.frame < m_world.FrameCount)
        {
            CheckHistotryFrame(msg);
        }
        else
        {
            string log = "服务器超前 msg:" + msg.frame + " m_world:" + m_world.FrameCount + "\n";
            Debug.LogWarning(log);
            syncLog += log;
        }

        RecordDebugMsg(msg);
    }

    public void Record(string msg = "")
    {

        if (!m_world.IsCertainty)
        {
            return;
        }

        //Debug.Log("Record");

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            EntityBase entity = m_world.m_entityList[i];

            if (isPlayerOnly
                && !(entity.GetExistComp<SelfComponent>() || entity.GetExistComp<TheirComponent>()))
            {
                continue;
            }

            foreach (var item in entity.comps)
            {
                if (item == null)
                    continue;
                string compName = item.GetType().Name;
                if (IsFilter(compName))
                {
                    string key = "local_" + compName;
                    string content = "";

                    if (debugContent.ContainsKey(key))
                    {
                        content = debugContent[key];
                    }
                    else
                    {
                        debugContent.Add(key, content);
                    }

                    //content += "\nframe " + m_world.FrameCount + " id " + entity.ID + " -> " + Serializer.Serialize(item);
                    debugContent[key] = content;
                }
            }
        }

        foreach (var item in m_world.m_singleCompDict)
        {
            string compName = item.Value.GetType().Name;
            if (IsFilter(compName))
            {
                string key = "local_singleComp_" + compName;
                string content = "";

                if (debugContent.ContainsKey(key))
                {
                    content = debugContent[key];
                }
                else
                {
                    debugContent.Add(key, content);
                }

                content += "\nframe " + m_world.FrameCount + " -> " + Serializer.Serialize(item.Value);
                debugContent[key] = content;
            }
        }

        RecordMsg("errorMsg", m_world.FrameCount, msg);
        RecordRandomSeed("local_randomSeed", m_world.FrameCount, m_world.m_RandomSeed);

    }

    void RecordRandomSeed(string key, int frame, int seed)
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

        content += "\nframe " + frame + " -> " + seed;
        debugContent[key] = content;
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
    }

    public static void RecordRandomChange( int frame, int seed,string log)
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

        content += "\nframe " + frame + " -> " + seed + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key] = content;
    }

    public void RecordDebugMsg(DebugMsg msg)
    {
        for (int i = 0; i < msg.infos.Count; i++)
        {
            for (int j = 0; j < msg.infos[i].infos.Count; j++)
            {
                string key = "remote_" + msg.infos[i].infos[j].m_compName;
                string content = "";

                if (debugContent.ContainsKey(key))
                {
                    content = debugContent[key];
                }
                else
                {
                    debugContent.Add(key,content);
                }

                content += "\nframe " + msg.frame + " id " + msg.infos[i].id + " -> " + msg.infos[i].infos[j].content;
                debugContent[key] = content;
            }
        }

        for (int i = 0; i < msg.singleCompInfo.Count; i++)
        {
            string key = "remote_singleComp_" + msg.singleCompInfo[i].m_compName;
            string content = "";

            if (debugContent.ContainsKey(key))
            {
                content = debugContent[key];
            }
            else
            {
                debugContent.Add(key, content);
            }


            content += "\nframe " + msg.frame + " -> " + msg.singleCompInfo[i].content;
            debugContent[key] = content;
        }

        RecordRandomSeed("remote_randomSeed", msg.frame, msg.seed);
    }

    public void OutPutDebugRecord()
    {
        foreach (var item in debugContent)
        {
            //Debug.Log(item.Key + "\n" + item.Value);

            ResourceIOTool.WriteStringByFile(Application.dataPath + "/.OutPut/" + item.Key + ".txt", item.Value);
        }
    }

    void CheckCurrentFrame(DebugMsg msg)
    {
        //Debug.Log("CheckCurrentFrame");
        for (int i = 0; i < msg.infos.Count; i++)
        {
            if (m_world.GetEntityIsExist(msg.infos[i].id))
            {
                EntityBase entity = m_world.GetEntity(msg.infos[i].id);

                for (int j = 0; j < msg.infos[i].infos.Count; j++)
                {
                    if (msg.infos[i].infos[j].m_compName == "CommandComponent")
                    {
                        CheckCommandLogic(msg, msg.infos[i], msg.infos[i].infos[j]);
                    }
                    else
                    {
                        CheckCurrentComponentLogic(msg, entity, msg.infos[i], msg.infos[i].infos[j]);
                    }
                }
            }
            else
            {
                //string log = "error not find entity frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + msg.infos[i].id + "\n";
                //Debug.LogWarning(log);
                //syncLog += log;
            }
        }

        for (int i = 0; i < msg.singleCompInfo.Count; i++)
        {
            CheckCurrentSingleComponentLogic(msg, msg.singleCompInfo[i]);
        }
    }

    void CheckHistotryFrame(DebugMsg msg)
    {
        //Debug.Log("CheckHistotryFrame");
        for (int i = 0; i < msg.infos.Count; i++)
        {
            if (m_world.GetEntityIsExist(msg.infos[i].id))
            {
                for (int j = 0; j < msg.infos[i].infos.Count; j++)
                {
                    if (msg.infos[i].infos[j].m_compName == "CommandComponent")
                    {
                        CheckCommandLogic(msg, msg.infos[i], msg.infos[i].infos[j]);
                    }
                    else
                    {
                        CheckComponentLogic(msg, msg.infos[i], msg.infos[i].infos[j]);
                    }
                }
            }
            else
            {
                //string log = "error not find entity frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + msg.infos[i].id + "\n";
                //Debug.LogWarning(log);
            }
        }

        for (int i = 0; i < msg.singleCompInfo.Count; i++)
        {
            CheckSingleComponentLogic(msg, msg.singleCompInfo[i]);
        }
    }

    //void CheckCurrentCommandLogic(DebugMsg msg, EntityBase entity, EntityInfo entityInfo, ComponentInfo compInfo)
    //{

    //}

    void CheckCurrentComponentLogic(DebugMsg msg, EntityBase entity, EntityInfo entityInfo, ComponentInfo compInfo)
    {
        //Debug.Log("CheckCurrentComponentLogic");

        if (!m_world.GetExistRecordSystem(compInfo.m_compName))
            return;

        ComponentBase compLocal = entity.GetComp(compInfo.m_compName);

        if (IsFilter(compInfo.m_compName))
        {
            string content = Serializer.Serialize(compLocal);

            if (!content.Equals(compInfo.content))
            {
                RecordSystemBase rsb = m_world.GetRecordSystemBase(compInfo.m_compName);

                string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + entityInfo.id + " comp:" + compInfo.m_compName + "\n remote:" + compInfo.content + "\n local:" + content + "\n";
                Debug.LogWarning(log);
                rsb.PrintRecord(entity.ID);

                //派发冲突
                GlobalEvent.DispatchEvent(c_isConflict, msg.frame);

                Time.timeScale = 0;
                Record(log);
                OutPutDebugRecord();
            }
            else
            {
                //Debug.Log("ReceviceDebugMsg  correct! frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + entityInfo.id + " comp:" + compInfo.m_compName + " content :" + compInfo.content);
            }
        }
    }

    void CheckCurrentSingleComponentLogic(DebugMsg msg, ComponentInfo info)
    {
        SingletonComponent sc = m_world.GetSingletonComp(info.m_compName);

        string content = Serializer.Serialize(sc);

        if (!content.Equals(info.content))
        {
            RecordSystemBase rsb = m_world.GetRecordSystemBase(info.m_compName);
            string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " singleComp:" + info.m_compName + "\n remote:" + info.content + "\n local:" + content + "\n";
            Debug.LogWarning(log);
            rsb.PrintRecord(0);

            Time.timeScale = 0;
            Record(log);
            OutPutDebugRecord();
        }
        else
        {
            //Debug.Log("singleComp correct ! frame " + msg.frame + " m_world:" + m_world.FrameCount + "\ncontent " + info.content);
        }
    }

    void CheckCommandLogic(DebugMsg msg, EntityInfo entityInfo, ComponentInfo compInfo)
    {
        PlayerCommandRecordComponent pcrc = m_world.GetEntity(entityInfo.id).GetComp<PlayerCommandRecordComponent>();
        PlayerCommandBase compLocal = pcrc.GetInputCahae(msg.frame);

        if (compLocal == null)
        {
            return;
        }

        compLocal.time = 0;
        string content = Serializer.Serialize(compLocal);

        if (!content.Equals(compInfo.content))
        {
            string log = "error: frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + msg + " msg.id " + entityInfo.id + " comp:" + compInfo.m_compName + "\n remote:" + compInfo.content + "\n local:" + content + "\n";
            Debug.LogWarning(log);
            string record = "";

            for (int k = msg.frame; k > msg.frame - 10; k--)
            {
                PlayerCommandBase tmp = pcrc.GetInputCahae(k);

                record += "\nframe " + k + " c: " + Serializer.Serialize(tmp);
            }

            Debug.Log(record);

            Time.timeScale = 0;
            Record(log);
            OutPutDebugRecord();
        }
        else
        {
            //Debug.Log(" confirm " + compInfo.content);
        }
    }

    void CheckComponentLogic(DebugMsg msg, EntityInfo entityInfo, ComponentInfo compInfo)
    {
        //Debug.Log("CheckComponentLogic");

        if (!m_world.GetExistRecordSystem(compInfo.m_compName))
            return;

        RecordSystemBase rsb = m_world.GetRecordSystemBase(compInfo.m_compName);
        ComponentBase compLocal = rsb.GetRecord(entityInfo.id, msg.frame);

        if (IsFilter(compInfo.m_compName))
        {
            if (compLocal != null)
            {
                string content = Serializer.Serialize(compLocal);

                if (!content.Equals(compInfo.content))
                {
                    string log = "error: frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entityInfo.id + " comp:" + compInfo.m_compName 
                        + "\n remote:" + compInfo.content 
                        + "\n local:" + content + "\n";
                    Debug.LogWarning(log);
                    rsb.PrintRecord(entityInfo.id);

                    Time.timeScale = 0;
                    Record(log);
                    OutPutDebugRecord();
                }
                else
                {
                    //Debug.Log("ReceviceDebugMsg  correct! frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entityInfo.id + " comp:" + compInfo.m_compName + " content :" + compInfo.content);
                }

                //派发冲突
                GlobalEvent.DispatchEvent(c_isConflict, msg.frame);
            }
            else
            {
                //string log = "not find Record ->> frame:" + msg.frame + " id " + entityInfo.id + " compName: " + compInfo.m_compName + " currentframe: " + m_world.FrameCount + " content " + compInfo.content;

                //Debug.LogWarning(log);
                //syncLog += log;
            }
        }
        else
        {
            Debug.Log("Not is filter " + compInfo.m_compName);
        }
    }

    void CheckSingleComponentLogic(DebugMsg msg, ComponentInfo info)
    {
        RecordSystemBase rsb = m_world.GetRecordSystemBase(info.m_compName);
        SingletonComponent sc = rsb.GetSingletonRecord(msg.frame);

        if (sc == null)
        {
            //Debug.LogWarning("");
            return;
        }

        string content = Serializer.Serialize(sc);

        if (!content.Equals(info.content))
        {
            string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " HashCode " + sc.GetHashCode() + " singleComp:" + info.m_compName + "\n remote:" + info.content + "\n local:" + content + "\n";
            Debug.LogWarning(log);
            rsb.PrintRecord(0);

            Time.timeScale = 0;
            Record(log);
            OutPutDebugRecord();
        }
        else
        {
            //Debug.Log("singleComp correct ! frame " + msg.frame + " m_world:" + m_world.FrameCount + "\ncontent " + info.content);
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

        for (int i = 0; i < SingleCompFilter.Length; i++)
        {
            if (SingleCompFilter[i] == compName)
            {
                return true;
            }
        }

        return false;
    }

    public static void LogAndDebug(string content, string tag = null)
    {
        if (isDebug && (tag == null || IsFilter(tag)))
        {
            syncLog += content + "\n";
            Debug.Log(content);
        }
    }
}

