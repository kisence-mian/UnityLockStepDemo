using Protocol;
using System.Collections.Generic;

public class RecordComponent : SingletonComponent
{
    public List<ChangeRecordInfo> m_changeCache = new List<ChangeRecordInfo>();

    public List<ServiceMessageInfo> m_messageList = new List<ServiceMessageInfo>(); 
    public List<RecordInfo> m_recordList = new List<RecordInfo>();

    public void ClearCache()
    {
        m_changeCache.Clear();
    }
}

public class RecordInfo
{
    public int frame;
    public List<ChangeRecordInfo> m_changeData = new List<ChangeRecordInfo>(); //所有本地改动
}

public struct ChangeRecordInfo
{
    public ChangeType m_type;
    public int m_EnityID;
    public string m_compName;
    public ComponentBase m_comp;
}

public struct ServiceMessageInfo
{
    public int m_frame;
    public MessageType m_type;
    public SyncModule m_msg;
}

public enum ChangeType
{
    AddComp,
    RemoveComp,
    ChangeComp,

    CreateEntity,
    DestroyEntity,
}

public enum MessageType
{
    SyncEntity,
    DestroyEntity,
    ChangeSingletonComponent
}