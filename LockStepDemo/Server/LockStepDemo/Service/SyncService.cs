using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Protocol;
using LockStepDemo.ServiceLogic;
using LockStepDemo.GameLogic.Component;

namespace LockStepDemo.Service
{
    class SyncService : AppServer<SyncSession,ProtocolRequestBase>
    {
        int updateInterval = 1000; //世界更新间隔ms
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

            m_world = WorldManager.CreateWorld<DemoWorld>();
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
            Debug.Log("SyncService OnSessionClosed");

            base.OnSessionClosed(session, reason);
        }

        int id = 0;
        protected override void OnNewSessionConnected(SyncSession session)
        {
            Debug.Log("SyncService OnNewSessionConnected");

            base.OnNewSessionConnected(session);

            ConnectionComponent conn = new ConnectionComponent();
            conn.m_session = session;

            session.m_gameWorld = m_world;

            PlayerComponent pc = new PlayerComponent();
            WaitSyncComponent ws = new WaitSyncComponent();

            ViewComponent vc = new ViewComponent();
            AssetComponent ac = new AssetComponent();
            ac.m_assetName = "Cube";

            EntityBase entity = m_world.CreateEntity(id++);
            entity.AddComp(conn);
            entity.AddComp(pc);
            entity.AddComp(ws);
            entity.AddComp(vc);
            entity.AddComp(ac);
        }
    }
}
