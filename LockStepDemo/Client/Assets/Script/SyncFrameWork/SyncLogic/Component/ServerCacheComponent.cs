using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ServerCacheComponent : SingletonComponent
{
    public List<ServiceMessageInfo> m_messageList = new List<ServiceMessageInfo>();
}

public enum MessageType
{
    SyncEntity,
    DestroyEntity,
    ChangeSingletonComponent
}

public struct ServiceMessageInfo
{
    public int m_frame;
    public MessageType m_type;
    public SyncModule m_msg;
}
