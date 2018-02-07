using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Protocol;
using System;

public class SyncService : AppServer<SyncSession, ProtocolRequestBase>
{
    int updateInterval = 100; //世界更新间隔ms TODO 读取配置

    public SessionCreateHandle OnSessionCreate;
    public SessionCloseHandle OnSessionClose;
    public PlayerHandle OnPlayerLogin;
    public PlayerHandle OnPlayerLogout;

    TimerService m_timerService = new TimerService();
    LoginService loginService = new LoginService();
    MatchService matchService = new MatchService();
    ReConnectService reConnectService = new ReConnectService();
    ShopService selectCharacterService = new ShopService();
    SettlementService settlementService = new SettlementService();
    DataBaseService dataBaseService = new DataBaseService();

    AIService m_aiService = new AIService();

    MsgTestService msgTestService = new MsgTestService();

    public SyncService() : base(new ProtocolReceiveFilterFactory())
    {

    }

    protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
    {
        //TODO 读取配置设置isDebug
        Debug.SetLogger(Logger, true);

        try
        {
            updateInterval = int.Parse(config.Options.Get("UpdateInterval"));
            SyncDebugSystem.isDebug = bool.Parse(config.Options.Get("IsDebug"));
        }
        catch{ }

        Debug.Log("SyncService Setup Mode: " + config.Mode 
            + "\nupdateInterval " + updateInterval
            + "\nisDebug " + SyncDebugSystem.isDebug);

        dataBaseService.Init(this, config);

        m_timerService.Init(this, config);
        matchService.Init(this, config);
        loginService.Init(this, config);
        reConnectService.Init(this, config);
        selectCharacterService.Init(this, config);
        settlementService.Init(this, config);

        m_aiService.Init(this,config);

        msgTestService.Init(this, config);

        CommandMessageService<CommandComponent>.Init();

        UpdateEngine.Init(updateInterval);

        return base.Setup(rootConfig, config);
    }

    protected override void OnStarted()
    {
        Debug.Log("SyncService OnStarted");
        base.OnStarted();
    }

    protected override bool RegisterSession(string sessionID, SyncSession appSession)
    {
        Debug.Log("SyncService RegisterSession");
        return base.RegisterSession(sessionID, appSession);
    }

    protected override void OnSessionClosed(SyncSession session, CloseReason reason)
    {
        Debug.Log("SyncService OnSessionClosed " + session.SessionID);
        try
        {
            base.OnSessionClosed(session, reason);
            OnSessionClose?.Invoke(session, reason);
        }
        catch (Exception e)
        {
            Debug.LogError("OnSessionClosed Exception " + e.ToString());
        }
    }

    protected override void OnNewSessionConnected(SyncSession session)
    {
        Debug.Log("SyncService OnNewSessionConnected " + session.SessionID);

        try
        {
            base.OnNewSessionConnected(session);
            OnSessionCreate?.Invoke(session);
        }
        catch (Exception e)
        {
            Debug.LogError("OnNewSessionConnected Exception " + e.ToString());
        }
    }
    }

public delegate void SessionCreateHandle(SyncSession session);
public delegate void SessionCloseHandle(SyncSession session, CloseReason reason);
public delegate void PlayerHandle(Player player);

