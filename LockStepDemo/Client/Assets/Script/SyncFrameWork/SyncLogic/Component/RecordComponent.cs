using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RecordComponent : SingletonComponent
{
    public PlayerCommandBase m_inputCache;
    public List<ComponentRecordInfo> m_changeCache = new List<ComponentRecordInfo>();
    public List<ComponentRecordInfo> m_AddCompDataCache = new List<ComponentRecordInfo>();
    public List<ComponentRecordInfo> m_RemoveCompDataCache = new List<ComponentRecordInfo>();

    public List<EntityRecordInfo> m_CreateEntitypDataCache = new List<EntityRecordInfo>();
    public List<EntityRecordInfo> m_DestroyEntitypDataCache = new List<EntityRecordInfo>();

    public List<RecordInfo> m_recordList = new List<RecordInfo>();
}

public class RecordInfo
{
    public int frame;

    public PlayerCommandBase m_inputCmd;
    public List<ComponentRecordInfo> m_ChangeCompData = new List<ComponentRecordInfo>();
    public List<ComponentRecordInfo> m_AddCompData = new List<ComponentRecordInfo>();
    public List<ComponentRecordInfo> m_RemoveCompData = new List<ComponentRecordInfo>();

    public List<EntityRecordInfo> m_CreateEntitypData = new List<EntityRecordInfo>();
    public List<EntityRecordInfo> m_DestroyEntitypData = new List<EntityRecordInfo>();
}

public class ComponentRecordInfo
{
    public string m_name;
    public ComponentBase m_comp;
}

public class EntityRecordInfo
{
    public int m_id;
    public ComponentBase m_comp;
}