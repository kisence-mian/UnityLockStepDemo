#if Server
using DeJson;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSingletonSystem<T> : RecordSystemBase where T : MomentSingletonComponent, new()
{
    List<MomentSingletonComponent> m_recordInfo = new List<MomentSingletonComponent>();

    public override void Record(int frame)
    {
        MomentSingletonComponent record = (MomentSingletonComponent)m_world.GetSingletonComp<T>().DeepCopy();
        record.Frame = frame;

        m_recordInfo.Add(record);

        //if(typeof(T) == typeof(LogicRuntimeMachineComponent))
        //{
        //    LogicRuntimeMachineComponent lrmc = (LogicRuntimeMachineComponent)record;

        //    //Debug.Log("Record  " + " content: " + Serializer.Serialize(record) +" frame " +m_world.FrameCount);
        //}
    }

    public override void RevertToFrame(int frame)
    {
        T record = (T)GetSingletonRecord(frame);
        m_world.ChangeSingletonComp<T>(record);
    }


    public override void ClearAfter(int frame)
    {
        for (int i = 0; i < m_recordInfo.Count; i++)
        {
            if (m_recordInfo[i].Frame > frame)
            {
                m_recordInfo.RemoveAt(i);
                i--;
            }
        }
    }

    public override void ClearAll()
    {
        m_recordInfo.Clear();
    }

    public override void ClearBefore(int frame)
    {
        for (int i = 0; i < m_recordInfo.Count; i++)
        {
            if (m_recordInfo[i].Frame < frame)
            {
                m_recordInfo.RemoveAt(i);
                i--;
            }
        }
    }

    public override MomentSingletonComponent GetSingletonRecord(int frame)
    {
        for (int i = 0; i < m_recordInfo.Count; i++)
        {
            if(m_recordInfo[i].Frame == frame)
            {
                return m_recordInfo[i];
            }
        }

        throw new Exception("Not find MomentSingletonComponent　" + typeof(T).FullName + " by frame　" + frame);
    }


    public override void PrintRecord(int id)
    {
        string content = "SingleCompName : " + typeof(T).Name + "\n";
        for (int i = 0; i < m_recordInfo.Count; i++)
        {
            content += " Frame:" + m_recordInfo[i].Frame + " content:" + Serializer.Serialize(m_recordInfo[i]) + "\n";
        }
        Debug.LogWarning("PrintRecord:" + content);
    }

}
