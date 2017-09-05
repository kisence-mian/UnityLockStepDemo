using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EntityRecordComponent :SingletonComponent
{
    public List<EntityRecordInfo> m_list = new List<EntityRecordInfo>();
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
