using Protocol;
using System.Collections.Generic;

public class RecordComponent : SingletonComponent
{
    public PlayerCommandBase m_inputCache;
    public List<ChangeRecordInfo> m_changeCache = new List<ChangeRecordInfo>();

    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();  //其他人的输入
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

    public PlayerCommandBase m_inputCmd; //玩家自己的输入
    public List<PlayerCommandBase> m_forecastList = new List<PlayerCommandBase>(); //本地预测输入
    public List<PlayerCommandBase> m_commandList = new List<PlayerCommandBase>();  //其他人的输入

    public List<ChangeRecordInfo> m_changeData = new List<ChangeRecordInfo>(); //所有本地改动

    public PlayerCommandBase GetForecastCmd(int id)
    {
        for (int i = 0; i < m_forecastList.Count; i++)
        {
            if(m_forecastList[i].id == id)
            {
                return m_forecastList[i];
            }
        }

        throw new System.Exception("GetForecastCmd Exception not find cmd by " + id);
    }
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