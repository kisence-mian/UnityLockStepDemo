#if Server
using DeJson;
#endif
using FastCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RecordSystem<T> : RecordSystemBase where T : MomentComponentBase, new()
{
    public FastList<T> m_record = new FastList<T>();
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
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            T data = list[i].GetComp<T>(hashCode);

            //if(data.IsChange)
            {
                data.IsChange = false;

                T record = (T)data.DeepCopy();
                record.Frame = frame;
                record.ID = list[i].ID;

                m_record.Add(record);
            }
        }
    }

    public override void RevertToFrame(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            T record = m_record[i];

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

    public override MomentComponentBase GetRecord(int id, int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if(m_record[i].ID == id &&
                m_record[i].Frame == frame)
            {
                return m_record[i];
            }
        }

        return null;
    }
    public override void PrintRecord(int id)
    {
        string content = "compName : " + typeof(T).Name + "\n";
        for (int i = 0; i < m_record.Count; i++)
        {
            if(m_record[i].Frame > m_world.FrameCount - 10)
            {
                if (id == -1 || m_record[i].ID == id)
                {
                    content += " ID:" + m_record[i].ID + " Frame:" +m_record[i].Frame + " content:" + Serializer.Serialize(m_record[i]) + "\n";
                }
            }
        }
        Debug.LogWarning("PrintRecord:" + content);
    }

    public override void ClearAll()
    {
        m_record.Clear();
    }

    public override void ClearRecordAt(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if (m_record[i].Frame == frame)
            {
                m_record.RemoveAt(i);
                i--;
            }
        }
    }

    public override void ClearAfter(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if (m_record[i].Frame > frame)
            {
                m_record.RemoveAt(i);
                i--;
            }
        }
    }

    public override void ClearBefore(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if (m_record[i].Frame < frame)
            {
                m_record.RemoveAt(i);
                i--;
            }
        }
    }
}
