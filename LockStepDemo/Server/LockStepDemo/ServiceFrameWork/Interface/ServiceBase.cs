using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class ServiceBase
{
    public SyncService m_service;

    public void Init(SyncService service, IServerConfig config)
    {
        m_service = service;

        m_service.OnSessionCreate += OnSessionCreate;
        m_service.OnSessionClose += OnSessionClose;
        m_service.OnPlayerLogin += OnPlayerLogin;
        m_service.OnPlayerLogout += OnPlayerLogout;

        OnInit(config);
    }

    protected virtual void OnInit(IServerConfig config)
    {

    }

    protected virtual void OnSessionCreate(SyncSession session)
    {

    }

    protected virtual void OnSessionClose(SyncSession session, CloseReason reason)
    {

    }

    protected virtual void OnPlayerLogin(Player player)
    {

    }

    protected virtual void OnPlayerLogout(Player player)
    {

    }
}
