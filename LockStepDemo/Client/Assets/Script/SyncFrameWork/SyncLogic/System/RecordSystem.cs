using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RecordSystem<T> : RecordSystemBase where T: MomentComponentBase ,new()
{
    public override Type[] GetFilter()
    {
        return new Type[] { typeof(T) };
    }

    public override void Record()
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>> ();

        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            T record = (T)list[i].GetComp<T>().DeepCopy();
            record.Frame = m_world.FrameCount;
            record.ID    = list[i].ID;

            rc.m_record.Add(record);
        }
    }

    public override void RevertToFrame(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();

        List<T> list = rc.GetRecordList(frame);

        for (int i = 0; i < list.Count; i++)
        {
            EntityBase entity = m_world.GetEntity(list[i].ID);

            entity.ChangeComp(list[i]);

            //Debug.Log("数据回滚 ID：" + list[i].ID + " frame:"+ list[i].Frame +" conent:"+  Serializer.Serialize(list[i]));
        }
    }

    public override void ClearAfter(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();
        rc.ClearAfter(frame);
    }

    public override void ClearBefore(int frame)
    {
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
}
