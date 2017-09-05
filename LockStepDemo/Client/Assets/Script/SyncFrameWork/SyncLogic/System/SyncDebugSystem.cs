using FrameWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SyncDebugSystem : SystemBase
{
    public static bool isDebug = false;

    public static string[] DebugFilter = new string[] {"MoveComponent"};

    public static string syncLog = "";

    public override void Init()
    {
        GlobalEvent.AddTypeEvent<DebugMsg>(ReceviceDebugMsg);

        if(isDebug)
            ApplicationManager.s_OnApplicationOnGUI += OnGUI;
    }

    public override void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<DebugMsg>(ReceviceDebugMsg);
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Show Log"))
        {
            Debug.Log("syncLog:"+syncLog);
            ResourceIOTool.WriteStringByFile(Application.dataPath + "/SyncLog/syncLog.txt", syncLog);
        }
    }

    Deserializer deserializer = new Deserializer();
    public void ReceviceDebugMsg(DebugMsg msg, params object[] objs)
    {
        //if (!isDebug)
        //    return;

        if (msg.frame == m_world.FrameCount)
        {
            for (int i = 0; i < msg.infos.Count; i++)
            {
                if (m_world.GetEntityIsExist(msg.infos[i].id))
                {
                    EntityBase entity = m_world.GetEntity(msg.infos[i].id);

                    for (int j = 0; j < msg.infos[i].infos.Count; j++)
                    {
                        ComponentBase compLocal = entity.GetComp(msg.infos[i].infos[j].m_compName);

                        if (IsFilter(msg.infos[i].infos[j].m_compName))
                        {
                            string content = Serializer.Serialize(compLocal);

                            if (!content.Equals(msg.infos[i].infos[j].content))
                            {
                                RecordSystemBase rsb = m_world.GetRecordSystemBase(msg.infos[i].infos[j].m_compName);

                                string log = "error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + msg.infos[i].id + " comp:" + msg.infos[i].infos[j].m_compName + "\n remote:" + msg.infos[i].infos[j].content + "\n local:" + content +"\n";
                                Debug.LogWarning(log);
                                rsb.PrintRecord(entity.ID);
                                syncLog += log;
                            }
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
        }
        else if (msg.frame < m_world.FrameCount)
        {
            for (int i = 0; i < msg.infos.Count; i++)
            {
                if (m_world.GetEntityIsExist(msg.infos[i].id))
                {
                    EntityBase entity = m_world.GetEntity(msg.infos[i].id);

                    for (int j = 0; j < msg.infos[i].infos.Count; j++)
                    {
                        RecordSystemBase rsb = m_world.GetRecordSystemBase(msg.infos[i].infos[j].m_compName);
                        ComponentBase compLocal = rsb.GetRecord(msg.infos[i].id, msg.frame);

                        if(IsFilter(msg.infos[i].infos[j].m_compName))
                        {
                            if (compLocal != null)
                            {
                                string content = Serializer.Serialize(compLocal);

                                if (!content.Equals(msg.infos[i].infos[j].content))
                                {
                                    string log = "error: frame " + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + msg.infos[i].id + " comp:" + msg.infos[i].infos[j].m_compName + "\n remote:" + msg.infos[i].infos[j].content + "\n local:" + content + "\n";
                                    Debug.LogWarning(log);
                                    rsb.PrintRecord(entity.ID);

                                    syncLog += log;
                                }
                            }
                            else
                            {
                                string log = "not find Record ->> frame:" + msg.frame + " id " + msg.infos[i].id + " compName: " + msg.infos[i].infos[j].m_compName + "\ncurrentframe: "+m_world.FrameCount;

                                //Debug.LogWarning(log);
                                syncLog += log;
                            }
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
        }
        else
        {
            string log = "服务器超前 msg:" + msg.frame + " m_world:" + m_world.FrameCount + "\n";
            //Debug.LogWarning(log);
            syncLog += log;
        }
    }

    public static bool IsFilter(string compName)
    {
        if(DebugFilter.Length == 0)
        {
            return true;
        }

        for (int i = 0; i < DebugFilter.Length; i++)
        {
            if(DebugFilter[i] == compName)
            {
                return true;
            }
        }

        return false;
    }

    public static void LogAndDebug(string content,string tag = null)
    {
        if(isDebug && ( tag == null || IsFilter(tag)))
        {
            syncLog += content + "\n";
            Debug.Log(content);
        }
    }
}

