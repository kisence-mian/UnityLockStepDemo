using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EntityRecordComponent :SingletonComponent
{
    public List<EntityRecordInfo> m_list = new List<EntityRecordInfo>();

    public EntityRecordInfo GetReord(int frame,int ID, EntityChangeType changeType)
    {
        for (int i = 0; i < m_list.Count; i++)
        {
            if(m_list[i].frame == frame
                && m_list[i].id == ID
                && m_list[i].changeType == changeType)
            {
                return m_list[i];
            }
        }

        return null;
    }
}

public class EntityRecordInfo
{
    public int frame;
    public int id;
    public EntityChangeType changeType;
    
    public List<ComponentBase> compList = new List<ComponentBase>();

    public void SaveComp(EntityBase entity)
    {
        foreach(var item in entity.m_compDict)
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

public struct CompRecordInfo
{
    public string compName;
    public ComponentBase comp;
}

public enum EntityChangeType
{
    Create,
    Destroy,
}
