using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Protocol;
using LockStepDemo.Service.Game;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
        int updateInterval = 200; //世界更新间隔ms
        WorldBase m_world;

        LoginService loginService = new LoginService();
        MatchService matchService = new MatchService();
        ReConnectService reConnectService = new ReConnectService();

        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {
            DataBaseService.Init();

            matchService.Init();
            loginService.Init();
            reConnectService.Init();
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            //TODO 读取配置设置isDebug
            Debug.SetLogger(Logger,true);
            Debug.Log("SyncService Setup");

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Debug.Log("SyncService OnStarted");

            GameMessageService<CommandComponent>.Init();

            //m_world = WorldManager.CreateWorld<DemoWorld>();
            //m_world.IsStart = true;
            //m_world.SyncRule = SyncRule.Status;

            UpdateEngine.Init(updateInterval);

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

            base.OnSessionClosed(session, reason);

            //掉线玩家维护一个id与world的映射，用以重连
            if (session.m_connect != null)
            {
                reConnectService.AddRecord(session.m_connect);
                //m_world.DestroyEntity(session.m_connect.Entity.ID);
            }
        }

        protected override void OnNewSessionConnected(SyncSession session)
        {
            Debug.Log("SyncService OnNewSessionConnected " + session.SessionID);

            base.OnNewSessionConnected(session);

            //ConnectionComponent conn = new ConnectionComponent();
            //conn.m_session = session;

            //m_world.CreateEntity(conn);

            //session.m_connect = conn;
        }
    }
}
