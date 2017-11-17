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

    public override void Record(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>> ();

        List<EntityBase> list = GetEntityList();

        //if (SyncDebugSystem.IsFilter(typeof(T).Name))
        //{
        //    Debug.Log("Record count " + list.Count);
        //}

        for (int i = 0; i < list.Count; i++)
        {
            T record = (T)list[i].GetComp<T>().DeepCopy();
            record.Frame = frame;
            record.ID    = list[i].ID;

            rc.m_record.Add(record);
            //if (SyncDebugSystem.IsFilter(typeof(T).Name))
            //{
            //    Debug.Log("数据记录 ID：" + list[i].ID + " frame:" + frame + " conent:" + Serializer.Serialize(record));
            //}
        }
    }

    public override void RevertToFrame(int frame)
    {
        RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();

        List<T> list = rc.GetRecordList(frame);

        //if (SyncDebugSystem.IsFilter(typeof(T).Name))
        //{
        //    Debug.Log("数据回滚  frame:" + frame + " Count:" + list.Count);
        //}

        for (int i = 0; i < list.Count; i++)
        {
            if(m_world.GetEntityIsExist(list[i].ID))
            {
                //if (SyncDebugSystem.IsFilter(typeof(T).Name))
                //{
                //    Debug.Log("在游戏中 " + list[i].ID);
                //}

                EntityBase entity = m_world.GetEntity(list[i].ID);
                entity.ChangeComp((T)list[i].DeepCopy());
            }
            else if(m_world.GetIsExistDispatchDestroyCache(list[i].ID))
            {
                //if (SyncDebugSystem.IsFilter(typeof(T).Name))
                //{
                //    Debug.Log("在回滚创建列表 " + list[i].ID);
                //}
                EntityBase entity = m_world.GetDispatchDestroyCache(list[i].ID);
                entity.ChangeComp((T)list[i].DeepCopy());
            }
            else if(m_world.GetIsExistDispatchCreateCache(list[i].ID))
            {
                //if (SyncDebugSystem.IsFilter(typeof(T).Name))
                //{
                //    Debug.Log("在回滚删除列表 " + list[i].ID);
                //}
                EntityBase entity = m_world.GetDispatchCreateCache(list[i].ID);
                entity.ChangeComp((T)list[i].DeepCopy());
            }
            else
            {
                //if (SyncDebugSystem.IsFilter(typeof(T).Name))
                //{
                //    Debug.Log("没有找到回滚对象 " + list[i].ID + " frame " + frame);
                //}
            }

            //if (SyncDebugSystem.IsFilter(typeof(T).Name))
            //{
            //    Debug.Log("数据回滚 ID：" + list[i].ID + " frame:" + list[i].Frame + " conent:" + Serializer.Serialize(list[i]));
            //}

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
    public override void PrintRecord(int id)
    {
        //RecordComponent<T> rc = m_world.GetSingletonComp<RecordComponent<T>>();

        //string content = "compName : " + typeof(T).Name + "\n";
        //for (int i = 0; i < rc.m_record.Count; i++)
        //{
        //    if(id == -1 || rc.m_record[i].ID == id)
        //    {
        //        content += " ID:" + rc.m_record[i].ID + " Frame:" + rc.m_record[i].Frame + " content:" + Serializer.Serialize(rc.m_record[i]) + "\n";
        //    }
        //}
        //Debug.LogWarning("PrintRecord:" + content);
    }

    public override void Record(int frame, EntityBase entity)
    {
        throw new NotImplementedException();
    }
}
