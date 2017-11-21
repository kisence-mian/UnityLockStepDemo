using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EntityRecordComponent :SingletonComponent
{
    public List<EntityRecordInfo> m_list = new List<EntityRecordInfo>();

    public bool GetReordIsExist(int frame, int ID,/* string systemName, RecordEntityTiming timing,*/ EntityChangeType changeType)
    {
        for (int i = 0; i < m_list.Count; i++)
        {
            if (m_list[i].frame == frame
                && m_list[i].id == ID
                //&& m_list[i].systemName == systemName
                //&& m_list[i].timing == timing
                && m_list[i].changeType == changeType)
            {
                return true;
            }
        }

        return false;
    }

    public EntityRecordInfo GetReord(int frame, int ID, /*string systemName, RecordEntityTiming timing, */EntityChangeType changeType)
    {
        for (int i = 0; i < m_list.Count; i++)
        {
            if (m_list[i].frame == frame
                && m_list[i].id == ID
                //&& m_list[i].systemName == systemName
                //&& m_list[i].timing == timing
                && m_list[i].changeType == changeType)
            {
                return m_list[i];
            }
        }

        throw new Exception("Not Find EntityRecordInfo by : id -> " + ID + " frame " + frame + " changeType " + changeType /*+ " systemName " + systemName + " timing " + timing*/);
    }
}

public struct EntityRecordInfo
{
    public int frame;
    public int id;
    //public string systemName;
    //public RecordEntityTiming timing;
    public EntityChangeType changeType;

    public List<ComponentBase> compList;

    public void SaveComp(EntityBase entity)
    {
        compList = new List<ComponentBase>();

        foreach (var item in entity.CompDict)
        {
            if(item.Value is MomentComponentBase)
            {
                MomentComponentBase mc = (MomentComponentBase)item.Value;
                compList.Add(mc.DeepCopy());
            }
            else
            {
                compList.Add(item.Value);
            }

        }
    }
}

public enum EntityChangeType
{
    Create,
    Destroy,
}

public enum RecordEntityTiming
{
    BeforeFixedUpdate,
    FixedUpdate,
    LaterFixedUpdate,
    Other,
}

