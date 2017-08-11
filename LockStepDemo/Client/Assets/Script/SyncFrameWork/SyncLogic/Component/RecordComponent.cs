using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RecordComponent : SingletonComponent
{
    public PlayerCommandBase m_inputCache;
    public List<ChangeRecordInfo> m_changeCache = new List<ChangeRecordInfo>();

    public List<RecordInfo> m_recordList = new List<RecordInfo>();

    public void ClearCache()
    {
        m_changeCache.Clear();
    }
}

public class RecordInfo
{
    public int frame;

    public PlayerCommandBase m_inputCmd;
    public List<ChangeRecordInfo> m_changeData = new List<ChangeRecordInfo>();
}

public struct ChangeRecordInfo
{
    public ChangeType m_type;
    public int m_EnityID;
    public string m_compName;
    public ComponentBase m_comp;
}

public enum ChangeType
{
    AddComp,
    RemoveComp,
    ChangeComp,

    CreateEntity,
    DestroyEntity,
}