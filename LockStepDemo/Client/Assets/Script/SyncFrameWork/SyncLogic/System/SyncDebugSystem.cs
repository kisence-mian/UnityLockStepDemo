using FrameWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.SyncFrameWork.SyncLogic.System
{
    class SyncDebugSystem : SystemBase
    {
        public override void Init()
        {
            GlobalEvent.AddTypeEvent<DebugMsg>(ReceviceDebugMsg);
        }

        public override void Dispose()
        {
            GlobalEvent.RemoveTypeEvent<DebugMsg>(ReceviceDebugMsg);
        }
        Deserializer deserializer = new Deserializer();
        public void ReceviceDebugMsg(DebugMsg msg,params object[] objs)
        {
            if(msg.frame == m_world.FrameCount)
            {
                for (int i = 0; i < msg.infos.Count; i++)
                {
                    EntityBase entity = m_world.GetEntity(msg.infos[i].id);

                    for (int j = 0; j < msg.infos[i].infos.Count; j++)
                    {
                        ComponentBase compLocal = entity.GetComp(msg.infos[i].infos[j].m_compName);

                        string content = Serializer.Serialize(compLocal);

                        if (!content.Equals(msg.infos[i].infos[j].content))
                        {
                            RecordSystemBase rsb = m_world.GetRecordSystemBase(msg.infos[i].infos[j].m_compName);

                            Debug.LogWarning("error: frame" + msg.frame + " currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + msg.infos[i].id + " comp:" + msg.infos[i].infos[j].m_compName + "\n remote:" + msg.infos[i].infos[j].content + "\n local:" + content);
                            rsb.PrintRecord(entity.ID);
                        }
                    }
                }
            }
            else if(msg.frame < m_world.FrameCount)
            {
                for (int i = 0; i < msg.infos.Count; i++)
                {
                    EntityBase entity = m_world.GetEntity(msg.infos[i].id);

                    for (int j = 0; j < msg.infos[i].infos.Count; j++)
                    {
                        RecordSystemBase rsb = m_world.GetRecordSystemBase(msg.infos[i].infos[j].m_compName);

                        ComponentBase compLocal = rsb.GetRecord(msg.infos[i].id, msg.frame);

                        if(compLocal != null)
                        {
                            string content = Serializer.Serialize(compLocal);

                            if (!content.Equals(msg.infos[i].infos[j].content))
                            {
                                Debug.LogWarning("error: frame" + msg.frame +" currentFrame:" + m_world.FrameCount + " id:" + entity.ID + " msg.id " + msg.infos[i].id + " comp:" + msg.infos[i].infos[j].m_compName + "\n remote:" + msg.infos[i].infos[j].content + "\n local:" + content);
                                rsb.PrintRecord(entity.ID);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("not find Record ->> frame:" + msg.frame + " id " + msg.infos[i].id + " compName: " + msg.infos[i].infos[j].m_compName);
                            rsb.PrintRecord(entity.ID);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("服务器超前 msg:" + msg.frame + " m_world:" + m_world.FrameCount);
            }
        }
    }
}
