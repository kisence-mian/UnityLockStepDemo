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
    public EntityChangeType changeType;
    public int id;
    public List<CompRecordInfo> compList = new List<CompRecordInfo>();
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
