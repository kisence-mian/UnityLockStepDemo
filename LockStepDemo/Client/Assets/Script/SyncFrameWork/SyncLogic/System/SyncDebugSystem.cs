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

    public const string c_isAllMessage = "AllMessage";
    public const string c_isConflict   = "Conflict";
    public const string c_MissData     = "MissData";
    public const string c_Recalc       = "Recalc";

    public static string[] DebugFilter = new string[] { "MoveComponent", "GrowUpComponent" /*"LifeComponent", "LifeSpanComponent"*/ };

    public static string[] SingleCompFilter = new string[] { /*"LogicRuntimeMachineComponent"*/ };

    public static string syncLog = "";

    List<DebugMsg> debugList = new List<DebugMsg>();

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

        ConnectStatusComponent csc = m_world.GetSingletonComp<ConnectStatusComponent>();

        if (msg.frame > csc.confirmFrame)
        {
            debugList.Add(msg);
            return;
        }
        DebugLogic(msg);
    }

    public override void FixedUpdate(int deltaTime)
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

        //for (int i = 0; i < m_world.m_entityList.Count; i++)
        //{
        //    int hash = 0;
        //    hash += m_world.m_entityList[i].ToHash();
        //}

        //sendHash(hash);

        //SendHash();
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
                string log = "error not find entity frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + msg.infos[i].id + "\n";
                Debug.LogWarning(log);
                syncLog += log;
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
                syncLog += log;

                //派发冲突
                GlobalEvent.DispatchEvent(c_isConflict, msg.frame);
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
        }
        else
        {
            Debug.Log("singleComp correct ! frame " + msg.frame + " m_world:" + m_world.FrameCount + "\ncontent " + info.content);
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
        }
        else
        {
            //Debug.Log(" confirm " + compInfo.content);
        }
    }

    void CheckComponentLogic(DebugMsg msg, EntityInfo entityInfo, ComponentInfo compInfo)
    {
        //Debug.Log("CheckComponentLogic");

        RecordSystemBase rsb = m_world.GetRecordSystemBase(compInfo.m_compName);
        ComponentBase compLocal = rsb.GetRecord(entityInfo.id, msg.frame);

        if (IsFilter(compInfo.m_compName))
        {
            if (compLocal != null)
            {
                string content = Serializer.Serialize(compLocal);

                if (!content.Equals(compInfo.content))
                {
                    string log = "error: frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entityInfo.id + " comp:" + compInfo.m_compName + "\n remote:" + compInfo.content + "\n local:" + content + "\n";
                    Debug.LogWarning(log);
                    rsb.PrintRecord(entityInfo.id);

                    syncLog += log;
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
            Debug.LogWarning("");
            return;
        }

        string content = Serializer.Serialize(sc);

        if (!content.Equals(info.content))
        {
            string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " HashCode " + sc.GetHashCode() + " singleComp:" + info.m_compName + "\n remote:" + info.content + "\n local:" + content + "\n";
            Debug.LogWarning(log);
            rsb.PrintRecord(0);
        }
        else
        {
            Debug.Log("singleComp correct ! frame " + msg.frame + " m_world:" + m_world.FrameCount + "\ncontent " + info.content);
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

    public static void LogAndDebug(string content, string tag = null)
    {
        if (isDebug && (tag == null || IsFilter(tag)))
        {
            syncLog += content + "\n";
            Debug.Log(content);
        }
    }
}

