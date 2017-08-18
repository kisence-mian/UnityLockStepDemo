using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Protocol;
using LockStepDemo.ServiceLogic;
using LockStepDemo.GameLogic.Component;
using LockStepDemo.Service.Game;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
        int updateInterval = 250; //世界更新间隔ms
        WorldBase m_world;

        public SyncService() : base(new ProtocolReceiveFilterFactory())
        {
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

            m_world = WorldManager.CreateWorld<DemoWorld>();
            m_world.IsStart = true;

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

            if(session.m_connect != null)
            {
                m_world.DestroyEntity(session.m_connect.Entity.ID);
            }
        }

        protected override void OnNewSessionConnected(SyncSession session)
        {
            Debug.Log("SyncService OnNewSessionConnected " + session.SessionID);

            base.OnNewSessionConnected(session);

            WaitSyncComponent wsc = new WaitSyncComponent();
            ConnectionComponent conn = new ConnectionComponent();
            conn.m_session = session;

            PlayerComponent pc = new PlayerComponent();
            CommandComponent cc = new CommandComponent();

            ViewComponent vc  = new ViewComponent();
            AssetComponent ac = new AssetComponent();
            MoveComponent mc  = new MoveComponent();
            ac.m_assetName = "Cube";

            m_world.CreateEntity(conn, pc, vc, ac, cc, mc, wsc);

            session.m_connect = conn;
        }
    }
}
