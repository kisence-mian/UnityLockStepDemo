using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Protocol;


public class SyncService : AppServer<SyncSession, ProtocolRequestBase>
{
    int updateInterval = 100; //世界更新间隔ms TODO 读取配置

    public SessionCreateHandle OnSessionCreate;
    public SessionCloseHandle OnSessionClose;
    public PlayerHandle OnPlayerLogin;
    public PlayerHandle OnPlayerLogout;

    LoginService loginService = new LoginService();
    MatchService matchService = new MatchService();
    ReConnectService reConnectService = new ReConnectService();
    SelectCharcterService selectCharacterService = new SelectCharcterService();

    public SyncService() : base(new ProtocolReceiveFilterFactory())
    {

    }

    protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
    {
        //TODO 读取配置设置isDebug
        Debug.SetLogger(Logger, true);
        Debug.Log("SyncService Setup");

        Debug.Log(MD5Tool.GetStringToHash("123456PlayerHaHa").ToString());

        DataBaseService.Init();

        matchService.Init(this);
        loginService.Init(this);
        reConnectService.Init(this);
        selectCharacterService.Init(this);

        CommandMessageService<CommandComponent>.Init();
        GameMessageService.Init();

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
        OnSessionClose?.Invoke(session, reason);
        base.OnSessionClosed(session, reason);
    }

    protected override void OnNewSessionConnected(SyncSession session)
    {
        Debug.Log("SyncService OnNewSessionConnected " + session.SessionID);
        OnSessionCreate?.Invoke(session);
        base.OnNewSessionConnected(session);
    }
}

public delegate void SessionCreateHandle(SyncSession session);
public delegate void SessionCloseHandle(SyncSession session, CloseReason reason);
public delegate void PlayerHandle(Player player);

