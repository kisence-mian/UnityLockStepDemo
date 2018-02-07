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
    public static bool isFlyObject = false;

    public const string c_isAllMessage = "AllMessage";
    public const string c_isConflict   = "Conflict";
    public const string c_MissData     = "MissData";
    public const string c_Recalc       = "Recalc";

    public static string[] DebugFilter = new string[] {
        "LifeSpanComponent",
        "MoveComponent",
        "PlayerComponent",
        "LifeComponent",
        "BlowFlyComponent",
        "FlyObjectComponent",
        "GrowUpComponent",
        "AIComponent",
    };

    public static string[] SingleCompFilter = new string[] { /*"MapGridStateComponent",*/ /*"LogicRuntimeMachineComponent" */};

    //public static string syncLog = "";

    List<DebugMsg> debugList = new List<DebugMsg>();

    static Dictionary<string, StringBuilder> debugContent = new Dictionary<string, StringBuilder>();

    static List<DebugRecord> msgCacheList = new List<DebugRecord>();
    public static StringBuilder msgCache = new StringBuilder();

    public static WorldBase s_world;

    public override Type[] GetFilter()
    {
        return new Type[]
            {
                typeof(SelfComponent),
            };
    }

    public override void Init()
    {
        s_world = m_world;
        //AddEntityCreaterLisnter();
        //AddEntityDestroyLisnter();

        GlobalEvent.AddTypeEvent<DebugMsg>(ReceviceDebugMsg);
        ApplicationManager.s_OnApplicationOnGUI += GUI;

    }

    public override void Dispose()
    {
        //RemoveEntityCreaterLisnter();
        //RemoveEntityDestroyLisnter();
        ApplicationManager.s_OnApplicationOnGUI -= GUI;
        GlobalEvent.RemoveTypeEvent<DebugMsg>(ReceviceDebugMsg);
    }

    void GUI()
    {
        if(isDebug && GUILayout.Button("Print"))
        {
            OutPutDebugRecord();
        }

        if (isDebug && GUILayout.Button("Query"))
        {
            List<EntityBase> list = GetEntityList();

            for (int i = 0; i < list.Count; i++)
            {
                QueryCommand qc = new QueryCommand();
                qc.frame = m_world.FrameCount - 10;
                qc.id = list[i].ID;

                ProtocolAnalysisService.SendCommand(qc);
            }
        }
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
        //Debug.Log("ReceviceDebugMsg");
        isDebug = true;

        debugList.Add(msg);
        return;
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        msgCache.Remove(0, msgCache.Length);
    }

    public override void EndFrame(int deltaTime)
    {
        //Debug.Log("EndFrame ");
        if (!isDebug)
            return;

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();
        //Debug.Log("debugList.Count " + debugList.Count);

        for (int i = 0; i < debugList.Count; i++)
        {
            if (debugList[i].frame == m_world.FrameCount)
            {
                DebugLogic(debugList[i]);
                debugList.RemoveAt(i);
                i--;
            }
            else if(debugList[i].frame < m_world.FrameCount)
            {
                //Debug.Log("历史数据 " + debugList[i].frame);
                RecordDebugMsg(debugList[i]);

                debugList.RemoveAt(i);
                i--;
            }
        }

        Record(null);
    }

    public override void Update(int deltaTime)
    {
        if (!isDebug)
            return;

        if (Input.GetKeyDown(KeyCode.F12))
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
            Debug.Log("检查了历史数据 msg " + msg.frame + " world" + m_world.FrameCount);
            //CheckHistotryFrame(msg);
        }
        else
        {
            string log = "服务器超前 msg:" + msg.frame + " m_world:" + m_world.FrameCount + "\n";
            Debug.LogWarning(log);
            //syncLog += log;
        }

        var dr = GetDebugMsg(msg.frame);
        if(dr!= null && dr.msg != msg.msg)
        {
            //Time.timeScale = 0;
            //Debug.LogWarning("msg error! frame :" + msg.frame + "\nlocal :\n " + dr.msg + "\nremote :\n " + msg.msg);
        }
        else
        {
            //Debug.Log("msg " + msg.msg);
        }

        RecordDebugMsg(msg);
    }

    int x = 0;
    System.Diagnostics.StackTrace st;

    public void Record(string msg = "")
    {
        //return;

        //Debug.Log("sync record " + m_world.FrameCount);

        if (!m_world.IsCertainty)
        {
            return;
        }

        if (x >= m_world.FrameCount)
        {
            Debug.LogError("重复的确定帧！" + st + "\n\n");
        }

        DebugRecord dr = new DebugRecord();
        dr.frame = m_world.FrameCount;

        //把有哪些ID也打印进来
        //msgCache.Append("\nIDs:\n");
        //for (int i = 0; i < m_world.m_entityList.Count; i++)
        //{
        //    msgCache.Append(m_world.m_entityList[i].ID + "\n");
        //}

        dr.msg = msgCache.ToString();

        RecordMsg("DebugMsg", m_world.FrameCount, dr.msg);

        msgCacheList.Add(dr);

        x = m_world.FrameCount;
        st = new System.Diagnostics.StackTrace();

        //Debug.Log("Record");

        for (int i = 0; i < m_world.m_entityList.Count; i++)
        {
            EntityBase entity = m_world.m_entityList[i];

            bool isFilter = false;
            if (isPlayerOnly
                 && !entity.GetExistComp(ComponentType.PlayerComponent))
            {
                isFilter = true;
            }

            if (isFlyObject
                 && !entity.GetExistComp(ComponentType.FlyObjectComponent))
            {
                isFilter = true;
            }

            if (isFilter)
            {
                continue;
            }

            foreach (var item in entity.comps)
            {
                if(item == null)
                {
                    continue;
                }

                string compName = item.GetType().Name;
                if (IsFilter(compName))
                {
                    string key = "local_" + compName;
                    string content = "";

                    if (!debugContent.ContainsKey(key))
                    {
                        debugContent.Add(key, new StringBuilder());
                    }

                    content += "\nframe " + m_world.FrameCount + " id " + entity.ID + " -> " + Serializer.Serialize(item);
                    debugContent[key].Append( content);

                    //Debug.Log("frame " + m_world.FrameCount + " id " + entity.ID + "-> " + Serializer.Serialize(item));
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

                if (!debugContent.ContainsKey(key))
                {
                    debugContent.Add(key, new StringBuilder());
                }

                content += "\nframe " + m_world.FrameCount + " -> " + Serializer.Serialize(item.Value);
                debugContent[key].Append(content);
            }
        }

        RecordMsg("errorMsg", m_world.FrameCount, msg);
        RecordRandomSeed("local_randomSeed", m_world.FrameCount, m_world.m_RandomSeed);

        //Debug.Log("local_randomSeed " + m_world.FrameCount + " " + m_world.m_RandomSeed);
    }

    void RecordRandomSeed(string key, int frame, int seed)
    {
        string content = "";

        if (!debugContent.ContainsKey(key))
        {
            debugContent.Add(key, new StringBuilder());
        }

        content += "\nframe " + frame + " -> " + seed;
        debugContent[key].Append(content);
    }

    public static void RecordMsg(string key, int frame, string msg)
    {
        if (!isDebug)
            return;

        if (msg == null)
            return;

        string content = "";

        if (!debugContent.ContainsKey(key))
        {
            debugContent.Add(key, new StringBuilder());
        }

        content += "\nframe " + frame + " -> " + msg;
        debugContent[key].Append(content);
    }

    public static void AddDebugMsg(string msg)
    {
        if (!isDebug)
            return;

        if (!s_world.IsCertainty)
            return;

        msgCache.Append(msg + "\n");
    }

    public static void RecordRandomChange( int frame, int seed,string log)
    {
        string key = "local_randomChange";
        string content = "";

        if (!debugContent.ContainsKey(key))
        {
            debugContent.Add(key, new StringBuilder());
        }

        content += "\nframe " + frame + " -> " + seed + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key].Append(content);
    }

    public static void RecordGrowUpChange(int frame, string log)
    {
        string key = "GrowUpChange";
        string content = "";

        if (!debugContent.ContainsKey(key))
        {
            debugContent.Add(key, new StringBuilder());
        }

        content += "\nframe " + frame + " -> " + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key].Append(content);
    }

    public static void RecordMsgByStackTrace(string key,int frame, string log)
    {
        string content = "";

        if (!debugContent.ContainsKey(key))
        {
            debugContent.Add(key, new StringBuilder());
        }

        content += "\nframe " + frame + " -> " + " log " + log + "\n" + new System.Diagnostics.StackTrace().ToString();
        debugContent[key].Append(content);
    }

    public void RecordDebugMsg(DebugMsg msg)
    {
        for (int i = 0; i < msg.infos.Count; i++)
        {
            for (int j = 0; j < msg.infos[i].infos.Count; j++)
            {
                string key = "remote_" + msg.infos[i].infos[j].m_compName;
                string content = "";

                if (!debugContent.ContainsKey(key))
                {
                    debugContent.Add(key, new StringBuilder());
                }

                content += "\nframe " + msg.frame + " id " + msg.infos[i].id + " -> " + msg.infos[i].infos[j].content;
                debugContent[key].Append(content);
            }
        }

        for (int i = 0; i < msg.singleCompInfo.Count; i++)
        {
            string key = "remote_singleComp_" + msg.singleCompInfo[i].m_compName;
            string content = "";

            if (!debugContent.ContainsKey(key))
            {
                debugContent.Add(key, new StringBuilder());
            }


            content += "\nframe " + msg.frame + " -> " + msg.singleCompInfo[i].content;
            debugContent[key].Append(content);
        }

        RecordRandomSeed("remote_randomSeed", msg.frame, msg.seed);
    }

    public void OutPutDebugRecord()
    {
//#if UNITY_EDITOR
        foreach (var item in debugContent)
        {
            //Debug.Log(item.Key + "\n" + item.Value);
            PersistentFileManager.SaveData(UserData.NickName+"_" + item.Key, item.Value.ToString());

            //ResourceIOTool.WriteStringByFile(Application.dataPath + "/.OutPut/" + item.Key + ".txt", item.Value.ToString());
        }
//#endif
    }

    void CheckCurrentFrame(DebugMsg msg)
    {
        //Debug.Log("CheckCurrentFrame " + msg.frame);
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
                OutPutDebugRecord();
            }
            else
            {
                //Debug.Log("ReceviceDebugMsg  correct! frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + entityInfo.id + " comp:" + compInfo.m_compName + " content :" + compInfo.content);
            }
        }
    }

    Deserializer des = new Deserializer();

    void CheckCurrentSingleComponentLogic(DebugMsg msg, ComponentInfo info)
    {
        SingletonComponent sc = m_world.GetSingletonComp(info.m_compName);

        if(info.m_compName == "MapGridStateComponent")
        {
            MapGridStateComponent lmsc = (MapGridStateComponent)sc;

            MapGridStateComponent msc = des.Deserialize<MapGridStateComponent>(info.content);

            if(!JudgeDict(msc.globalRandomCellHaveItemList, lmsc.globalRandomCellHaveItemList))
            {
                string content = Serializer.Serialize(sc);
                string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " singleComp:" + info.m_compName + "\n remote:" + info.content + "\n local:" + content + "\n";
                Debug.LogWarning(log);

                OutPutDebugRecord();
            }
        }
        else
        {
            string content = Serializer.Serialize(sc);

            if (!content.Equals(info.content))
            {
                RecordSystemBase rsb = m_world.GetRecordSystemBase(info.m_compName);
                string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " singleComp:" + info.m_compName + "\n remote:" + info.content + "\n local:" + content + "\n";
                Debug.LogWarning(log);
                rsb.PrintRecord(0);

                Time.timeScale = 0;
                OutPutDebugRecord();
            }
            else
            {
                //Debug.Log("singleComp correct ! frame " + msg.frame + " m_world:" + m_world.FrameCount + "\ncontent " + info.content);
            }
        }


    }

    bool JudgeDict(Dictionary<int,MapCell> a, Dictionary<int, MapCell> b)
    {
        foreach (var item in a)
        {
            if(b.ContainsKey(item.Key))
            {
                if(!b[item.Key].Eq(item.Value))
                {
                    Debug.LogWarning("dont Eq " + item.Key);
                    return false;
                }
            }
            else
            {
                Debug.LogWarning("dont ContainsKey " + item.Key);

                return false;
            }
        }

        foreach (var item in b)
        {
            if (a.ContainsKey(item.Key))
            {
                if (!a[item.Key].Eq(item.Value))
                {
                    Debug.LogWarning("dont Eq " + item.Key);
                    return false;
                }
            }
            else
            {
                Debug.LogWarning("dont ContainsKey " + item.Key);
                return false;
            }
        }

        return true;
    }


    void CheckCommandLogic(DebugMsg msg, EntityInfo entityInfo, ComponentInfo compInfo)
    {
        if(!m_world.GetEntity(entityInfo.id).GetExistComp(ComponentType.PlayerCommandRecordComponent))
        {
            return;
        }

        PlayerCommandRecordComponent pcrc = m_world.GetEntity(entityInfo.id).GetComp<PlayerCommandRecordComponent>(ComponentType.PlayerCommandRecordComponent);
        PlayerCommandBase compLocal = pcrc.GetInputCahae(msg.frame);

        if (compLocal == null)
        {
            return;
        }

        compLocal.time = 0;
        compLocal.frame = msg.frame;
        string content = Serializer.Serialize(compLocal);

        if (!content.Equals(compInfo.content))
        {
            string log = "error: frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " msg.id " + entityInfo.id + " comp:" + compInfo.m_compName + "\n remote:" + compInfo.content + "\n local:" + content + "\n";
            Debug.LogWarning(log);
            string record = "";

            for (int k = msg.frame; k > msg.frame - 10; k--)
            {
                PlayerCommandBase tmp = pcrc.GetInputCahae(k);

                record += "\nframe " + k + " c: " + Serializer.Serialize(tmp);
            }

            Debug.Log(record);

            Time.timeScale = 0;
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
            //Debug.Log("Not is filter " + compInfo.m_compName);
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

    //public static void LogAndDebug(string content, string tag = null)
    //{
    //    if (isDebug && (tag == null || IsFilter(tag)))
    //    {
    //        syncLog += content + "\n";
    //        Debug.Log(content);
    //    }
    //}

    DebugRecord GetDebugMsg(int frame)
    {
        for (int i = 0; i < msgCacheList.Count; i++)
        {
            if(frame == msgCacheList[i].frame)
            {
                return msgCacheList[i];
            }
        }

        return null;
    }

    class DebugRecord
    {
        public int frame;
        public string msg;
    }
}

