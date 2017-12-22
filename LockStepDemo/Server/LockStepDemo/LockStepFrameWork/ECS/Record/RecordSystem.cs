#if Server
using DeJson;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RecordSystem<T> : RecordSystemBase where T : MomentComponentBase, new()
{
    int hashCode = 0;

    public override Type[] GetFilter()
    {
        return new Type[] { typeof(T) };
    }

    public override void Init()
    {
        hashCode = m_world.componentType.GetComponentIndex(typeof(T).Name);
    }

    public override void Record(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>> ();

        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            T record = (T)list[i].GetComp<T>(hashCode).DeepCopy();
            record.Frame = frame;
            record.ID    = list[i].ID;

            rc.m_record.Add(record);
        }
    }

    public override void RevertToFrame(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();

        for (int i = 0; i < rc.m_record.Count; i++)
        {
            T record = rc.m_record[i];

            if (record.Frame == frame)
            {
                if (m_world.GetEntityIsExist(record.ID))
                {
                    EntityBase entity = m_world.GetEntity(record.ID);
                    entity.ChangeComp(hashCode, record.DeepCopy());
                }
                else if (m_world.GetIsExistDispatchDestroyCache(record.ID))
                {
                    EntityBase entity = m_world.GetDispatchDestroyCache(record.ID);
                    entity.ChangeComp(hashCode, record.DeepCopy());
                }
                else if (m_world.GetIsExistDispatchCreateCache(record.ID))
                {
                    EntityBase entity = m_world.GetDispatchCreateCache(record.ID);
                    entity.ChangeComp(hashCode, record.DeepCopy());
                }
            }
        }
    }

    public override void ClearAfter(int frame)
    {
       //return;

        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        rc.ClearAfter(frame);
    }

    public override void ClearBefore(int frame)
    {
        //return;

        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        rc.ClearBefore(frame);
    }

    public override MomentComponentBase GetRecord(int id, int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        for (int i = 0; i < rc.m_record.Count; i++)
        {
            if(rc.m_record[i].ID == id &&
                rc.m_record[i].Frame == frame)
            {
                return rc.m_record[i];
            }
        }

        return null;
    }
    public override void PrintRecord(int id)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();

        string content = "compName : " + typeof(T).Name + "\n";
        for (int i = 0; i < rc.m_record.Count; i++)
        {
            if(rc.m_record[i].Frame > m_world.FrameCount - 10)
            {
                if (id == -1 || rc.m_record[i].ID == id)
                {
                    content += " ID:" + rc.m_record[i].ID + " Frame:" + rc.m_record[i].Frame + " content:" + Serializer.Serialize(rc.m_record[i]) + "\n";
                }
            }
        }
        Debug.LogWarning("PrintRecord:" + content);
    }

    public override void ClearAll()
    {
        //return;

        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        rc.m_record.Clear();
    }

    public override void ClearRecordAt(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        rc.ClearRecordAt(frame);
    }
}
